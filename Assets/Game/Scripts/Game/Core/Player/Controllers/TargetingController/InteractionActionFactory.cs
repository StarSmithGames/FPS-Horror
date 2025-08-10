using System;
using Zenject;

namespace Game.Core.Player
{
    public sealed class InteractionActionFactory
    {
        private CompositeHandler _open;
        private CompositeHandler _pull;
        private CompositeHandler _inspect;
        private CompositeHandler _item;
        private CompositeHandler _itemNote;

        private readonly DiContainer _diContainer;
        
        public InteractionActionFactory( DiContainer diContainer )
        {
            _diContainer = diContainer ?? throw new ArgumentNullException( nameof(diContainer) );
        }
        
        public CompositeHandler GetOrCreateItemHandler()
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

        public CompositeHandler GetOrCreateItemNoteHandler()
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
        
        public CompositeHandler GetOrCreateOpenableHandler()
        {
            if ( _open == null )
            {
                _open = new( new() { _diContainer.Instantiate< OpenActionHandler >() } );
            }

            return _open;
        }
        
        public CompositeHandler GetOrCreatePullableHandler()
        {
            if ( _pull == null )
            {
                _pull = new( new() { _diContainer.Instantiate< PullActionHandler >() } );
            }
            return _pull;
        }
    }
}