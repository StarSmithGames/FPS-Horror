using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Player
{
    [ System.Serializable ]
    public class FootStepSoundsGroup
    {
        [ field: Range( 0f, 1f ) ]
        [ field: SerializeField ] public float VolumeCommon { get; private set; } = 1f;
        [ field: Range( 0f, 1f ) ]
        [ field: SerializeField ] public float VolumeCrouching { get; private set; } = 1f;
        [ field: Range( 0f, 1f ) ]
        [ field: SerializeField ] public float VolumeSprinting { get; private set; } = 1f;
        [ field: SerializeField ] public List< AudioClip > Sounds { get; private set; } = new();
    }
}