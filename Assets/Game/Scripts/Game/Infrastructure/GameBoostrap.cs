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
    public sealed class GameBoostrap : IInitializable, IDisposable
    {
        private readonly StoryManager _storyManager;
        private readonly UIRootGame _uiRootGame;

        public GameBoostrap( UIRootGame uiRootGame , StoryManager storyManager )
        {
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            _storyManager = storyManager ?? throw new ArgumentNullException( nameof(storyManager) );
        }

        public void Initialize()
        {
            InputManager.Initialize();
            CursorManager.Disable();
            
            _storyManager.CreateAndStartStory( GameObject.FindAnyObjectByType< IntroduceLevelObject >() );
            GameObject.FindAnyObjectByType< PlayerObject >().Controller.Initialize();
            
            // _uiRootGame.ScreenAggregator.ShowAndCreateIfNotExist< MenuScreenViewModel >();
        }

        public void Dispose()
        {
            InputManager.Dispose();
        }
    }
}