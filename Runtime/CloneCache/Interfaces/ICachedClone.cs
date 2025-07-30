using System.Collections;
using Michis.VfxUtils.CloneCache.Abstract;

namespace Michis.VfxUtils.CloneCache.Interfaces
{
    public interface ICachedClone
    {
        CloneCashable Prefab { get; set; }
        void OnBorrow();
        IEnumerator OnReturn();
        void OnReturnImmediate();
    }
}