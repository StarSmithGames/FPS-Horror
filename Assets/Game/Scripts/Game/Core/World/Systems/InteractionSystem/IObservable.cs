namespace Game.Core.World.InteractionSystem
{
    public interface IObservable
    {
        bool IsCollidersEnabled { get; }
        
        void StartObserve();
        void Observe();
        void EndObserve();
        
        void EnableCollider( bool trigger );
    }
}