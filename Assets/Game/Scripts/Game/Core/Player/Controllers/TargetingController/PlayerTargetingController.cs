using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.UI.GameScreen;
using PuzzlescapeGames.Localization;
using PuzzlescapeGames.Extensions;
using System;
using System.Collections.Generic;
using IObservable = Game.Core.World.InteractionSystem.IObservable;
using PointerType = Game.Core.UI.GameScreen.PointerType;

namespace Game.Core.Player
{
    public sealed class PlayerTargetingController
    {
        private GameScreenViewModel _gameScreenViewModel;
        private PointerController _pointerController;
        private PointerType _pointerType;
        private UITargetInformer _targetInformer;
        private bool _isShowingInformer;
        
        private IObservable _currentObservable;

        private readonly PickableHandler _pickableHandler;
        private readonly InspectableHandler _inspectableHandler;
        private readonly OpenableHandler _openableHandler;
        private readonly PullableHandler _pullableHandler;
        private readonly UIRootGame _uiRootGame;
        private readonly CameraVisionController _cameraVisionController;
        private readonly ILocalizationSystem _localizationSystem;
        
        public PlayerTargetingController(
            PickableHandler pickableHandler,
            InspectableHandler inspectableHandler,
            OpenableHandler openableHandler,
            PullableHandler pullableHandler,
            UIRootGame uiRootGame,
            CameraVisionController cameraVisionController,
            ILocalizationSystem localizationSystem
            )
        {
            _pickableHandler = pickableHandler ?? throw new ArgumentNullException( nameof(pickableHandler) );
            _inspectableHandler = inspectableHandler ?? throw new ArgumentNullException( nameof(inspectableHandler) );
            _openableHandler = openableHandler ?? throw new ArgumentNullException( nameof(openableHandler) );
            _pullableHandler = pullableHandler ?? throw new ArgumentNullException( nameof(pullableHandler) );
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            _cameraVisionController = cameraVisionController ?? throw new ArgumentNullException( nameof(cameraVisionController) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }

        public void Initialize()
        {
            _gameScreenViewModel = _uiRootGame.ScreenAggregator.GetAs< GameScreenViewModel >();
            _pointerController = new( _gameScreenViewModel );
            _targetInformer = _gameScreenViewModel.ModelView.TargetInformer;
            _targetInformer.Enable( false );

            _pickableHandler.Initialize( IsBreak );
            _inspectableHandler.Initialize();
            _openableHandler.Initialize( IsBreak );
            _pullableHandler.Initialize( IsBreak );

            _pickableHandler.OnCompleted += HandlerCompletedHandler;
            _inspectableHandler.OnCompleted += HandlerCompletedHandler;
            _openableHandler.OnCompleted += HandlerCompletedHandler;
            _pullableHandler.OnCompleted += HandlerCompletedHandler;
            
            _cameraVisionController.OnObservablesChanged += ObservablesChangedHandler;
            _cameraVisionController.OnCurrentObservableChanged += CurrentObservableChangedHandler;
            CurrentObservableChangedHandler( _cameraVisionController.CurrentObservable );
        }

        public void Dispose()
        {
            _cameraVisionController.OnObservablesChanged -= ObservablesChangedHandler;
            _cameraVisionController.OnCurrentObservableChanged -= CurrentObservableChangedHandler;
            
            _pickableHandler.OnCompleted -= HandlerCompletedHandler;
            _inspectableHandler.OnCompleted -= HandlerCompletedHandler;
            _openableHandler.OnCompleted -= HandlerCompletedHandler;
            _pullableHandler.OnCompleted -= HandlerCompletedHandler;
        }

        private void ObservablesChangedHandler( bool trigger )
        {
            _pointerController.SetObservablesAround( trigger );
        }
        
        private void CurrentObservableChangedHandler( IObservable observable )
        {
            _currentObservable = observable;

            for ( int i = 0; i < _targetInformer.Options.Count; i++ )
            {
                _targetInformer.Options[ i ].gameObject.SetActive( false );
                _targetInformer.Options[ i ].SetFillAmount( 0f );
            }
            
            _pickableHandler.Disable();
            _inspectableHandler.Disable();
            _openableHandler.Disable();
            _pullableHandler.Disable();
            
            if ( _currentObservable == null )
            {
                if ( _isShowingInformer )
                {
                    _isShowingInformer = false;
                    _targetInformer.Hide();
                }

                _pointerController.SetPointer( PointerType.None );

                return;
            }

            var handler = TryGetHandler();
            if ( handler != null )
            {
                List< ContextMenuOperation > options = handler.GetContextMenuOptions();
                if ( options != null )
                {
                    for ( int i = 0; i < options.Count; i++ )
                    {
                        var option = options[ i ];
                        var view = _targetInformer.Options[ i ];
                        view.gameObject.SetActive( true );
                        view.Set( option.Key, option.Name );
                    }
                }
            }
            
            if ( !_isShowingInformer )
            {
                _isShowingInformer = true;
                _targetInformer.Show();
            }
        }

        private InteractableHandler TryGetHandler()
        {
            InteractableHandler handler = null;
            
            if ( _currentObservable is DynamicObject dynamic )
            {
                _targetInformer.Name.text = dynamic.NameId.IsEmpty() ? string.Empty : _localizationSystem.Translate( dynamic.NameId );

                if ( _currentObservable is OpenableObject openable )
                {
                    handler = _openableHandler;
                    _openableHandler.Enable( _targetInformer.Options[ 0 ], openable );
                }
                else if ( _currentObservable is PullableObject pullable )
                {
                    handler = _pullableHandler;
                    _pullableHandler.Enable( _targetInformer.Options[ 0 ], pullable );
                }
                
                _pointerController.SetPointer( PointerType.Hand );
            }
            else if( _currentObservable is ItemObject item )
            {
                _targetInformer.Name.text = item.NameId.IsEmpty() ? string.Empty : _localizationSystem.Translate( item.NameId );
                
                if ( _currentObservable is PickableItemObject )
                {
                    handler = _pickableHandler;
                    _pickableHandler.Enable( _targetInformer.Options[ 0 ], item );
                }
                else
                {
                    handler = _inspectableHandler;
                    _inspectableHandler.Enable( item );
                }
                
                _pointerController.SetPointer( PointerType.Point );
            }

            return handler;
        }

        private bool IsBreak() => _currentObservable != _cameraVisionController.CurrentObservable;

        private void HandlerCompletedHandler( InteractableHandler handler )
        {
            CurrentObservableChangedHandler( _currentObservable );
        }
    }
}