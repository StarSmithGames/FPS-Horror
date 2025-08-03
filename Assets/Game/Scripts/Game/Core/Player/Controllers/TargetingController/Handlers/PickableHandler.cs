using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.World.Systems.InteractionSystem;
using Game.Managers.InputManager;
using PuzzlescapeGames.Localization;
using System;
using System.Collections.Generic;

namespace Game.Core.Player
{
    public class PickableHandler : InteractableHandler
    {
        private List< ContextMenuOperation > _contextMenuOperations;
        
        private InputActionProvider _provider;
        private ItemObject _item;
        private UIInfoButton _ui;
        
        private readonly InteractionsSettings _interactionsSettings;
        private readonly ILocalizationSystem _localizationSystem;
        
        public PickableHandler(
            InteractionsSettings interactionsSettings,
            ILocalizationSystem localizationSystem
            )
        {
            _interactionsSettings = interactionsSettings ?? throw new ArgumentNullException( nameof(interactionsSettings) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }
        
        public void Initialize( Func< bool > breaker )
        {
            _provider = new( _interactionsSettings.InteractAction.InputAction, InteractCompleted, 0.33f, breaker: breaker, progress: InteractProgress, callback: InteractFinished );

            _contextMenuOperations = new()
            {
                new()
                {
                    Key = _interactionsSettings.InteractAction.GetDisplayKey(),
                    Name = _localizationSystem.Translate( _interactionsSettings.InteractAction.NameId )
                }
            };
        }

        public void Enable( UIInfoButton ui, ItemObject item )
        {
            _ui = ui ?? throw new ArgumentNullException( nameof(ui) );
            _item = item ?? throw new ArgumentNullException( nameof(item) );
            
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
            _item.Interact();
        }
        
        public override List< ContextMenuOperation > GetContextMenuOptions() => _contextMenuOperations;
    }
}