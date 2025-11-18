using System;
using UnityEngine;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class OptionSelectorController : OptionController
    {
        public int Index { get; private set; }
        
        private string[] _options;
        
        private readonly UIOptionLeftRight _view;
        
        public OptionSelectorController( UIOptionLeftRight view, int index ) : base( view )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            Index = index;
        }

        public void Initialize( params string[] options )
        {
            base.Initialize();
            
            _options = options;

            IsDirty = false;
            
            _view.OnLeftButtonClicked += LeftButtonClickedHandler;
            _view.OnRightButtonClicked += RightButtonClickedHandler;

            _view.SetCenter( 1 );
            
            RefreshUI();
        }

        public override void Dispose()
        {
            base.Dispose();
            
            _view.OnLeftButtonClicked -= LeftButtonClickedHandler;
            _view.OnRightButtonClicked -= RightButtonClickedHandler;
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

        private void RefreshUI()
        {
            _view.SetText( _options[ Index ] );
        }

        public void Left()
        {
            Index = Mathf.Clamp( Index - 1, 0, _options.Length - 1 );
            
            RefreshUI();

            IsDirty = true;
        }

        public void Right()
        {
            Index = Mathf.Clamp( Index + 1, 0, _options.Length - 1 );
            
            RefreshUI();
            
            IsDirty = true;
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