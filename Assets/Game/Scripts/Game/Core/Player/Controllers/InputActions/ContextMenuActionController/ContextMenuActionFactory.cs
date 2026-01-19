using System;
using Zenject;

namespace Game.Core.Player
{
    public sealed class ContextMenuActionFactory
    {
        private ContextHandlerComposite _open = null;
        private ContextHandlerComposite _pull = null;
        private ContextHandlerComposite _inspect = null;
        private ContextHandlerComposite _item = null;
        private ContextHandlerComposite _itemNote = null;
        private ContextHandlerComposite _puzzleUse = null;

        private readonly DiContainer _diContainer;
        
        public ContextMenuActionFactory( DiContainer diContainer )
        {
            _diContainer = diContainer ?? throw new ArgumentNullException( nameof(diContainer) );
        }
        
        public ContextHandlerComposite GetOrCreateItemHandler()
        {
            if ( _item == null )
            {
                _item = new( new()
                {
                    _diContainer.Instantiate< PickUpActionHandler >(),
                } );
            }

            return _item;
        }

        public ContextHandlerComposite GetOrCreateItemNoteHandler()
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
        
        public ContextHandlerComposite GetOrCreateOpenCloseHandler()
        {
            if ( _open == null )
            {
                _open = new( new() { _diContainer.Instantiate< OpenCloseActionHandler >() } );
            }

            return _open;
        }

        public ContextHandlerComposite GetOrCreatePuzzleHandler()
        {
            if ( _puzzleUse == null )
            {
                _puzzleUse = new( new() { _diContainer.Instantiate< InteractActionHandler >() } );
            }
        
            return _puzzleUse;
        }
    }
}