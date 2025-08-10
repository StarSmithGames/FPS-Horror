using System;

namespace Game.Core.Entity
{
    public sealed class LighterController : ItemController
    {
        private readonly LighterItemObject _view;
        
        public LighterController( LighterItemObject view ) : base( view )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
        }

        public void Show()
        {
            _view.EnableLight( true );
        }

        public void Hide()
        {
            _view.EnableLight( false );
        }
    }
}