namespace Midas.Engines.Engines
{
    public interface ISourceEngine
    {
        bool ShouldWork { get; }
        void DoCycle();
        void StopEngine();
    }
}