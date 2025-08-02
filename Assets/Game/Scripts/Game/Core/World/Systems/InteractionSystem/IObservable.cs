namespace Game.Core.World.InteractionSystem
{
    public interface IObservable
    {
        void StartObserve();
        void Observe();
        void EndObserve();
    }
}