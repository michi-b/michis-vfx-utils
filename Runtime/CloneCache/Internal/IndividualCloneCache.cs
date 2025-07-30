using System;
using System.Collections;
using System.Collections.Generic;
using Michis.VfxUtils.Base;
using Michis.VfxUtils.CloneCache.Abstract;
using Michis.VfxUtils.CloneCache.Interfaces;
using UnityEngine;

namespace Michis.VfxUtils.CloneCache.Internal
{
    [ExecuteAlways]
    internal sealed class IndividualCloneCache : MBehaviour
    {
        private const string InstanceNameFormat = "{0}_Instance_{1}";
        private const string CacheNameFormat = "{0}_Cache";
        private readonly Queue<CloneCashable> _availableInstances = new();
        private CloneCashable _prefab;
        private int _instanceCount = 0;
        private bool _isQuitting = false;

        public void Initialize(CloneCashable prefab)
        {
            _prefab = prefab;
            gameObject.name = string.Format(CacheNameFormat, prefab.gameObject.name);
        }

        public ICachedClone Borrow()
        {
            if (!_availableInstances.TryDequeue(out CloneCashable instance))
            {
                instance = Instantiate(_prefab, transform, false);
                instance.name = string.Format(InstanceNameFormat, _prefab.name, _instanceCount++);

                ICachedClone newICachedClone = instance;
                newICachedClone.Prefab = _prefab;
            }

            ICachedClone iCachedClone = instance;
            iCachedClone.OnBorrow();

            return instance;
        }

        public void Return(CloneCashable instance)
        {
            StartCoroutine(WaitAndReturn(instance));
        }

        private void Awake()
        {
            MBehaviourUtility.ProhibitInEditMode(this);
        }

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        private void OnDisable()
        {
            if (!_isQuitting)
            {
                throw new InvalidOperationException("This component may not be disabled while the application is still running.");
            }
        }

        private IEnumerator WaitAndReturn(CloneCashable instance)
        {
            ICachedClone iCachedClone = instance;
            yield return StartCoroutine(iCachedClone.OnReturn());
            instance.transform.SetParent(transform, false);
            _availableInstances.Enqueue(instance);
        }

        public void ReturnImmediate(CloneCashable instance)
        {
            ICachedClone iCachedClone = instance;
            iCachedClone.OnReturnImmediate();
            instance.transform.SetParent(transform, false);
            _availableInstances.Enqueue(instance);
        }
    }
}