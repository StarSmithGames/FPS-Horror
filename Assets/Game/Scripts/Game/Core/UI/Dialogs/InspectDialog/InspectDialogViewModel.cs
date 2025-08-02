using DG.Tweening;
using Game.Core.World.InteractionSystem;
using Game.Core.World.Systems.InteractionSystem;
using Game.Managers.CursorManager;
using Game.Managers.InputManager;
using PuzzlescapeGames.Extensions;
using PuzzlescapeGames.VVM;
using System;
using UnityEngine;

namespace Game.Core.UI.InspectDialog
{
    public sealed class InspectDialogViewModel : ViewModel< InspectDialog >
    {
        public event Action OnCancelButtonClicked;
        
        private IInspectable _inspectable;
        private bool _isExamine;

        private readonly InputHolder _inputRead;
        private readonly InputHolder _inputCancel;
        private readonly InteractionsSettings _interactionsSettings;
        
        public InspectDialogViewModel( InteractionsSettings interactionsSettings )
        {
            _interactionsSettings = interactionsSettings ?? throw new ArgumentNullException( nameof(interactionsSettings) );
            _inputRead = new( InputManager.Inputs.UI.Read, ReadButtonClickedHandler );
            _inputCancel = new( InputManager.Inputs.UI.Cancel, CancelButtonClickedHandler );
        }
        
        public void Set( IInspectable inspectable )
        {
            _inspectable = inspectable ?? throw new ArgumentNullException( nameof(inspectable) );
        }

        protected override void SubscribeView()
        {
            base.SubscribeView();
            
            CursorManager.Enable();
            
            ModelView.ReadButton.OnButtonClicked += ReadButtonClickedHandler;
            ModelView.CancelButton1.OnButtonClicked += CancelButtonClickedHandler;
            ModelView.CancelButton2.OnButtonClicked += CancelButtonClickedHandler;
            
            _inputRead.Enable();
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

            ModelView.ReadButton.Set( _interactionsSettings.ReadAction.GetDisplayKey(), _interactionsSettings.ReadAction.NameId );
            ModelView.CancelButton1.Set( _interactionsSettings.BackAction.GetDisplayKey(), _interactionsSettings.BackAction.NameId );
            ModelView.CancelButton2.Set( _interactionsSettings.BackAction.GetDisplayKey(), _interactionsSettings.BackAction.NameId );
        }

        private void ReadButtonClickedHandler()
        {
            _isExamine = true;

            ModelView.ControlButtons.SetActive( false );
            ModelView.ExamineCanvasGroup.Enable( true, false );
            ModelView.ExamineCanvasGroup.DOFade( 1f, 0.33f );
        }
        
        private void CancelButtonClickedHandler()
        {
            if ( _isExamine )
            {
                _isExamine = false;
                
                ModelView.ControlButtons.SetActive( true );
                ModelView.ExamineCanvasGroup.Enable( false, false );
                ModelView.ExamineCanvasGroup.DOFade( 0f, 0.33f );

                return;
            }
            
            OnCancelButtonClicked?.Invoke();
            
            HideViewAndDispose();
        }
    }
}