using Game.Core.Player;
using Game.Core.UI;
using Game.Core.UI.MenuScreen;
using Game.Managers.CursorManager;
using Game.Managers.InputManager;
using Game.StoryFlow;
using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class GameBoostrap : IInitializable, IDisposable
    {
        private Camera _cameraUI;
        private MenuScreenViewModel _menuScreenViewModel;
        
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

            _menuScreenViewModel = _uiRootGame.ScreenAggregator.GetOrCreateIfNotExist< MenuScreenViewModel >();
            _menuScreenViewModel.ShowView();
        }

        public void Dispose()
        {
            InputManager.Dispose();
        }

        public void Start()
        {
            _menuScreenViewModel.HideView();
            CursorManager.Disable();
            
            #if UNITY_EDITOR
            if ( _gameConfig.EditorLevelPrefab != null )
            {
                var level = _diContainer.InstantiatePrefabForComponent< LevelObject >( _gameConfig.EditorLevelPrefab );
                _storyManager.CreateAndStartStory( level );

                var playerInstaller = _diContainer.InstantiatePrefabForComponent< PlayerInstaller >( _gameConfig.PlayerPrefab );
                var player = playerInstaller.GetComponentInChildren< PlayerObject >();
                player.Controller.Initialize();
                player.transform.position = level.PlayerPoint.transform.position;
            }
            #endif

        }
    }
}