using Game.Core.Entity;
using System;

namespace Game.Core.Player
{
    public sealed class InteractionActionController
    {
        private ActionHandler _actionHandler;

        private readonly InteractionActionFactory _interactionActionFactory;
        
        public InteractionActionController( InteractionActionFactory interactionActionFactory )
        {
            _interactionActionFactory = interactionActionFactory ?? throw new ArgumentNullException( nameof(interactionActionFactory) );
        }
        
        public void Initialize()
        {
            
        }
        
        public void SetToDynamic( OpenCloseObject dynamic )
        {
            _actionHandler = _interactionActionFactory.GetOrCreateOpenCloseHandler( dynamic );
        }

        public void SetToItem( ItemObject item )
        {
            _actionHandler = _interactionActionFactory.GetOrCreateItemHandler( item );
        }

        public void SetToPuzzle( PuzzleObject puzzle )
        {
            _actionHandler = _interactionActionFactory.GetOrCreatePuzzleHandler( puzzle );
        }
        
        public void CurrentObservableChangedHandler( ObservableObject observable )
        {
            if ( observable == null )
            { 
                _actionHandler?.Dispose();
                _actionHandler = null;
                return;
            }

            _actionHandler?.Enable();
        }
    }
}