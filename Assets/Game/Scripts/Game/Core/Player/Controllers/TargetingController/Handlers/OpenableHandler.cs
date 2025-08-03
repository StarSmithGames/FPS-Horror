using Game.Core.UI;
using Game.Core.World.InteractionSystem;
using Game.Core.World.Systems.InteractionSystem;
using Game.Managers.InputManager;
using PuzzlescapeGames.Localization;
using System;
using System.Collections.Generic;

namespace Game.Core.Player
{
    public sealed class OpenableHandler : InteractableHandler
    {
        private List< ContextMenuOperation > _contextMenuOperations;
        
        private InputActionProvider _provider;
        private IOpenable _openable;
        private UIInfoButton _ui;
        
        private readonly InteractionsSettings _interactionsSettings;
        private readonly ILocalizationSystem _localizationSystem;
        
        public OpenableHandler(
            InteractionsSettings interactionsSettings,
            ILocalizationSystem localizationSystem
            )
        {
            _interactionsSettings = interactionsSettings ?? throw new ArgumentNullException( nameof(interactionsSettings) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }
        
        public void Initialize( Func< bool > breaker )
        {
            _provider = new( _interactionsSettings.OpenAction.InputAction, InteractCompleted, 0.33f, breaker: breaker, progress: InteractProgress, callback: InteractFinished );

            _contextMenuOperations = new()
            {
                new()
                {
                    Key = _interactionsSettings.OpenAction.GetDisplayKey(),
                    Name = _localizationSystem.Translate( _interactionsSettings.OpenAction.NameId )
                }
            };
        }

        public void Enable( UIInfoButton ui, IOpenable openable )
        {
            _ui = ui ?? throw new ArgumentNullException( nameof(ui) );
            _openable = openable ?? throw new ArgumentNullException( nameof(openable) );
            
            _provider.Enable();
        }

        public void Disable()
        {
            _provider.Disable();
            _ui = null;
        }
        
        private void InteractProgress( float value )
        {
            _ui.SetFillAmount( value );
        }

        private void InteractFinished( bool result )
        {
            _ui.SetFillAmount( 0 );
        }
        
        private void InteractCompleted()
        {
            // _openable.Interact();
        }
        
        public override List< ContextMenuOperation > GetContextMenuOptions() => _contextMenuOperations;
    }
}