using Game.Core.Player;
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
        private readonly StoryManager _storyManager;
        
        public GameplayPipeline( StoryManager storyManager )
        {
            _storyManager = storyManager ?? throw new ArgumentNullException( nameof(storyManager) );
        }
        
        public void Initialize()
        {
            CursorManager.Disable();
            InputManager.Initialize();

            _storyManager.CreateAndStartStory( GameObject.FindAnyObjectByType< IntroduceLevelObject >() );
            GameObject.FindAnyObjectByType< PlayerObject >().Controller.Initialize();
        }

        public void Dispose()
        {
            InputManager.Dispose();
        }
    }
}