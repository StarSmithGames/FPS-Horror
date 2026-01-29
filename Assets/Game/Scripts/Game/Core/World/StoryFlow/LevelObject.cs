using Game.Core.Entity;
using Game.Core.World.SpawnSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Game.StoryFlow
{
    public class LevelObject : MonoBehaviour
    {
        [ field: Header( "Settings" ) ]
        [ field: SerializeField ] public SpawnPlayerPoint PlayerPoint { get; private set; }
        [ field: SerializeField ] public List< ComputerObject > Computers { get; private set; } = new();
    }
}