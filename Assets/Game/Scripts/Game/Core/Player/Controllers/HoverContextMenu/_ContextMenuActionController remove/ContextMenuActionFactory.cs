using System;
using Zenject;

namespace Game.Core.Player
{
    public sealed class ContextMenuActionFactory
    {
        private ContextMenuActionHandlerComposite _open = null;
        private ContextMenuActionHandlerComposite _pull = null;
        private ContextMenuActionHandlerComposite _inspect = null;
        private ContextMenuActionHandlerComposite _item = null;
        private ContextMenuActionHandlerComposite _itemNote = null;
        private ContextMenuActionHandlerComposite _puzzleUse = null;

        private readonly DiContainer _diContainer;
        
        public ContextMenuActionFactory( DiContainer diContainer )
        {
            _diContainer = diContainer ?? throw new ArgumentNullException( nameof(diContainer) );
        }
        
        public ContextMenuActionHandlerComposite GetOrCreateItemHandler()
        {
            if ( _item == null )
            {
                _item = new( new()
                {
                    _diContainer.Instantiate< ContextMenuPickUpActionHandler >(),
                } );
            }

            return _item;
        }

        public ContextMenuActionHandlerComposite GetOrCreateItemNoteHandler()
        {
            if ( _itemNote == null )
            {
                _itemNote = new( new()
                {
                    _diContainer.Instantiate< ContextMenuInspectActionHandler >()
                } );
            }

            return _itemNote;
        }
        
        public ContextMenuActionHandlerComposite GetOrCreateOpenCloseHandler()
        {
            if ( _open == null )
            {
                _open = new( new() { _diContainer.Instantiate< ContextMenuOpenCloseActionHandler >() } );
            }

            return _open;
        }

        public ContextMenuActionHandlerComposite GetOrCreatePuzzleHandler()
        {
            if ( _puzzleUse == null )
            {
                _puzzleUse = new( new() { _diContainer.Instantiate< ContextMenuInteractActionHandler >() } );
            }
        
            return _puzzleUse;
        }
    }
}