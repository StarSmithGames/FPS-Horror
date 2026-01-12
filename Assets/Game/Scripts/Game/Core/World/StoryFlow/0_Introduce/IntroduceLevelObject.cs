using Game.Core.Environment;
using Moduls.Light;
using Moduls.Physics;
using UnityEngine;

namespace Game.StoryFlow.Introduce
{
    public sealed class IntroduceLevelObject : LevelObject
    {
        [ field: Header( "Story" ) ]
        [ field: SerializeField ] public Trigger LightTrigger { get; private set; }
        [ field: SerializeField ] public RoomObject Corridor { get; private set; }
        [ field: SerializeField ] public RoomObject MainRoom { get; private set; }
        [ field: SerializeField ] public AudioSource SoundLightDown { get; private set; }
        [ field: SerializeField ] public LightFlickerSettings LightFlickerSettings { get; private set; }
    }
}