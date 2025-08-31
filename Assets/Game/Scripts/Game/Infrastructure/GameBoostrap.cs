using Game.Core.Player;
using Game.Core.UI;
using Game.Core.UI.MenuScreen;
using Game.Managers.CursorManager;
using Game.Managers.InputManager;
using Game.StoryFlow;
using Game.StoryFlow.Introduce;
using System;
using UnityEngine;
using Zenject;
using ArgumentNullException = System.ArgumentNullException;

namespace Game
{
    public sealed class GameBoostrap : IInitializable, IDisposable
    {
        private readonly DiContainer _diContainer;
        private readonly UIRootGame _uiRootGame;
        private readonly GameConfig _gameConfig;
        private readonly StoryManager _storyManager;

        public GameBoostrap(
            DiContainer diContainer,
            UIRootGame uiRootGame,
            GameConfig gameConfig,
            StoryManager storyManager
            )
        {
            _diContainer = diContainer ?? throw new ArgumentNullException( nameof(diContainer) );
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            _gameConfig = gameConfig ?? throw new ArgumentNullException( nameof(gameConfig) );
            _storyManager = storyManager ?? throw new ArgumentNullException( nameof(storyManager) );
        }

        public void Initialize()
        {
            InputManager.Initialize();
            CursorManager.Disable();

            _uiRootGame.ScreenAggregator.ShowAndCreateIfNotExist< MenuScreenViewModel >();
        }

        public void Dispose()
        {
            InputManager.Dispose();
        }

        public void Start()
        {
            var level = _diContainer.InstantiatePrefabForComponent< IntroduceLevelObject >( _gameConfig.Level1Prefab );
            _storyManager.CreateAndStartStory( level );

            var player = GameObject.FindAnyObjectByType< PlayerObject >( FindObjectsInactive.Include );
            player.gameObject.SetActive( true );
            player.Controller.Initialize();
        }
    }
}