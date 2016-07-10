namespace Midas.Source
{
    public interface ISourceEngine
    {
        bool ShouldWork { get; }
        void DoCycle();
        void StopEngine();
    }
}