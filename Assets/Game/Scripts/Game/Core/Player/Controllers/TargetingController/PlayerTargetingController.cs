using Game.Core.Entity;
using Game.Core.Entity.Item;
using Game.Core.UI;
using Game.Core.UI.GameScreen;
using PuzzlescapeGames.Localization;
using PuzzlescapeGames.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
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
        private CompositeHandler _compositeHandler;

        private readonly InteractionActionFactory _interactionActionFactory;
        private readonly UIRootGame _uiRootGame;
        private readonly CameraVisionController _cameraVisionController;
        private readonly ILocalizationSystem _localizationSystem;
        
        public PlayerTargetingController(
            InteractionActionFactory interactionActionFactory,
            UIRootGame uiRootGame,
            CameraVisionController cameraVisionController,
            ILocalizationSystem localizationSystem
            )
        {
            _interactionActionFactory = interactionActionFactory ?? throw new ArgumentNullException( nameof(interactionActionFactory) );
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

            for ( int i = 0; i < _targetInformer.Options.Count; i++ )
            {
                _targetInformer.Options[ i ].gameObject.SetActive( false );
                _targetInformer.Options[ i ].SetFillAmount( 0f );
            }

            if ( _compositeHandler != null )
            {
                _compositeHandler.Disable();
                _compositeHandler.OnCompleted -= ActionCompletedHandler;
            }
            
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

            _compositeHandler = TryGetHandler();
            if ( _compositeHandler != null )
            {
                _compositeHandler.Initialize( _currentObservable );
                List< ContextMenuOperation > options = _compositeHandler.GetContextMenuOptions();
                if ( options != null )
                {
                    List< UIInfoButton > ui = new( options.Count );
                    for ( int i = 0; i < options.Count; i++ )
                    {
                        var option = options[ i ];
                        var view = _targetInformer.Options[ i ];
                       
                        view.gameObject.SetActive( true );
                        view.Set( option.Key, option.Name );
                        
                        ui.Add( view );
                    }
                    
                    _compositeHandler.Enable( ui );
                    _compositeHandler.OnCompleted += ActionCompletedHandler;
                }
            }
            
            if ( !_isShowingInformer )
            {
                _isShowingInformer = true;
                _targetInformer.Show();
            }
        }

        private CompositeHandler TryGetHandler()
        {
            if ( _currentObservable is DynamicObject dynamic )
            {
                _pointerController.SetPointer( PointerType.Hand );
                _targetInformer.Name.text = dynamic.NameId.IsEmpty() ? string.Empty : _localizationSystem.Translate( dynamic.NameId );
                if ( _currentObservable is OpenableObject )
                {
                    return _interactionActionFactory.GetOrCreateOpenableHandler();
                }
                if ( _currentObservable is PullableObject )
                {
                    return _interactionActionFactory.GetOrCreatePullableHandler();
                }
            }
            else if( _currentObservable is ItemObject item )
            {
                _pointerController.SetPointer( PointerType.Point );
                _targetInformer.Name.text = item.NameId.IsEmpty() ? string.Empty : _localizationSystem.Translate( item.NameId );

                if ( item is Note )
                {
                    return _interactionActionFactory.GetOrCreateItemNoteHandler();
                }
                
                return _interactionActionFactory.GetOrCreateItemHandler();
            }

            return null;
        }

        private void ActionCompletedHandler()
        {
            CurrentObservableChangedHandler( _currentObservable );
        }
    }
}