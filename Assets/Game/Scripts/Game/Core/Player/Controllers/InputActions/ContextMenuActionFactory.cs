using System;
using Zenject;

namespace Game.Core.Player
{
    public sealed class ContextMenuActionFactory
    {
        private ActionHandlerComposite _open = null;
        private ActionHandlerComposite _pull = null;
        private ActionHandlerComposite _inspect = null;
        private ActionHandlerComposite _item = null;
        private ActionHandlerComposite _itemNote = null;
        private ActionHandlerComposite _puzzleUse = null;

        private readonly DiContainer _diContainer;
        
        public ContextMenuActionFactory( DiContainer diContainer )
        {
            _diContainer = diContainer ?? throw new ArgumentNullException( nameof(diContainer) );
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

        public ActionHandlerComposite GetOrCreatePuzzleHandler()
        {
            if ( _puzzleUse == null )
            {
                _puzzleUse = new( new() { _diContainer.Instantiate< InteractActionHandler >() } );
            }
        
            return _puzzleUse;
        }
    }
}