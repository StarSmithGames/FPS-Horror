using System;
using UnityEngine;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class OptionPercentsController : OptionController
    {
        public int Value { get; private set; }
        
        private readonly UIOptionLeftRight _view;
        
        public OptionPercentsController( UIOptionLeftRight view, int value ) : base( view )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            Value = value;
        }

        public void Initialize()
        {
            IsDirty = false;
            
            _view.OnLeftButtonClicked += LeftButtonClickedHandler;
            _view.OnRightButtonClicked += RightButtonClickedHandler;

            _view.SetCenter( 0 );
            
            RefreshUI();
        }

        public void Dispose()
        {
            _view.OnLeftButtonClicked -= LeftButtonClickedHandler;
            _view.OnRightButtonClicked -= RightButtonClickedHandler;
        }

        public void Left()
        {
            Value = Mathf.Clamp( Value - 1, 0, 100 );
            
            RefreshUI();

            IsDirty = true;
        }

        public void Right()
        {
            Value = Mathf.Clamp( Value + 1, 0, 100 );
            
            RefreshUI();
            
            IsDirty = true;
        }

        private void RefreshUI()
        {
            _view.SetText( $"{Value}%" );
        }

        private void LeftButtonClickedHandler()
        {
            Left();
        }

        private void RightButtonClickedHandler()
        {
            Right();
        }
    }
}