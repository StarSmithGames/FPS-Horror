using UnityEngine;

namespace Game.Core.Entity
{
    public sealed class LighterItemObject : ItemObject
    {
        [ field: Header( "Zippo" ) ]
        [ field: SerializeField ] public Animator Animator { get; private set; }
        [ field: SerializeField ] public ParticleSystem Flame { get; private set; }
        [ field: SerializeField ] public Light Light { get; private set; }
        [ field: SerializeField ] public AudioClip SoundOpen { get; private set; }
        [ field: SerializeField ] public AudioClip SoundClose { get; private set; }
        [ field: SerializeField ] public AudioClip SoundIgnite { get; private set; }
    }
}