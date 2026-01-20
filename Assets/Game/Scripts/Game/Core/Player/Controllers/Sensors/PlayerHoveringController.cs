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

        private readonly UIRootGame _uiRootGame;
        private readonly PlayerVisionController _playerVisionController;
        private readonly PlayerAroundController _playerAroundController;
        private readonly InteractionActionController _interactionActionController;

        public PlayerHoveringController(
            UIRootGame uiRootGame,
            PlayerVisionController playerVisionController,
            PlayerAroundController playerAroundController,
            InteractionActionController interactionActionController
            )
        {
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            _playerVisionController = playerVisionController ?? throw new ArgumentNullException( nameof(playerVisionController) );
            _playerAroundController = playerAroundController ?? throw new ArgumentNullException( nameof(playerAroundController) );
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
            
            _playerVisionController.OnObservablesChanged += ObservablesChangedHandler;
            _playerVisionController.OnCurrentObservableChanged += CurrentObservableChangedVisionHandler;
            _playerAroundController.OnCurrentObservableChanged += CurrentObservableChangedAroundHandler;
            CurrentObservableChangedVisionHandler( _playerVisionController.CurrentObservable );
        }

        public void Dispose()
        {
            _playerVisionController.OnObservablesChanged -= ObservablesChangedHandler;
            _playerVisionController.OnCurrentObservableChanged -= CurrentObservableChangedVisionHandler;
            _playerAroundController.OnCurrentObservableChanged -= CurrentObservableChangedAroundHandler;
        }

        private void ObservablesChangedHandler( bool trigger )
        {
            // _isObservablesAround = trigger;
            // SetPointer( _pointerType );
        }
        
        private void CurrentObservableChangedVisionHandler( ObservableObject observable )
        {
            // InteractionVision( observable );

            // ContextMenu( observable );
        }
        
        private void CurrentObservableChangedAroundHandler( ObservableObject observable )
        {
            InteractionAround( observable );
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
        
        private void InteractionVision( ObservableObject observable )
        {
            if ( observable == null )
            {
                SetPointer( PointerType.None );

                _interactionActionController.CurrentObservableChangedHandler( null );
                
                return;
            }

            if ( observable is OpenCloseObject dynamic )
            {
                SetPointer( PointerType.Hand );
                
                _interactionActionController.SetToDynamic( dynamic );
            }
            else if ( observable is ItemObject item )
            {
                SetPointer( PointerType.Point );
                
                _interactionActionController.SetToItem( item );
            }
            else if ( observable is PuzzleObject puzzle )
            {
                SetPointer( PointerType.Point );
                
                _interactionActionController.SetToPuzzle( puzzle );
            }
            
            _interactionActionController.CurrentObservableChangedHandler( observable );
        }
        
        private void InteractionAround( ObservableObject observable )
        {
            if ( observable == null )
            {
                SetPointer( PointerType.None );

                _interactionActionController.CurrentObservableChangedHandler( null );
                
                return;
            }

            if ( observable is ItemObject item )
            {
                SetPointer( PointerType.Point );
                
                _interactionActionController.SetToItem( item );
            }
            else if ( observable is PuzzleObject puzzle )
            {
                SetPointer( PointerType.Point );
                
                _interactionActionController.SetToPuzzle( puzzle );
            }
            
            _interactionActionController.CurrentObservableChangedHandler( observable );
        }

        // private void ContextMenu( ObservableObject observable )
        // {
        //     if ( observable == null )
        //     {
        //         _pointerController.SetPointer( PointerType.None );
        //         _contextMenuActionController.CurrentObservableChangedHandler( null );
        //         return;
        //     }
        //         
        //     if ( observable is OpenCloseObject dynamic )
        //     {
        //         _pointerController.SetPointer( PointerType.Hand );
        //         _contextMenuActionController.SetToDynamic( dynamic );
        //     }
        //     else if( observable is ItemObject item )
        //     {
        //         _pointerController.SetPointer( PointerType.Point );
        //         _contextMenuActionController.SetToItem( item );
        //     }
        //     else if ( observable is PuzzleObject puzzle )
        //     {
        //         _pointerController.SetPointer( PointerType.Point );
        //         _contextMenuActionController.SetToPuzzle( puzzle );
        //     }
        //         
        //     _contextMenuActionController.CurrentObservableChangedHandler( observable );
        // }
    }
}