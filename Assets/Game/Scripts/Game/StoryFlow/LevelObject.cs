using Game.Core.World.SpawnSystem;
using UnityEngine;

namespace Game.StoryFlow
{
    public class LevelObject : MonoBehaviour
    {
        [ field: Header( "Settings" ) ]
        [ field: SerializeField ] public SpawnPlayerPoint PlayerPoint { get; private set; }
    }
}