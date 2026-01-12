using Game.Core.Entity;
using System;

namespace Game.Core.World.IndicatorManager
{
    public sealed class IndicatorManager
    {
        private readonly InteractionIndicatorFactory _interactionIndicatorFactory;
        
        public IndicatorManager( InteractionIndicatorFactory interactionIndicatorFactory )
        {
            _interactionIndicatorFactory = interactionIndicatorFactory ?? throw new ArgumentNullException( nameof(interactionIndicatorFactory) );
        }

        public InteractionIndicator CreateIndicator( ObservableObject observable )
        {
            var indicator = _interactionIndicatorFactory.Create();
            return indicator;
        }
    }
}