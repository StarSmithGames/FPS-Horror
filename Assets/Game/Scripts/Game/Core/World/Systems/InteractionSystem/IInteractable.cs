namespace Game.Core.World.InteractionSystem
{
    public interface IInteractable : IObservable
    {
        void Interact( IInteractor interactor );
    }
}