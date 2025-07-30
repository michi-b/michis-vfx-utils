using System.Collections;
using Michis.VfxUtils.CloneCache.Abstract;
using UnityEngine;

namespace Michis.VfxUtils.CloneCache
{
    public class SimpleCached : CloneCashable<SimpleCached>
    {
        [SerializeField] private float _returnDelay = 0f;

        protected override void OnBorrow()
        {
            gameObject.SetActive(true);
        }

        protected override IEnumerator OnReturn()
        {
            yield return new WaitForSeconds(_returnDelay);
            gameObject.SetActive(false);
        }

        protected override void OnReturnImmediate()
        {
            gameObject.SetActive(false);
        }
    }
}