using Game.StoryFlow.Introduce;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.StoryFlow
{
    public sealed class StoryManager
    {
        private readonly Dictionary< Type, Type > _stories = new()
        {
            { typeof(IntroduceLevelObject), typeof(IntroduceStoryController) }
        };
     
        public StoryFlowController CurrentStory { get; private set; }

        private readonly DiContainer _diContainer;
        
        public StoryManager( DiContainer diContainer )
        {
            _diContainer = diContainer ?? throw new ArgumentNullException( nameof(diContainer) );
        }
        
        public void CreateAndStartStory( LevelObject level )
        {
            CurrentStory = (StoryFlowController)_diContainer.Instantiate( _stories[ level.GetType() ], new[] { level } );
            CurrentStory.Initialize();
        }
    }
}