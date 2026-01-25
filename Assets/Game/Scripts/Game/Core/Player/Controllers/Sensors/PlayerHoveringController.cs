using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.UI.GameScreen;
using System;
using PointerType = Game.Core.UI.GameScreen.PointerType;

namespace Game.Core.Player
{
    public sealed class PlayerHoveringController
    {
        private GameScreenViewModel _gameScreenViewModel;
        private bool _isObservablesAround;
        private PointerType _pointerType;

        private readonly UIRootGame _uiRootGame;
        private readonly InteractionActionController _interactionActionController;
        private readonly PlayerInteractablesController _interactablesController;

        public PlayerHoveringController(
            UIRootGame uiRootGame,
            InteractionActionController interactionActionController,
            PlayerInteractablesController interactablesController
            )
        {
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            _interactionActionController = interactionActionController ?? throw new ArgumentNullException( nameof(interactionActionController) );
            _interactablesController = interactablesController ?? throw new ArgumentNullException( nameof(interactablesController) );
        }

        public void Initialize()
        {
            _gameScreenViewModel = _uiRootGame.ScreenAggregator.GetOrCreateIfNotExist< GameScreenViewModel >();
            _gameScreenViewModel.EnableView( true );
            _gameScreenViewModel.ModelView.TargetPoint.Disable();
            _gameScreenViewModel.ModelView.TargetHand.Disable();
            _gameScreenViewModel.ModelView.TargetHolder.Disable();
            
            _gameScreenViewModel.ModelView.TargetInformer.Enable( false );

            _interactionActionController.Initialize();

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
            if ( interactable == null )
            {
                SetPointer( PointerType.None );
            }
            
            if ( interactable is ItemObject item )
            {
                SetPointer( PointerType.Point );
                
                _interactionActionController.SetToItem( item );
            }
            else if ( interactable is OpenCloseObject dynamic )
            {
                SetPointer( PointerType.Hand );
                
                _interactionActionController.SetToDynamic( dynamic );
            }
            else if ( interactable is PuzzleObject puzzle )
            {
                SetPointer( PointerType.Point );
                
                _interactionActionController.SetToPuzzle( puzzle );
            }

            _interactionActionController.CurrentObservableChangedHandler( interactable );
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
    }
}