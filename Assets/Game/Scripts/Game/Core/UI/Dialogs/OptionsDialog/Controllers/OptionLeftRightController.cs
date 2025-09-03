using System;
using UnityEngine;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class OptionLeftRightController
    {
        private int _index;
        private string[] _options;
        
        private readonly UIOptionLeftRight _view;
        
        public OptionLeftRightController( UIOptionLeftRight view )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
        }

        public void Initialize( int index = 0, params string[] options )
        {
            _index = index;
            _options = options;
            
            _view.OnLeftButtonClicked += LeftButtonClickedHandler;
            _view.OnRightButtonClicked += RightButtonClickedHandler;

            RefreshUI();
        }

        public void Dispose()
        {
            _view.OnLeftButtonClicked -= LeftButtonClickedHandler;
            _view.OnRightButtonClicked -= RightButtonClickedHandler;
        }

        private void RefreshUI()
        {
            _view.SetText( _options[ _index ] );
        }

        private void LeftButtonClickedHandler()
        {
            _index = Mathf.Clamp( _index - 1, 0, _options.Length - 1 );
            
            RefreshUI();
        }

        private void RightButtonClickedHandler()
        {
            _index = Mathf.Clamp( _index + 1, 0, _options.Length - 1 );
            
            RefreshUI();
        }
    }
}