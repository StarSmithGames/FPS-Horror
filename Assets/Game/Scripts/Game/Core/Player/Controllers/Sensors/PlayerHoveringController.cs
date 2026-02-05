using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.UI.GameScreen;
using Game.Core.World.PointerSystem;
using System;
using System.Dynamic;
using UnityEngine;
using PointerType = Game.Core.UI.GameScreen.PointerType;

namespace Game.Core.Player
{
    public sealed class PlayerHoveringController
    {
        private GameScreenViewModel _gameScreenViewModel;
        private bool _isObservablesAround;
        private PointerType _pointerType;
        private InteractionPointer _lastPointer;
        private ActionHandler _actionHandler;
        
        private readonly UIRootGame _uiRootGame;
        private readonly PlayerInteractablesController _interactablesController;
        private readonly InteractionActionFactory _interactionActionFactory;

        public PlayerHoveringController(
            UIRootGame uiRootGame,
            PlayerInteractablesController interactablesController,
            InteractionActionFactory interactionActionFactory
            )
        {
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            _interactablesController = interactablesController ?? throw new ArgumentNullException( nameof(interactablesController) );
            _interactionActionFactory = interactionActionFactory ?? throw new ArgumentNullException( nameof(interactionActionFactory) );
        }

        public void Initialize()
        {
            _gameScreenViewModel = _uiRootGame.ScreenAggregator.GetOrCreateIfNotExist< GameScreenViewModel >();
            _gameScreenViewModel.EnableView( true );
            _gameScreenViewModel.ModelView.TargetPoint.Disable();
            _gameScreenViewModel.ModelView.TargetHand.Disable();
            _gameScreenViewModel.ModelView.TargetHolder.Disable();
            
            _gameScreenViewModel.ModelView.TargetInformer.Enable( false );

            _interactablesController.OnCurrentInteractableChanged += CurrentInteractableChangedHandler;
            _interactablesController.OnObservablesChanged += ObservablesChangedHandler;
            CurrentInteractableChangedHandler( _interactablesController.CurrentObservable );
        }

        public void Dispose()
        {
            _interactablesController.OnCurrentInteractableChanged -= CurrentInteractableChangedHandler;
            _interactablesController.OnObservablesChanged -= ObservablesChangedHandler;
        }

        private void ObservablesChangedHandler( bool trigger )
        {
            _isObservablesAround = trigger;
            SetPointer( _pointerType );
        }
        
        private void CurrentInteractableChangedHandler( InteractableObject interactable )
        {
            if ( _actionHandler != null )
            {
                _actionHandler.OnStarted -= DynamicProgressStartedHandler;
                _actionHandler.OnFinished -= DynamicProgressFinishedHandler;
                _actionHandler.OnProgressChanged -= DynamicProgressChangedHandler;
            }
            
            _lastPointer?.SetHolderBar( 0 );
            _lastPointer = _interactablesController.GetPointer( interactable );
            _lastPointer?.SetHolderBar( 0 );
            
            if ( interactable == null )
            {
                SetPointer( PointerType.None );
            }
            
            if ( interactable is ItemObject item )
            {
                SetPointer( PointerType.Point );
                
                SetToItem( item );
            }
            else if ( interactable is OpenCloseObject dynamic )
            {
                SetPointer( PointerType.Hand );
                
                SetToDynamic( dynamic );
            }
            else if ( interactable is PuzzleObject puzzle )
            {
                SetPointer( PointerType.Point );
                
                SetToPuzzle( puzzle );
            }

            if ( interactable == null )
            {
                if ( _actionHandler != null )
                {
                    _actionHandler.OnStarted -= DynamicProgressStartedHandler;
                    _actionHandler.OnFinished -= DynamicProgressFinishedHandler;
                    _actionHandler.OnProgressChanged -= DynamicProgressChangedHandler;
                    _actionHandler.Dispose();
                }
                _actionHandler = null;
                _lastPointer = null;
                return;
            }
            _actionHandler?.Enable();
        }
        
        private void SetPointer( PointerType type )
        {
            _pointerType = type;
            
            if ( _pointerType == PointerType.None )
            {
                _gameScreenViewModel.ModelView.TargetPoint.EnableTargetPoint( _isObservablesAround );
                _gameScreenViewModel.ModelView.TargetHand.EnableTargetHand( false );
            }
            else if ( _pointerType == PointerType.Hand )
            {

            }
            else if ( _pointerType == PointerType.Point || _isObservablesAround )
            {
                _gameScreenViewModel.ModelView.TargetPoint.EnableTargetPoint( true );
                _gameScreenViewModel.ModelView.TargetHand.EnableTargetHand( false );
            }
        }
        
        private void SetToDynamic( OpenCloseObject dynamic )
        {
            _actionHandler?.Dispose();
            _actionHandler = _interactionActionFactory.GetOrCreateOpenCloseHandler( dynamic );
            
            _actionHandler.OnStarted += DynamicProgressStartedHandler;
            _actionHandler.OnFinished += DynamicProgressFinishedHandler;
            _actionHandler.OnProgressChanged += DynamicProgressChangedHandler;
        }

        private void SetToItem( ItemObject item )
        {
            _actionHandler?.Dispose();
            _actionHandler = _interactionActionFactory.GetOrCreateItemHandler( item );
        }

        private void SetToPuzzle( PuzzleObject puzzle )
        {
            _actionHandler?.Dispose();
            _actionHandler = _interactionActionFactory.GetOrCreatePuzzleHandler( puzzle );
        }
        
        private void DynamicProgressStartedHandler()
        {
            _lastPointer.SetHolderBar( 0 );
        }
        
        private void DynamicProgressFinishedHandler()
        {
            _lastPointer.SetHolderBar( 0 );
            _lastPointer = null;
        }
        
        private void DynamicProgressChangedHandler( float progress )
        {
            _lastPointer.SetHolderBar( progress );
        }
    }
}