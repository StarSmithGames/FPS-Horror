using System;

namespace Game.Core.UI.OptionsDialog
{
    public abstract class OptionController
    {
        public bool IsDirty { get; protected set; }

        private readonly UIOption _view;
        
        public OptionController( UIOption view )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
        }

        public void SetName( string name )
        {
            _view.SetName( name );
        }

        public void ResetDirty()
        {
            IsDirty = false;
        }
    }
}