using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.UI.GameScreen;
using System;
using UnityEngine;
using PointerType = Game.Core.UI.GameScreen.PointerType;

namespace Game.Core.Player
{
    public sealed class PlayerHoveringController
    {
        private GameScreenViewModel _gameScreenViewModel;
        private bool _isObservablesAround;
        private PointerType _pointerType;
        private ObservableObject _visionObservable;

        private readonly UIRootGame _uiRootGame;
        private readonly InteractionActionController _interactionActionController;

        public PlayerHoveringController(
            UIRootGame uiRootGame,
            InteractionActionController interactionActionController
            )
        {
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
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
            
            // _playerVisionController.OnObservablesChanged += ObservablesChangedHandler;
            // _playerVisionController.OnCurrentObservableChanged += CurrentObservableChangedVisionHandler;
            // _playerAroundController.OnCurrentObservableChanged += CurrentObservableChangedAroundHandler;
            // CurrentObservableChangedVisionHandler( _playerVisionController.CurrentObservable );
        }

        public void Dispose()
        {
            // _playerVisionController.OnObservablesChanged -= ObservablesChangedHandler;
            // _playerVisionController.OnCurrentObservableChanged -= CurrentObservableChangedVisionHandler;
            // _playerAroundController.OnCurrentObservableChanged -= CurrentObservableChangedAroundHandler;
        }

        private void ObservablesChangedHandler( bool trigger )
        {
            // _isObservablesAround = trigger;
            // SetPointer( _pointerType );
        }
        
        private void CurrentObservableChangedVisionHandler( ObservableObject observable )
        {
            InteractionVision( observable );
            
            // _playerPointsController.PointsAround( allTargets, _view.transform, _view.CameraFPS.transform );
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
            _visionObservable = observable;
            
            if ( _visionObservable == null )
            {
                SetPointer( PointerType.None );
                
                return;
            }

            if ( _visionObservable is OpenCloseObject dynamic )
            {
                SetPointer( PointerType.Hand );
                
                _interactionActionController.SetToDynamic( dynamic );
            }
        }
        
        private void InteractionAround( ObservableObject observable )
        {
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

            if ( _visionObservable != null )
            {
                _interactionActionController.CurrentObservableChangedHandler( _visionObservable );
            }
            else
            {
                _interactionActionController.CurrentObservableChangedHandler( observable );
            }
        }
    }
}