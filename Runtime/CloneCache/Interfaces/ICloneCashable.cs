namespace Michis.VfxUtils.CloneCache.Interfaces
{
    public interface ICloneCashable
    {
        ICachedClone BorrowInstance();
    }

    public interface ICloneCashable<out TBehaviour>
    {
        TBehaviour BorrowInstance();
    }
}