using System;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class OptionKeyController : OptionController
    {
        private UIOptionKey _view;
        
        public OptionKeyController( UIOptionKey view ) : base( view )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
        }

        public override void Initialize()
        {
            base.Initialize();

            _view.Keyboard.OnButtonClicked += KeyboardButtonClickedHandler;
            _view.Mouse.OnButtonClicked += MouseButtonClickedHandler;
            _view.Gamepad.OnButtonClicked += GamepadButtonClickedHandler;
        }

        public override void Dispose()
        {
            base.Dispose();
            
            _view.Keyboard.OnButtonClicked -= KeyboardButtonClickedHandler;
            _view.Mouse.OnButtonClicked -= MouseButtonClickedHandler;
            _view.Gamepad.OnButtonClicked -= GamepadButtonClickedHandler;
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

        private void KeyboardButtonClickedHandler( UIKeyBox uiKeyBox )
        {
            
        }
        
        private void MouseButtonClickedHandler( UIKeyBox uiKeyBox )
        {
            
        }
        
        private void GamepadButtonClickedHandler( UIKeyBox uiKeyBox )
        {
            
        }
    }
}