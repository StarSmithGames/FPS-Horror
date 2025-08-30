namespace Game.SceneSystem
{
    public interface IProgressHandle
    {
        bool IsDone { get; }
        
        float GetProgress();
    }
}