using System;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class OptionKeyController : OptionController
    {
        public event Action< OptionKeyController > OnKeyButtonClicked;
        
        public UIOptionKey ViewKey { get; }
        
        public OptionKeyController( UIOptionKey view ) : base( view )
        {
            ViewKey = view ?? throw new ArgumentNullException( nameof(view) );
        }

        public override void Initialize()
        {
            base.Initialize();

            ViewKey.Keyboard.OnButtonClicked += KeyboardButtonClickedHandler;
            ViewKey.Mouse.OnButtonClicked += MouseButtonClickedHandler;
            ViewKey.Gamepad.OnButtonClicked += GamepadButtonClickedHandler;
        }

        public override void Dispose()
        {
            base.Dispose();
            
            ViewKey.Keyboard.OnButtonClicked -= KeyboardButtonClickedHandler;
            ViewKey.Mouse.OnButtonClicked -= MouseButtonClickedHandler;
            ViewKey.Gamepad.OnButtonClicked -= GamepadButtonClickedHandler;
        }
        
        public override void Select()
        {
            base.Select();
            
            ViewKey.SetBackColor( new( 1, 1, 1, 0.3f ) );
        }

        public override void Deselect()
        {
            base.Deselect();

            ViewKey.SetBackColor( new( 1, 1, 1, 0.0f ) );
        }

        private void KeyboardButtonClickedHandler( UIKeyBox uiKeyBox )
        {
            OnKeyButtonClicked?.Invoke( this );
        }
        
        private void MouseButtonClickedHandler( UIKeyBox uiKeyBox )
        {
            OnKeyButtonClicked?.Invoke( this );
        }
        
        private void GamepadButtonClickedHandler( UIKeyBox uiKeyBox )
        {
            OnKeyButtonClicked?.Invoke( this );
        }
    }
}