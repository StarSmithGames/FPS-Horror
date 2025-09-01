using Game.Core.Player;
using Game.Core.UI;
using Game.Core.UI.MenuScreen;
using Game.Managers.CursorManager;
using Game.Managers.GameManager;
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
        private readonly GameManager _gameManager;
        private readonly StoryManager _storyManager;

        public GameBoostrap(
            DiContainer diContainer,
            UIRootGame uiRootGame,
            GameConfig gameConfig,
            GameManager gameManager,
            StoryManager storyManager
            )
        {
            _diContainer = diContainer ?? throw new ArgumentNullException( nameof(diContainer) );
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            _gameConfig = gameConfig ?? throw new ArgumentNullException( nameof(gameConfig) );
            _gameManager = gameManager ?? throw new ArgumentNullException( nameof(gameManager) );
            _storyManager = storyManager ?? throw new ArgumentNullException( nameof(storyManager) );
        }

        public void Initialize()
        {
            _gameManager.SetState( GameState.Loading );
            
            InputManager.Initialize();

            _menuScreenViewModel = _uiRootGame.ScreenAggregator.GetOrCreateIfNotExist< MenuScreenViewModel >();
            _menuScreenViewModel.ShowView();
            
            _gameManager.SetState( GameState.Menu );
        }

        public void Dispose()
        {
            InputManager.Dispose();
        }

        public void Start()
        {
            _gameManager.SetState( GameState.Game );
            
            _menuScreenViewModel.HideView();
            CursorManager.Disable();
            
            #if UNITY_EDITOR
            if ( _gameConfig.EditorLevelPrefab != null )
            {
                var level = _diContainer.InstantiatePrefabForComponent< LevelObject >( _gameConfig.EditorLevelPrefab );
                _storyManager.CreateAndStartStory( level );

                var playerInstaller = _diContainer.InstantiatePrefabForComponent< PlayerInstaller >( _gameConfig.PlayerPrefab );
                var player = playerInstaller.GetComponentInChildren< PlayerObject >();
                player.Controller.Teleport( level.PlayerPoint.transform.position, level.PlayerPoint.transform.rotation.eulerAngles );
                player.Controller.Initialize();
            }
            #endif

        }
    }
}