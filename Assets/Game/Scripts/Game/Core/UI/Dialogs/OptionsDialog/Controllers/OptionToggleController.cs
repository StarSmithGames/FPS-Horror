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

        public override void Initialize()
        {
            base.Initialize();
            
            _view.OnToggleClicked += ToggleClickedHandler;
        }

        public override void Dispose()
        {
            base.Dispose();
            
            _view.OnToggleClicked -= ToggleClickedHandler;
        }
        
        public override void Select()
        {
            base.Select();
            
            _view.SetBackColor( new( 1, 1, 1, 0.3f ) );
        }

        public override void Deselect()
        {
            base.Deselect();

            _view.SetBackColor( new( 1, 1, 1, 0.0f ) );
        }

        private void ToggleClickedHandler( bool result )
        {
            IsDirty = true;
        }
    }
}