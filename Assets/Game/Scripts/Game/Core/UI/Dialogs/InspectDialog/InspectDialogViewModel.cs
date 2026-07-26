using DG.Tweening;
using Game.Core.Entity;
using Game.Core.World.InspectionSystem;
using Game.Managers.CursorManager;
using Game.Managers.InputManager;
using PuzzlescapeGames.Extensions;
using PuzzlescapeGames.Localization;
using PuzzlescapeGames.VVM;
using StarSmithGames.Localization;
using System;
using UnityEngine;

namespace Game.Core.UI.InspectDialog
{
    public sealed class InspectDialogViewModel : ViewModel< InspectDialog >
    {
        public event Action OnActionButtonClicked;
        public event Action OnCancelButtonClicked;

        private InspectionSystem _inspectionSystem;
        private ItemObject _item;
        private bool _isExamine;
        private bool _isFromWorld;

        private readonly InputActionVoidWrap _inputAction;
        private readonly InputActionVoidWrap _inputActionCancel;
        private readonly InputKeyActionsSettings _inputKeyActionsSettings;
        private readonly ILocalizationSystem _localizationSystem;
        
        public InspectDialogViewModel(
            InputKeyActionsSettings inputKeyActionsSettings,
            ILocalizationSystem localizationSystem
            )
        {
            _inputKeyActionsSettings = inputKeyActionsSettings ?? throw new ArgumentNullException( nameof(inputKeyActionsSettings) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
            
            _inputAction = InputActionManager.CreateInputActionWrap( _inputKeyActionsSettings.InteractAction.InputAction, ActionButtonClickedHandler );
            _inputActionCancel = InputActionManager.CreateInputActionWrap( InputManager.Inputs.UI.Cancel, CancelButtonClickedHandler );
        }
        
        public void Set( ItemObject item, Camera camera, bool isFromWorld )
        {
            _item = item ?? throw new ArgumentNullException( nameof(item) );
            _inspectionSystem = new( camera );
            _isFromWorld = isFromWorld;
        }

        protected override void SubscribeView()
        {
            base.SubscribeView();
            
            ModelView.ActionButton.OnButtonClicked += ActionButtonClickedHandler;
            ModelView.CancelButton1.OnButtonClicked += CancelButtonClickedHandler;
            ModelView.CancelButton2.OnButtonClicked += CancelButtonClickedHandler;
            
            _inputActionCancel.Enable();
        }

        protected override void UnSubscribeView()
        {
            base.UnSubscribeView();
            
            ModelView.ActionButton.OnButtonClicked -= ActionButtonClickedHandler;
            ModelView.CancelButton1.OnButtonClicked -= CancelButtonClickedHandler;
            ModelView.CancelButton2.OnButtonClicked -= CancelButtonClickedHandler;
            
            _inputAction.Disable();
            _inputActionCancel.Disable();
            InputActionManager.RemoveInputActionWrap( _inputAction );
            InputActionManager.RemoveInputActionWrap( _inputActionCancel );
        }

        protected override void OnViewShowingChanged()
        {
            if ( !ModelView.IsShowing ) return;
            _isExamine = false;
            
            ModelView.ControlButtons.SetActive( true );
            ModelView.ExamineCanvasGroup.Enable( false );

            _inputAction.Enable();
            
            if ( _item is Note )
            {
                ModelView.ActionButton.Set( _inputKeyActionsSettings.InteractAction.GetDisplayKey(), _localizationSystem.Translate( LocalizationIds.UI_CONTROL_READ ) );
            }
            else
            {
                ModelView.ActionButton.gameObject.SetActive( true );
                ModelView.ActionButton.Set( _inputKeyActionsSettings.InteractAction.GetDisplayKey(), _localizationSystem.Translate( LocalizationIds.UI_CONTROL_TAKE ) );
            }
            ModelView.ActionButton.gameObject.SetActive( _isFromWorld );
            ModelView.CancelButton1.Set( _inputKeyActionsSettings.BackAction.GetDisplayKey(), _localizationSystem.Translate( LocalizationIds.UI_CONTROL_BACK ) );
            ModelView.CancelButton2.Set( _inputKeyActionsSettings.BackAction.GetDisplayKey(), _localizationSystem.Translate( LocalizationIds.UI_CONTROL_BACK ) );

            ModelView.ExamineName.text = string.Empty;
            ModelView.ExamineText.text = string.Empty;

            if ( !_item.Config.NameId.IsEmpty() )
            {
                ModelView.ExamineName.text = _localizationSystem.Translate( _item.Config.NameId );
            }
            if ( !_item.Config.DescriptionId.IsEmpty() )
            {
                ModelView.ExamineText.text = _localizationSystem.Translate( _item.Config.DescriptionId );
            }
            
            _item.SetLayer( LayersParams.ABOVE );
            _inspectionSystem.StartInspection( _item, _isFromWorld );
        }

        private void ActionButtonClickedHandler()
        {
            if ( !_isFromWorld ) return;
            if ( _item is Note )
            {
                _isExamine = true;

                ModelView.ControlButtons.SetActive( false );
                ModelView.ExamineCanvasGroup.Enable( true, false );
                ModelView.ExamineCanvasGroup.DOFade( 1f, 0.33f );
            
                _inspectionSystem.Block( true ); 
            }
            else
            {
                _inspectionSystem.StopInspection();
                _item.ResetLayer();
            
                OnActionButtonClicked?.Invoke();
                
                HideViewAndDispose();
            }
        }
        
        private void CancelButtonClickedHandler()
        {
            if ( _isExamine )
            {
                _isExamine = false;
                
                ModelView.ControlButtons.SetActive( true );
                ModelView.ExamineCanvasGroup.Enable( false, false );
                ModelView.ExamineCanvasGroup.DOFade( 0f, 0.33f );

                _inspectionSystem.Block( false );
                
                return;
            }
            
            _inspectionSystem.StopInspection();
            _item.ResetLayer();
            if ( _item is Note )
            {
                _item.EnableCollider( false );
            }
            
            OnCancelButtonClicked?.Invoke();
            
            HideViewAndDispose();
        }
    }
}