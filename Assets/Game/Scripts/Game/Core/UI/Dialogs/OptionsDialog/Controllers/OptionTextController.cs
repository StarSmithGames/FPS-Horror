using System;

namespace Game.Core.UI.OptionsDialog
{
    public abstract class OptionTextController : OptionController
    {
        private UIOptionText _view { get; }
        
        public OptionTextController( UIOptionText view ) : base( view )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
        }
        
        public void SetName( string name )
        {
            _view.SetName( name );
        }
    }
}