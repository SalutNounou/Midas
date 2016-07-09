public interface ISourceEngine
{
    bool ShouldWork { get; }
    void DoCycle();
    void StopEngine();
}