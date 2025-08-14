using PuzzlescapeGames.Localization;
using System;
using System.Linq;
using Zenject;

namespace Game.Core.Player
{
    public sealed class ContextMenuActionFactory
    {
        private ActionHandlerComposite _open;
        private ActionHandlerComposite _pull;
        private ActionHandlerComposite _inspect;
        private ActionHandlerComposite _item;
        private ActionHandlerComposite _itemNote;
        private ActionHandlerComposite _puzzleUse;

        private readonly DiContainer _diContainer;
        private readonly ILocalizationSystem _localizationSystem;
        
        public ContextMenuActionFactory(
            DiContainer diContainer,
            ILocalizationSystem localizationSystem
            )
        {
            _diContainer = diContainer ?? throw new ArgumentNullException( nameof(diContainer) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }
        
        public ActionHandlerComposite GetOrCreateItemHandler()
        {
            if ( _item == null )
            {
                _item = new( new()
                {
                    _diContainer.Instantiate< PickUpActionHandler >(),
                    _diContainer.Instantiate< InspectActionHandler >()
                } );
            }

            return _item;
        }

        public ActionHandlerComposite GetOrCreateItemNoteHandler()
        {
            if ( _itemNote == null )
            {
                _itemNote = new( new()
                {
                    _diContainer.Instantiate< InspectActionHandler >()
                } );
            }

            return _itemNote;
        }
        
        public ActionHandlerComposite GetOrCreateOpenCloseHandler()
        {
            if ( _open == null )
            {
                _open = new( new() { _diContainer.Instantiate< OpenCloseActionHandler >() } );
            }

            return _open;
        }

        // public ActionHandlerComposite GetOrCreatePuzzleHandler()
        // {
        //     if ( _puzzleUse == null )
        //     {
        //         _puzzleUse = new();
        //     }
        //
        //     return _puzzleUse;
        // }
    }
}