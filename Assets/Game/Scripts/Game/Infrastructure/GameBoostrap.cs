using Game.Core.World.EntityManager;
using Game.Core.Player;
using Game.Core.UI;
using Game.Core.UI.MenuScreen;
using Game.Managers.CursorManager;
using Game.Managers.GameManager;
using Game.Managers.InputManager;
using Game.StoryFlow;
using PuzzlescapeGames.Services.UITransitionService;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        private readonly EntityManager _entityManager;
        private readonly StoryManager _storyManager;
        private readonly UITransitionService _uiTransitionService;

        public GameBoostrap(
            DiContainer diContainer,
            UIRootGame uiRootGame,
            GameConfig gameConfig,
            GameManager gameManager,
            EntityManager entityManager,
            StoryManager storyManager,
            UITransitionService uiTransitionService
            )
        {
            _diContainer = diContainer ?? throw new ArgumentNullException( nameof(diContainer) );
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            _gameConfig = gameConfig ?? throw new ArgumentNullException( nameof(gameConfig) );
            _gameManager = gameManager ?? throw new ArgumentNullException( nameof(gameManager) );
            _entityManager = entityManager ?? throw new ArgumentNullException( nameof(entityManager) );
            _storyManager = storyManager ?? throw new ArgumentNullException( nameof(storyManager) );
            _uiTransitionService = uiTransitionService ?? throw new ArgumentNullException( nameof(uiTransitionService) );
        }

        public void Initialize()
        {
            _gameManager.SetState( GameState.Loading );
            
            InputManager.Initialize();

            if ( !IsTestScene() )
            {
                _menuScreenViewModel = _uiRootGame.ScreenAggregator.GetOrCreateIfNotExist< MenuScreenViewModel >();
                _menuScreenViewModel.ShowView();
                
                _gameManager.SetState( GameState.Menu );
            }
            else
            {
                QuickStart();
            }
        }

        public void Dispose()
        {
            InputManager.Dispose();
        }

        public void Start()
        {
            _gameManager.SetState( GameState.Game );
         
            _uiTransitionService.LoadThroughBlank( onShowed: () =>
            {
                try
                {
                    _menuScreenViewModel.HideView();
                    CursorManager.Disable();

                    if ( !IsTestScene() )
                    {
                        if ( _gameConfig.EditorLevelPrefab != null )
                        {
                            var level = _diContainer.InstantiatePrefabForComponent< LevelObject >( _gameConfig.EditorLevelPrefab );
                            _storyManager.CreateAndStartStory( level );

                            var playerInstaller = _diContainer.InstantiatePrefabForComponent< PlayerInstaller >( _gameConfig.PlayerPrefab );
                            var player = playerInstaller.GetComponentInChildren< PlayerObject >();
                            player.Controller.Teleport( level.PlayerPoint.transform.position, level.PlayerPoint.transform.rotation.eulerAngles );
                            player.Controller.Initialize();
                            
                            _entityManager.SetPlayer( player );
                        }
                    }
                    
                }
                catch ( Exception e )
                {
                    Debug.LogError( e );
                }
            } );
        }

        private void QuickStart()
        {
            _gameManager.SetState( GameState.Game );
            CursorManager.Disable();

            LevelObject level = GameObject.FindAnyObjectByType< LevelObject >();
            _storyManager.CreateAndStartStory( level );

            var playerInstaller = _diContainer.InstantiatePrefabForComponent< PlayerInstaller >( _gameConfig.PlayerPrefab );
            var player = playerInstaller.GetComponentInChildren< PlayerObject >();
            player.Controller.Teleport( level.PlayerPoint.transform.position, level.PlayerPoint.transform.rotation.eulerAngles );
            player.Controller.Initialize();
            
            _entityManager.SetPlayer( player );
        }

        private bool IsTestScene()
        {
#if UNITY_EDITOR
            return SceneManager.GetActiveScene().name.Contains( "test", StringComparison.InvariantCultureIgnoreCase );
#endif
            return false;
        }
    }
}