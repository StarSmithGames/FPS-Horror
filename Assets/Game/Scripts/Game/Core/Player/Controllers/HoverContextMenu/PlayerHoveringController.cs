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
    public sealed class PlayerHoveringController
    {
        private GameScreenViewModel _gameScreenViewModel;
        private PointerController _pointerController;
        private PointerType _pointerType;
        private UITargetInformer _targetInformer;
        private bool _isShowingInformer;
        
        private IObservable _currentObservable;
        private ActionHandlerComposite _actionHandlerComposite;

        private readonly ContextMenuActionFactory _contextMenuActionFactory;
        private readonly UIRootGame _uiRootGame;
        private readonly CameraVisionController _cameraVisionController;
        private readonly ILocalizationSystem _localizationSystem;
        
        public PlayerHoveringController(
            ContextMenuActionFactory contextMenuActionFactory,
            UIRootGame uiRootGame,
            CameraVisionController cameraVisionController,
            ILocalizationSystem localizationSystem
            )
        {
            _contextMenuActionFactory = contextMenuActionFactory ?? throw new ArgumentNullException( nameof(contextMenuActionFactory) );
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

            ResetOptions();

            if ( _actionHandlerComposite != null )
            {
                _actionHandlerComposite.Disable();
                _actionHandlerComposite.OnCompleted -= ActionCompleted;
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

            _actionHandlerComposite = TryGetHandler();
            if ( _actionHandlerComposite != null )
            {
                _actionHandlerComposite.Initialize( _currentObservable );
                List< ContextMenuOperation > options = _actionHandlerComposite.GetContextMenuOptions();
                if ( options != null )
                {
                    _actionHandlerComposite.Enable( GetOptions( options ) );
                    _actionHandlerComposite.OnCompleted += ActionCompleted;
                }
            }
            
            if ( !_isShowingInformer )
            {
                _isShowingInformer = true;
                _targetInformer.Show();
            }

            List< UIInfoButton > GetOptions( List< ContextMenuOperation > options )
            {
                List< UIInfoButton > result = new( options.Count );
                for ( int i = 0; i < options.Count; i++ )
                {
                    var option = options[ i ];
                    var view = _targetInformer.Options[ i ];
                       
                    view.gameObject.SetActive( true );
                    view.Set( option.Key, option.Name );
                        
                    result.Add( view );
                }

                return result;
            }
            
            void ResetOptions()
            {
                for ( int i = 0; i < _targetInformer.Options.Count; i++ )
                {
                    _targetInformer.Options[ i ].gameObject.SetActive( false );
                    _targetInformer.Options[ i ].SetFillAmount( 0f );
                }
            }
        }

        private ActionHandlerComposite TryGetHandler()
        {
            if ( _currentObservable is DynamicObject dynamic )
            {
                _pointerController.SetPointer( PointerType.Hand );
                _targetInformer.Name.text = dynamic.NameId.IsEmpty() ? string.Empty : _localizationSystem.Translate( dynamic.NameId );
                if ( _currentObservable is OpenCloseDynamicObject )
                {
                    return _contextMenuActionFactory.GetOrCreateOpenCloseHandler();
                }
            }
            else if( _currentObservable is ItemObject item )
            {
                _pointerController.SetPointer( PointerType.Point );
                _targetInformer.Name.text = item.NameId.IsEmpty() ? string.Empty : _localizationSystem.Translate( item.NameId );

                if ( item is Note )
                {
                    return _contextMenuActionFactory.GetOrCreateItemNoteHandler();
                }
                
                return _contextMenuActionFactory.GetOrCreateItemHandler();
            }
            // else if ( _currentObservable is PuzzleObject puzzle )
            // {
            //     _pointerController.SetPointer( PointerType.Point );
            //     _targetInformer.Name.text = string.Empty;
            //     
            //     return _contextMenuActionFactory.GetOrCreateOpenCloseHandler();
            // }

            return null;
        }

        private void ActionCompleted()
        {
            CurrentObservableChangedHandler( _currentObservable );
        }
    }
}