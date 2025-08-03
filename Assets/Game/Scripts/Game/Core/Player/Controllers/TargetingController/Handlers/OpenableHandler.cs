using Game.Core.Entity;
using Game.Core.UI;
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

        private Func< bool > _breaker;
        private InputActionProvider _provider;
        private OpenableObject _openable;
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

        public void Initialize( Func< bool > breaker)
        {
            _provider = new( _interactionsSettings.OpenCloseAction.InputAction, InteractCompleted, 0.33f, breaker: breaker, progress: InteractProgress, callback: InteractFinished );
        }
        
        public void Enable( UIInfoButton ui, OpenableObject openable )
        {
            _ui = ui ?? throw new ArgumentNullException( nameof(ui) );
            _openable = openable ?? throw new ArgumentNullException( nameof(openable) );

            _contextMenuOperations = new()
            {
                new()
                {
                    Key =  _interactionsSettings.OpenCloseAction.GetDisplayKey(),
                    Name = _localizationSystem.Translate( _openable.IsOpen ? _interactionsSettings.OpenCloseAction.AdditionalNameIds[ 0 ] : _interactionsSettings.OpenCloseAction.NameId  )
                }
            };
            _provider.Enable();
        }

        public void Disable()
        {
            _provider.Disable();
            _ui = null;
            
            _contextMenuOperations?.Clear();
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
            if ( _openable.IsOpen )
            {
                _openable.Close();
            }
            else
            {
                _openable.Open();
            }
            
            Completed();
        }
        
        public override List< ContextMenuOperation > GetContextMenuOptions() => _contextMenuOperations;
    }
}