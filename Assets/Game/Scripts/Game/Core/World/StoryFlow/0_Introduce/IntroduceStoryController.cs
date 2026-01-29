using Cysharp.Threading.Tasks;
using Moduls.Light;
using System;
using System.Threading;
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

            LightDown().Forget();
        }

        private async UniTask LightDown( CancellationToken cancellationToken = default )
        {
            //2.8 sec
            View.SoundLightDown.time = 0f;
            View.SoundLightDown.Play();
            
            // await UniTask.WaitForSeconds( 1.7f, cancellationToken: cancellationToken );
            await LightFlicker.FlickerAndGrowingIntensity( View.Corridor.CeilLamps, View.LightFlickerSettings );
            LampUtils.SetLightsEnabled( View.MainRoom.CeilLamps, false );
            foreach ( var computer in View.Computers )
            {
                computer.EnableComputer( false );
            }
            
            View.LightTrigger.Enable( false );
        }
    }
}