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

namespace Game
{
    public sealed class GameplayPipeline : IInitializable, IDisposable
    {
        private readonly UIRootGame _uiRootGame;
        private readonly StoryManager _storyManager;
        
        public GameplayPipeline( UIRootGame uiRootGame, StoryManager storyManager )
        {
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            _storyManager = storyManager ?? throw new ArgumentNullException( nameof(storyManager) );
        }
        
        public void Initialize()
        {
            InputManager.Initialize();
            
            _uiRootGame.ScreenAggregator.ShowAndCreateIfNotExist< MenuScreenViewModel >();

            // CursorManager.Disable();
            //
            // _storyManager.CreateAndStartStory( GameObject.FindAnyObjectByType< IntroduceLevelObject >() );
            // GameObject.FindAnyObjectByType< PlayerObject >().Controller.Initialize();
        }

        public void Dispose()
        {
            InputManager.Dispose();
        }
    }
}