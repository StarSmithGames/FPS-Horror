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
        [ field: SerializeField ] public List< ItemObject > Items { get; private set; } = new();
        [ field: SerializeField ] public List< PuzzleObject > Puzzles { get; private set; } = new();
    }
}