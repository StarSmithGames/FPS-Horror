using System;

namespace Game.Core.UI.OptionsDialog
{
    public abstract class OptionController
    {
        public event Action< OptionController > OnPointerEntered;
        public event Action< OptionController > OnPointerExited;
        public event Action< OptionController > OnButtonClicked;
        
        public bool IsDirty { get; protected set; }
        public bool IsSelected => View.IsSelected;

        public UIOption View { get; }
        
        public OptionController( UIOption view )
        {
            View = view ?? throw new ArgumentNullException( nameof(view) );
        }

        public virtual void Initialize()
        {
            View.OnPointerEntered += PointerEnteredHandler;
            View.OnPointerExited += PointerExitedHandler;
            View.OnButtonClicked += ButtonClickedHandler;
        }

        public virtual void Dispose()
        {
            View.OnPointerEntered -= PointerEnteredHandler;
            View.OnPointerExited -= PointerExitedHandler;
            View.OnButtonClicked -= ButtonClickedHandler;
        }

        public void SetName( string name )
        {
            View.SetName( name );
        }

        public virtual void Select()
        {
            View.Select();
        }

        public virtual void Deselect()
        {
            View.Deselect();
        }

        public void ResetDirty()
        {
            IsDirty = false;
        }

        private void PointerEnteredHandler( UIOption option )
        {
            OnPointerEntered?.Invoke( this );
        }
        
        private void PointerExitedHandler( UIOption option )
        {
            OnPointerExited?.Invoke( this );
        }
        
        private void ButtonClickedHandler( UIOption option )
        {
            OnButtonClicked?.Invoke( this );
        }
    }
}