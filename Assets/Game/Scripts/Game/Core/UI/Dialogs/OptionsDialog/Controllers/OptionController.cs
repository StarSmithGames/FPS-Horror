using System;

namespace Game.Core.UI.OptionsDialog
{
    public abstract class OptionController
    {
        public bool IsDirty { get; protected set; }

        public UIOption View { get; }
        
        public OptionController( UIOption view )
        {
            View = view ?? throw new ArgumentNullException( nameof(view) );
        }

        public void SetName( string name )
        {
            View.SetName( name );
        }

        public void Select()
        {
            View.Select();
        }

        public void Deselect()
        {
            View.Deselect();
        }

        public void ResetDirty()
        {
            IsDirty = false;
        }
    }
}