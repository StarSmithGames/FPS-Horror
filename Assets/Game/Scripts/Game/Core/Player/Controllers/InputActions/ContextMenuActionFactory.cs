using System;
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
        
        public ActionHandlerComposite GetOrCreateOpenableHandler()
        {
            if ( _open == null )
            {
                _open = new( new() { _diContainer.Instantiate< OpenActionHandler >() } );
            }

            return _open;
        }
        
        public ActionHandlerComposite GetOrCreatePullableHandler()
        {
            if ( _pull == null )
            {
                _pull = new( new() { _diContainer.Instantiate< PullActionHandler >() } );
            }
            return _pull;
        }
    }
}