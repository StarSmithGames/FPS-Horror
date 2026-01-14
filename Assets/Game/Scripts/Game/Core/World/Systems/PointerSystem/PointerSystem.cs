using Game.Core.Entity;
using System;

namespace Game.Core.World.PointerSystem
{
    public sealed class PointerSystem
    {
        private readonly InteractionPointerFactory _interactionPointerFactory;
        
        public PointerSystem( InteractionPointerFactory interactionPointerFactory )
        {
            _interactionPointerFactory = interactionPointerFactory ?? throw new ArgumentNullException( nameof(interactionPointerFactory) );
        }

        public InteractionPointer CreateIndicator()
        {
            var indicator = _interactionPointerFactory.Create();
            return indicator;
        }
    }
}