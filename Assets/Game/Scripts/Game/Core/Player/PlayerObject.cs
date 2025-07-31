using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerObject : MonoBehaviour
    {
        [ field: SerializeField ] public Rigidbody Rigidbody { get; private set; }
        [ field: SerializeField ] public CapsuleCollider CapsuleCollider { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public Camera FirstPersonCamera { get; private set; }

        public PlayerController Controller { get; private set; }
        
        public void SetController( PlayerController controller )
        {
            Controller = controller;
        }
    }
}