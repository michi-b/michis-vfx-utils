using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Michis.VfxUtils.Base;
using Michis.VfxUtils.CloneCache.Interfaces;
using Michis.VfxUtils.CloneCache.Internal;
using UnityEngine;

namespace Michis.VfxUtils.CloneCache.Abstract
{
    public abstract class CloneCashable<TInstance> : CloneCashable, ICloneCashable<TInstance>
        where TInstance : CloneCashable<TInstance>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TInstance BorrowInstance()
        {
            ICloneCashable iCloneCashable = this;
            return (TInstance)iCloneCashable.BorrowInstance();
        }
    }

    public abstract class CloneCashable : MBehaviour, ICachedClone, ICloneCashable
    {
        private CloneCashable _prefab;

        #region ICachedGameObject Implementation

        void ICachedClone.OnBorrow()
        {
            OnBorrow();
        }

        IEnumerator ICachedClone.OnReturn()
        {
            return OnReturn();
        }

        void ICachedClone.OnReturnImmediate()
        {
            OnReturnImmediate();
        }

        CloneCashable ICachedClone.Prefab
        {
            get => _prefab;
            set => _prefab = value;
        }

        #endregion

        #region ICachedGameObjectPrefab Implementation

        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        ICachedClone ICloneCashable.BorrowInstance()
        {
#if UNITY_EDITOR
            Debug.Assert(Application.isPlaying);
            Debug.Assert(_prefab is null);
#endif
            return UniversalCloneCache.Instance.Borrow(this);
        }

        #endregion

        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Return()
        {
#if UNITY_EDITOR
            Debug.Assert(Application.isPlaying);
            Debug.Assert(_prefab is not null);
#endif
            UniversalCloneCache.Instance.Return(this);
        }

        public void ReturnImmediate()
        {
#if UNITY_EDITOR
            Debug.Assert(Application.isPlaying);
            Debug.Assert(_prefab is not null);
#endif
            UniversalCloneCache.Instance.ReturnImmediate(this);
        }

        protected abstract void OnBorrow();
        protected abstract IEnumerator OnReturn();
        protected abstract void OnReturnImmediate();
    }
}