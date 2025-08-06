using Moduls.Light;
using Moduls.Physics;
using System.Collections.Generic;
using UnityEngine;

namespace Game.StoryFlow.Introduce
{
    public sealed class IntroduceLevelObject : LevelObject
    {
        [ field: Header( "Story" ) ]
        [ field: SerializeField ] public Trigger LightTrigger { get; private set; }
        [ field: SerializeField ] public List< Light > AllLights { get; private set; } = new();
        [ field: SerializeField ] public LightFlickerSettings LightFlickerSettings { get; private set; }
    }
}