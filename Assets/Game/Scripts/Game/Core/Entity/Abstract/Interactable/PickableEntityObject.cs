using Game.Core.World.InteractionSystem;
using UnityEngine;

namespace Game.Core.Entity
{
    public abstract class PickableEntityObject : InteractableEntityObject, IPickable
    {
        public override void Interact()
        {
            Debug.LogError( "Interact" );
        }
    }
}