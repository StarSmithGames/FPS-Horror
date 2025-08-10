using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.World.InteractionSystem;
using Game.Managers.InputManager;
using PuzzlescapeGames.Localization;
using System;

namespace Game.Core.Player
{
    public sealed class PullActionHandler : ContextMenuActionHandler
    {
        private Func< bool > _breaker;
        private InputActionProvider _provider;
        private PullableObject _pullable;
        private UIInfoButton _ui;
        
        private readonly InputKeyActionsSettings _inputKeyActionsSettings;
        private readonly ILocalizationSystem _localizationSystem;
        
        public PullActionHandler(
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
            _pullable = (PullableObject)target;
            
            if ( ContextMenuOperation == null )
            {
                ContextMenuOperation = new();
            }
            ContextMenuOperation.Key = _inputKeyActionsSettings.OpenCloseAction.GetDisplayKey();
            ContextMenuOperation.Name = _localizationSystem.Translate( _pullable.IsOpen ? _inputKeyActionsSettings.OpenCloseAction.AdditionalNameIds[ 0 ] : _inputKeyActionsSettings.OpenCloseAction.NameId );
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
            _pullable = null;

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
            
            if ( _pullable.IsOpen )
            {
                _pullable.Close();
            }
            else
            {
                _pullable.Open();
            }
            
            base.Completed();
        }
    }
}