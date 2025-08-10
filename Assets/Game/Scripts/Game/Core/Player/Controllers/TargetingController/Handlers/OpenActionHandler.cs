using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.World.InteractionSystem;
using Game.Core.World.Systems.InteractionSystem;
using Game.Managers.InputManager;
using PuzzlescapeGames.Localization;
using System;

namespace Game.Core.Player
{
    public sealed class OpenActionHandler : ActionHandler
    {
        private InputActionProvider _provider;
        private UIInfoButton _ui;
        private OpenableObject _openable;
        
        private readonly InteractionsSettings _interactionsSettings;
        private readonly ILocalizationSystem _localizationSystem;
        
        public OpenActionHandler(
            InteractionsSettings interactionsSettings,
            ILocalizationSystem localizationSystem
            )
        {
            _interactionsSettings = interactionsSettings ?? throw new ArgumentNullException( nameof(interactionsSettings) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
            
            _provider = new( _interactionsSettings.OpenCloseAction.InputAction, Completed, 0.33f, progress: InteractProgress, callback: InteractFinished );
        }

        public override void Initialize( IObservable target )
        {
            _openable = (OpenableObject)target;
            
            if ( ContextMenuOperation == null )
            {
                ContextMenuOperation = new();
            }
            ContextMenuOperation.Name = _localizationSystem.Translate( _openable.IsOpen ? _interactionsSettings.OpenCloseAction.AdditionalNameIds[ 0 ] : _interactionsSettings.OpenCloseAction.NameId );
            ContextMenuOperation.Key = _interactionsSettings.OpenCloseAction.GetDisplayKey();
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