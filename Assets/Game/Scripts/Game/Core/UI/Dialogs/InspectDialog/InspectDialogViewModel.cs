using DG.Tweening;
using Game.Core.Entity;
using Game.Core.World.InspectionSystem;
using Game.Managers.CursorManager;
using Game.Managers.InputManager;
using PuzzlescapeGames.Extensions;
using PuzzlescapeGames.Localization;
using PuzzlescapeGames.VVM;
using System;
using UnityEngine;

namespace Game.Core.UI.InspectDialog
{
    public sealed class InspectDialogViewModel : ViewModel< InspectDialog >
    {
        public event Action OnCancelButtonClicked;

        private InspectionSystem _inspectionSystem;
        private ItemObject _item;
        private bool _isExamine;

        private readonly InputHolder _inputRead;
        private readonly InputHolder _inputCancel;
        private readonly InputKeyActionsSettings _inputKeyActionsSettings;
        private readonly ILocalizationSystem _localizationSystem;
        
        public InspectDialogViewModel(
            InputKeyActionsSettings inputKeyActionsSettings,
            ILocalizationSystem localizationSystem
            )
        {
            _inputKeyActionsSettings = inputKeyActionsSettings ?? throw new ArgumentNullException( nameof(inputKeyActionsSettings) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
            _inputRead = new( InputManager.Inputs.UI.Read, ReadButtonClickedHandler );
            _inputCancel = new( InputManager.Inputs.UI.Cancel, CancelButtonClickedHandler );
        }
        
        public void Set( ItemObject item, Camera camera )
        {
            _item = item ?? throw new ArgumentNullException( nameof(item) );
            _inspectionSystem = new( camera );
        }

        protected override void SubscribeView()
        {
            base.SubscribeView();
            
            CursorManager.Enable();
            
            ModelView.ReadButton.OnButtonClicked += ReadButtonClickedHandler;
            ModelView.CancelButton1.OnButtonClicked += CancelButtonClickedHandler;
            ModelView.CancelButton2.OnButtonClicked += CancelButtonClickedHandler;
            
            _inputCancel.Enable();
        }

        protected override void UnSubscribeView()
        {
            base.UnSubscribeView();
            
            ModelView.ReadButton.OnButtonClicked -= ReadButtonClickedHandler;
            ModelView.CancelButton1.OnButtonClicked -= CancelButtonClickedHandler;
            ModelView.CancelButton2.OnButtonClicked -= CancelButtonClickedHandler;
            
            _inputRead.Disable();
            _inputCancel.Disable();
            
            CursorManager.Disable();
        }

        protected override void OnViewShowingChanged()
        {
            if ( !ModelView.IsShowing ) return;
            
            _isExamine = false;
            
            ModelView.ControlButtons.SetActive( true );
            ModelView.ExamineCanvasGroup.Enable( false );

            ModelView.ReadButton.gameObject.SetActive( false );
            ModelView.ReadButton.Set( _inputKeyActionsSettings.ReadAction.GetDisplayKey(), _localizationSystem.Translate( _inputKeyActionsSettings.ReadAction.NameId ) );
            ModelView.CancelButton1.Set( _inputKeyActionsSettings.BackAction.GetDisplayKey(), _localizationSystem.Translate( _inputKeyActionsSettings.BackAction.NameId ) );
            ModelView.CancelButton2.Set( _inputKeyActionsSettings.BackAction.GetDisplayKey(), _localizationSystem.Translate( _inputKeyActionsSettings.BackAction.NameId ) );

            ModelView.ExamineName.text = string.Empty;
            ModelView.ExamineText.text = string.Empty;

            if ( !_item.NameId.IsEmpty() )
            {
                ModelView.ExamineName.text = _localizationSystem.Translate( _item.NameId );
            }
            if ( !_item.TextId.IsEmpty() )
            {
                ModelView.ExamineText.text = _localizationSystem.Translate( _item.TextId );
                
                _inputRead.Enable();
                ModelView.ReadButton.gameObject.SetActive( true );
            }
            
            _inspectionSystem.StartInspection( _item );
        }

        private void ReadButtonClickedHandler()
        {
            _isExamine = true;

            ModelView.ControlButtons.SetActive( false );
            ModelView.ExamineCanvasGroup.Enable( true, false );
            ModelView.ExamineCanvasGroup.DOFade( 1f, 0.33f );
            
            _inspectionSystem.Block( true );
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
            
            OnCancelButtonClicked?.Invoke();
            
            HideViewAndDispose();
        }
    }
}