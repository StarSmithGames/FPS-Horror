using Game.StoryFlow.Introduce;
using UnityEngine;

namespace Game
{
    [ CreateAssetMenu( fileName = "GameConfig", menuName = "Game/GameConfig" ) ]
    public sealed class GameConfig : ScriptableObject
    {
        [ field: SerializeField ] public IntroduceLevelObject Level1Prefab { get; private set; }
    }
}