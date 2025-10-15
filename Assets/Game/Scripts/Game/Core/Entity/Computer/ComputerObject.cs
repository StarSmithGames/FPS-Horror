using Game.Core.World.InteractionSystem;
using UnityEngine;

namespace Game.Core.Entity.Computer
{
    public sealed class ComputerObject : ObservableObject, IInteractable
    {
        [ SerializeField ] private ComputerCanvas _computerCanvas;
        
        public void Interact( IInteractor interactor )
        {
            
        }
    }
}