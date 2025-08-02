using UnityEngine;

namespace Game.Core.Player
{
    [ System.Serializable ]
    public sealed class CrouchSettings
    {
        [ field: SerializeField ] public Vector3 CrouchScale { get; private set; } = new( 1, 0.5f, 1 );
        [ field: Min( 0.01f ) ]
        [ field: SerializeField ] public float CrouchTransitionSpeed { get; private set; } = 0.33f;
    }
}