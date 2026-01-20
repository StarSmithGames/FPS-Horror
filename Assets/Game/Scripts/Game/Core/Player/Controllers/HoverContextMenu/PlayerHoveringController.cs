using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.UI.GameScreen;
using System;

namespace Game.Core.Player
{
    public sealed class PlayerHoveringController
    {
        private GameScreenViewModel _gameScreenViewModel;
        private bool _isObservablesAround;
        private PointerType _pointerType;
        private ObservableObject _currentObservable;

        private readonly UIRootGame _uiRootGame;
        private readonly CameraVisionController _cameraVisionController;
        private readonly InteractionActionController _interactionActionController;

        public PlayerHoveringController(
            UIRootGame uiRootGame,
            CameraVisionController cameraVisionController,
            InteractionActionController interactionActionController
            )
        {
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            _cameraVisionController = cameraVisionController ?? throw new ArgumentNullException( nameof(cameraVisionController) );
            _interactionActionController = interactionActionController ?? throw new ArgumentNullException( nameof(interactionActionController) );
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
            // _contextMenuActionController.Initialize( _gameScreenViewModel.ModelView.TargetInformer );
            
            _cameraVisionController.OnObservablesChanged += ObservablesChangedHandler;
            _cameraVisionController.OnCurrentObservableChanged += CurrentObservableChangedHandler;
            CurrentObservableChangedHandler( _cameraVisionController.CurrentObservable );
        }

        public void Dispose()
        {
            _cameraVisionController.OnObservablesChanged -= ObservablesChangedHandler;
            _cameraVisionController.OnCurrentObservableChanged -= CurrentObservableChangedHandler;
        }

        private void ObservablesChangedHandler( bool trigger )
        {
            _isObservablesAround = trigger;
            SetPointer( _pointerType );
        }
        
        private void CurrentObservableChangedHandler( ObservableObject observable )
        {
            _currentObservable = observable;

            Interaction();

            // ContextMenu();
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
        
        private void Interaction()
        {
            if ( _currentObservable == null )
            {
                SetPointer( PointerType.None );

                _interactionActionController.CurrentObservableChangedHandler( null );
                
                return;
            }

            if ( _currentObservable is OpenCloseObject dynamic )
            {
                SetPointer( PointerType.Hand );
                
                _interactionActionController.SetToDynamic( dynamic );
            }
            else if ( _currentObservable is ItemObject item )
            {
                SetPointer( PointerType.Point );
                
                _interactionActionController.SetToItem( item );
            }
            else if ( _currentObservable is PuzzleObject puzzle )
            {
                SetPointer( PointerType.Point );
                
                _interactionActionController.SetToPuzzle( puzzle );
            }
            
            _interactionActionController.CurrentObservableChangedHandler( _currentObservable );
        }

        // private void ContextMenu()
        // {
        //     if ( _currentObservable == null )
        //     {
        //         _pointerController.SetPointer( PointerType.None );
        //         _contextMenuActionController.CurrentObservableChangedHandler( null );
        //         return;
        //     }
        //         
        //     if ( _currentObservable is OpenCloseObject dynamic )
        //     {
        //         _pointerController.SetPointer( PointerType.Hand );
        //         _contextMenuActionController.SetToDynamic( dynamic );
        //     }
        //     else if( _currentObservable is ItemObject item )
        //     {
        //         _pointerController.SetPointer( PointerType.Point );
        //         _contextMenuActionController.SetToItem( item );
        //     }
        //     else if ( _currentObservable is PuzzleObject puzzle )
        //     {
        //         _pointerController.SetPointer( PointerType.Point );
        //         _contextMenuActionController.SetToPuzzle( puzzle );
        //     }
        //         
        //     _contextMenuActionController.CurrentObservableChangedHandler( _currentObservable );
        // }
    }
}