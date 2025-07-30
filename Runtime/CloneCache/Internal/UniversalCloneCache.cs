using System.Collections.Generic;
using Michis.VfxUtils.Base;
using Michis.VfxUtils.CloneCache.Abstract;
using Michis.VfxUtils.CloneCache.Interfaces;
using UnityEngine;

namespace Michis.VfxUtils.CloneCache.Internal
{
    [ExecuteAlways]
    [AddComponentMenu("")]
    internal sealed class UniversalCloneCache : MBehaviour
    {
        private readonly Dictionary<CloneCashable, IndividualCloneCache> _caches = new();

        public ICachedClone Borrow(CloneCashable prefab)
        {
            Debug.Assert(Application.isPlaying);
            return GetCache(prefab).Borrow();
        }

        public void Return(CloneCashable instance)
        {
            ICachedClone iCachedClone = instance;
            GetCache(iCachedClone.Prefab).Return(instance);
        }
        
        public void ReturnImmediate(CloneCashable instance)
        {
            Debug.Assert(Application.isPlaying);
            ICachedClone iCachedClone = instance;
            GetCache(iCachedClone.Prefab).ReturnImmediate(instance);
        }

        public static UniversalCloneCache Instance
        {
            get
            {
                Debug.Assert(Application.isPlaying);
                if (_instance is null)
                {
                    new GameObject(nameof(UniversalCloneCache)).AddComponent<UniversalCloneCache>();
                    DontDestroyOnLoad(_instance);
                }

                Debug.Assert(_instance is not null);
                return _instance;
            }
        }

        private void OnEnable()
        {
            MBehaviourUtility.ProhibitInEditMode(this);
#if UNITY_EDITOR
            Debug.Assert(_instance is null, $"There may only be one {nameof(UniversalCloneCache)} instance loaded at a time.");
#endif
            _instance = this;
        }

        private IndividualCloneCache GetCache(CloneCashable prefab)
        {
            if (!_caches.TryGetValue(prefab, out IndividualCloneCache cache))
            {
                cache = new GameObject().AddComponent<IndividualCloneCache>();
                cache.transform.SetParent(transform, false);
                cache.Initialize(prefab);
                _caches[prefab] = cache;
            }

            return cache;
        }

        private static UniversalCloneCache _instance;
    }
}