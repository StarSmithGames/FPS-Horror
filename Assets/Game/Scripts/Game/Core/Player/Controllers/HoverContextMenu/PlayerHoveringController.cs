using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.UI.GameScreen;
using Game.Core.World.InteractionSystem;
using System;

namespace Game.Core.Player
{
    public sealed class PlayerHoveringController
    {
        private GameScreenViewModel _gameScreenViewModel;
        private PointerController _pointerController;
        private PointerType _pointerType;
        private IObservable _currentObservable;

        private readonly UIRootGame _uiRootGame;
        private readonly CameraVisionController _cameraVisionController;
        private readonly ContextMenuActionController _contextMenuActionController;

        public PlayerHoveringController(
            UIRootGame uiRootGame,
            CameraVisionController cameraVisionController,
            ContextMenuActionController contextMenuActionController
            )
        {
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            _cameraVisionController = cameraVisionController ?? throw new ArgumentNullException( nameof(cameraVisionController) );
            _contextMenuActionController = contextMenuActionController ?? throw new ArgumentNullException( nameof(contextMenuActionController) );
        }

        public void Initialize()
        {
            _gameScreenViewModel = _uiRootGame.ScreenAggregator.GetOrCreateIfNotExist< GameScreenViewModel >();
            _gameScreenViewModel.EnableView( true );
            _pointerController = new( _gameScreenViewModel );
            _gameScreenViewModel.ModelView.TargetInformer.Enable( false );
            
            _contextMenuActionController.Initialize( _gameScreenViewModel.ModelView.TargetInformer );
            
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
            _pointerController.SetObservablesAround( trigger );
        }
        
        private void CurrentObservableChangedHandler( IObservable observable )
        {
            _currentObservable = observable;

            // ContextMenu();
        }

        private void ContextMenu()
        {
            if ( _currentObservable == null )
            {
                _pointerController.SetPointer( PointerType.None );
                _contextMenuActionController.CurrentObservableChangedHandler( null );
                return;
            }
                
            if ( _currentObservable is OpenCloseObject dynamic )
            {
                _pointerController.SetPointer( PointerType.Hand );
                _contextMenuActionController.SetToDynamic( dynamic );
            }
            else if( _currentObservable is ItemObject item )
            {
                _pointerController.SetPointer( PointerType.Point );
                _contextMenuActionController.SetToItem( item );
            }
            else if ( _currentObservable is PuzzleObject puzzle )
            {
                _pointerController.SetPointer( PointerType.Point );
                _contextMenuActionController.SetToPuzzle( puzzle );
            }
                
            _contextMenuActionController.CurrentObservableChangedHandler( _currentObservable );
        }
    }
}