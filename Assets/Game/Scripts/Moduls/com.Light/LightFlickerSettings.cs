using System.Collections.Generic;
using UnityEngine;

namespace Moduls.Light
{
    [ System.Serializable ]
    public sealed class LightFlickerSettings
    {
        [ field: SerializeField ] public float FlickerDuration { get; private set; } = 2f;
        [ field: SerializeField ] public float MinFlickerInterval { get; private set; } = 0.05f;
        [ field: SerializeField ] public float MaxFlickerInterval { get; private set; } = 0.2f;
    }
}