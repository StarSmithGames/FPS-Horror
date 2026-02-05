using Game.Core.Entity;
using System;
using Zenject;

namespace Game.Core.Player
{
    public sealed class InteractionActionFactory
    {
        private InteractActionHandler _interact;
        private InspectActionHandler _inspect;
        private OpenCloseActionHandler _openClose;
        
        private readonly DiContainer _diContainer;
        
        public InteractionActionFactory( DiContainer diContainer )
        {
            _diContainer = diContainer ?? throw new ArgumentNullException( nameof(diContainer) );
        }
        
        public InspectActionHandler GetOrCreateItemHandler( ItemObject item )
        {
            if ( _inspect == null )
            {
                _inspect = _diContainer.Instantiate< InspectActionHandler >();
            }
            _inspect.Set( item );
        
            return _inspect;
        }
        
        public OpenCloseActionHandler GetOrCreateOpenCloseHandler( OpenCloseObject dynamic )
        {
            if ( _openClose == null )
            {
                _openClose = _diContainer.Instantiate< OpenCloseActionHandler >();
            }
            _openClose.Set( dynamic );
            
            return _openClose;
        }

        public InteractActionHandler GetOrCreatePuzzleHandler( InteractableObject interactable )
        {
            if ( _interact == null )
            {
                _interact = _diContainer.Instantiate< InteractActionHandler >();
            }
            _interact.Set( interactable );
        
            return _interact;
        }
    }
}