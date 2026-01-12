using Game.Core.Player;
using Game.StoryFlow;
using UnityEngine;

namespace Game
{
    [ CreateAssetMenu( fileName = "GameConfig", menuName = "Game/GameConfig" ) ]
    public sealed class GameConfig : ScriptableObject
    {
        [ field: SerializeField ] public PlayerInstaller PlayerPrefab { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public LevelObject LevelPrefab { get; private set; }
    }
}