using Game.Core.Entity;
using Game.Core.Player.InputActionProcesses;
using Game.Core.UI;
using Game.Core.UI.GameScreen;
using Game.Core.UI.InspectDialog;
using Game.Core.World.InspectionSystem;
using Game.Core.World.InteractionSystem;
using Game.Core.World.Systems.InteractionSystem;
using PuzzlescapeGames.Localization;
using System;

namespace Game.Core.Player
{
    public sealed class PlayerTargetingController
    {
        private GameScreenViewModel _gameScreenViewModel;
        private UITargetInformer _targetInformer;
        private bool _isShowingInformer;
        
        private IObservable _currentObservable;
        private InteractionProcess _interactionProcess;
        private InspectionProcess _inspectionProcess;

        private InspectDialogViewModel _inspectDialogViewModel;
        
        private readonly InteractionsSettings _interactionsSettings;
        private readonly UIRootGame _uiRootGame;
        private readonly PlayerObject _view;
        private readonly PlayerStates _states;
        private readonly CameraVisionController _cameraVisionController;
        private readonly ILocalizationSystem _localizationSystem;
        
        public PlayerTargetingController(
            InteractionsSettings interactionsSettings,
            UIRootGame uiRootGame,
            PlayerObject view,
            PlayerStates states,
            CameraVisionController cameraVisionController,
            ILocalizationSystem localizationSystem
            )
        {
            _interactionsSettings = interactionsSettings ?? throw new ArgumentNullException( nameof(interactionsSettings) );
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _states = states ?? throw new ArgumentNullException( nameof(states) );
            _cameraVisionController = cameraVisionController ?? throw new ArgumentNullException( nameof(cameraVisionController) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }

        public void Initialize()
        {
            _interactionProcess = new( () => _currentObservable != _cameraVisionController.CurrentObservable );
            _inspectionProcess = new();
            _inspectionProcess.OnCompleted += InspectionCompletedHandler;

            _gameScreenViewModel = _uiRootGame.ScreenAggregator.GetAs< GameScreenViewModel >();
            _gameScreenViewModel.ModelView.TargetPoint.Disable();
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
            
            _inspectionProcess.OnCompleted -= InspectionCompletedHandler;
        }

        private void ObservablesChangedHandler( bool trigger )
        {
            _gameScreenViewModel.ModelView.TargetPoint.EnableTargetPoint( trigger );
        }
        
        private void CurrentObservableChangedHandler( IObservable observable )
        {
            if ( _currentObservable == observable ) return;
            _currentObservable = observable;

            _targetInformer.Button1.SetFillAmount( 0f );
            _targetInformer.Button1.gameObject.SetActive( false );
            _targetInformer.Button2.SetFillAmount( 0f );
            _targetInformer.Button2.gameObject.SetActive( false );
            
            _interactionProcess.Disable();
            _inspectionProcess.Disable();

            if ( _currentObservable == null )
            {
                if ( _isShowingInformer )
                {
                    _isShowingInformer = false;
                    _targetInformer.Hide();
                }
            }
            else
            {
                if( _currentObservable is PickableInspectableEntityObject pickableInspectable )
                {
                    _targetInformer.Name.text = "Pickable Inspectable";
                    _targetInformer.Button1.Set( _interactionsSettings.InteractAction.GetDisplayKey(), _localizationSystem.Translate( _interactionsSettings.InteractAction.NameId ) );
                    _targetInformer.Button1.gameObject.SetActive( true );
                    _targetInformer.Button2.Set( _interactionsSettings.InspectAction.GetDisplayKey(), _localizationSystem.Translate( _interactionsSettings.InspectAction.NameId ) );
                    _targetInformer.Button2.gameObject.SetActive( true );

                    _interactionProcess.Enable( _targetInformer.Button1, pickableInspectable );
                    _inspectionProcess.Enable( _targetInformer.Button2, pickableInspectable );
                }
                else if ( _currentObservable is IPickable pickable )
                {
                    _targetInformer.Name.text = "Pickable";
                    _targetInformer.Button1.Set( _interactionsSettings.InteractAction.GetDisplayKey(), _localizationSystem.Translate( _interactionsSettings.InteractAction.NameId ) );
                    _targetInformer.Button1.gameObject.SetActive( true );

                    _interactionProcess.Enable( _targetInformer.Button1, pickable );
                }
                else if( _currentObservable is IInspectable inspectable )
                {
                    _targetInformer.Name.text = "Inspectable";
                    _targetInformer.Button1.Set( _interactionsSettings.InspectAction.GetDisplayKey(), _localizationSystem.Translate( _interactionsSettings.InspectAction.NameId ) );
                    _targetInformer.Button1.gameObject.SetActive( true );
                    
                    _inspectionProcess.Enable( _targetInformer.Button1, inspectable );
                }
                
                if ( !_isShowingInformer )
                {
                    _isShowingInformer = true;
                    _targetInformer.Show();
                }
            }
        }

        private void InspectionCompletedHandler( IInspectable inspectable )
        {
            _states.IsBlocked = true;

            _inspectDialogViewModel = _uiRootGame.DialogAggregator.GetOrCreateIfNotExist< InspectDialogViewModel >();
            _inspectDialogViewModel.Set( inspectable, _view.CameraFPS );
            _inspectDialogViewModel.OnCancelButtonClicked += InspectCompletedHandler;
            _inspectDialogViewModel.ShowView();
        }

        private void InspectCompletedHandler()
        {
            _inspectDialogViewModel.OnCancelButtonClicked -= InspectCompletedHandler;
            _inspectDialogViewModel = null;
            
            _states.IsBlocked = false;
        }
    }
}