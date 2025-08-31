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
        #if UNITY_EDITOR
        [ field: SerializeField ] public LevelObject EditorLevelPrefab { get; private set; }
        #endif
    }
}