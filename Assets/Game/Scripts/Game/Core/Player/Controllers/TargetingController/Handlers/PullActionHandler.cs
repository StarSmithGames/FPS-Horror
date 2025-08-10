using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.World.InteractionSystem;
using Game.Core.World.Systems.InteractionSystem;
using Game.Managers.InputManager;
using PuzzlescapeGames.Localization;
using System;

namespace Game.Core.Player
{
    public sealed class PullActionHandler : ActionHandler
    {
        private Func< bool > _breaker;
        private InputActionProvider _provider;
        private PullableObject _pullable;
        private UIInfoButton _ui;
        
        private readonly InteractionsSettings _interactionsSettings;
        private readonly ILocalizationSystem _localizationSystem;
        
        public PullActionHandler(
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
            _pullable = (PullableObject)target;
            
            if ( ContextMenuOperation == null )
            {
                ContextMenuOperation = new();
            }
            ContextMenuOperation.Key = _interactionsSettings.OpenCloseAction.GetDisplayKey();
            ContextMenuOperation.Name = _localizationSystem.Translate( _pullable.IsOpen ? _interactionsSettings.OpenCloseAction.AdditionalNameIds[ 0 ] : _interactionsSettings.OpenCloseAction.NameId );
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