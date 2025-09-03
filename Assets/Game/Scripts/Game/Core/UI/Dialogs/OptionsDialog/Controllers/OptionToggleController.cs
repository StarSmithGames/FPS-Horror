using System;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class OptionToggleController : OptionController
    {
        public bool IsOn => _view.Toggle.isOn;
        
        private readonly UIOptionToggle _view;
        
        public OptionToggleController( UIOptionToggle view, bool isOn ) : base( view )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );

            _view.Toggle.isOn = isOn;
        }

        public void Initialize()
        {
            _view.OnToggleClicked += ToggleClickedHandler;
        }

        public void Dispose()
        {
            _view.OnToggleClicked -= ToggleClickedHandler;
        }

        private void ToggleClickedHandler( bool result )
        {
            IsDirty = true;
        }
    }
}