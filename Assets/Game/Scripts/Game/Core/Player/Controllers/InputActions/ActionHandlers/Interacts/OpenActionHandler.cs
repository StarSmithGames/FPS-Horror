using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.World.InteractionSystem;
using Game.Managers.InputManager;
using PuzzlescapeGames.Localization;
using System;

namespace Game.Core.Player
{
    public sealed class OpenActionHandler : ContextMenuActionHandler
    {
        private InputActionProvider _provider;
        private UIInfoButton _ui;
        private OpenableObject _openable;
        
        private readonly InputKeyActionsSettings _inputKeyActionsSettings;
        private readonly ILocalizationSystem _localizationSystem;
        
        public OpenActionHandler(
            InputKeyActionsSettings inputKeyActionsSettings,
            ILocalizationSystem localizationSystem
            )
        {
            _inputKeyActionsSettings = inputKeyActionsSettings ?? throw new ArgumentNullException( nameof(inputKeyActionsSettings) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
            
            _provider = new( _inputKeyActionsSettings.OpenCloseAction.InputAction, Completed, 0.33f, progress: InteractProgress, callback: InteractFinished );
        }

        public override void Initialize( IObservable target )
        {
            _openable = (OpenableObject)target;
            
            if ( ContextMenuOperation == null )
            {
                ContextMenuOperation = new();
            }
            ContextMenuOperation.Name = _localizationSystem.Translate( _openable.IsOpen ? _inputKeyActionsSettings.OpenCloseAction.AdditionalNameIds[ 0 ] : _inputKeyActionsSettings.OpenCloseAction.NameId );
            ContextMenuOperation.Key = _inputKeyActionsSettings.OpenCloseAction.GetDisplayKey();
        }

        public override void Enable( UIInfoButton ui )
        {
            _ui = ui;
            _provider.Enable();
            
            IsEnable = true;
        }

        public override void Disable()
        {
            _provider.Disable();
            _ui = null;
            _openable = null;
            
            IsEnable = false;
        }
        
        private void InteractProgress( float value )
        {
            _ui.SetFillAmount( value );
        }

        private void InteractFinished( bool result )
        {
            if ( !IsEnable ) return;
            
            _ui.SetFillAmount( 0 );
        }
        
        protected override void Completed()
        {
            if ( !IsEnable ) return;
            
            if ( _openable.IsOpen )
            {
                _openable.Close();
            }
            else
            {
                _openable.Open();
            }
            
            base.Completed();
        }
    }
}