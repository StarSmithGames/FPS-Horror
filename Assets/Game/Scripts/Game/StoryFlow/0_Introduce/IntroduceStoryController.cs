using Cysharp.Threading.Tasks;
using Moduls.Light;
using System;
using UnityEngine;

namespace Game.StoryFlow.Introduce
{
    public sealed class IntroduceStoryController : StoryFlowController
    {
        public IntroduceLevelObject View { get; private set; }
        
        public IntroduceStoryController( IntroduceLevelObject view )
        {
            View = view ?? throw new ArgumentNullException( nameof(view) );
        }

        public override void Initialize()
        {
            View.LightTrigger.OnTriggerEntered += PlayerEnteredLightTrigger;
        }

        public override void Dispose()
        {
            View.LightTrigger.OnTriggerEntered -= PlayerEnteredLightTrigger;
        }

        private void PlayerEnteredLightTrigger( Collider colider )
        {
            View.LightTrigger.OnTriggerEntered -= PlayerEnteredLightTrigger;
            View.LightTrigger.Enable( false );
            
            LightFlicker.FlickerAndTurnOff( View.AllLights, View.LightFlickerSettings ).Forget();
        }
    }
}