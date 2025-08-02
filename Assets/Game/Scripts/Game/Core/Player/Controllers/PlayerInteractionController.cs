using Cysharp.Threading.Tasks;
using Game.Core.UI;
using Game.Core.World.InteractionSystem;
using System;
using System.Threading;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerInteractionController
    {
        public bool IsInteraction { get; private set; }

        private IInteractable _interactable;
        private CancellationTokenSource _cancellationTokenSource;
        
        private readonly UIRootGame _uiRootGame;
        private readonly CameraVisionController _cameraVisionController;
        
        public PlayerInteractionController( UIRootGame uiRootGame, CameraVisionController cameraVisionController )
        {
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            _cameraVisionController = cameraVisionController ?? throw new ArgumentNullException( nameof(cameraVisionController) );
        }
        
        public void StartInteract()
        {
            if ( _cameraVisionController.CurrentObservable == null ) return;
            if ( _cameraVisionController.CurrentObservable is not IInteractable interactable ) return;
            _interactable = interactable;
            
            IsInteraction = true;

            _cancellationTokenSource = new();
            Tick( _cancellationTokenSource.Token ).Forget();
        }

        public void StopInteract()
        {
            var gameScreenViewModel = _uiRootGame.ScreenAggregator.GetAs< GameScreenViewModel >();
            gameScreenViewModel.ModelView.TargetInformer.Button1.SetFillAmount( 0 );
            
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            
            IsInteraction = false;
        }

        private async UniTask Tick( CancellationToken cancellationToken = default )
        {
            Debug.LogError( "Start" );
            
            var gameScreenViewModel = _uiRootGame.ScreenAggregator.GetAs< GameScreenViewModel >();

            float interactionTime = 0.33f;
            float t = 0f;
            while ( t < interactionTime )
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                if ( _interactable != _cameraVisionController.CurrentObservable )
                {
                    StopInteract();
                    break;
                }

                t += Time.deltaTime;

                gameScreenViewModel.ModelView.TargetInformer.Button1.SetFillAmount( t / interactionTime );
                
                await UniTask.Yield();
            }

            Interact();
        }

        private void Interact()
        {
            _interactable.Interact();
        }
    }
}