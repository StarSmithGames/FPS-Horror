using System;
using UnityEngine;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class OptionBarController : OptionController
    {
        public float Value { get; private set; }
        public float Min { get; }
        public float Max { get; }

        private float _step;
        private string _postfix;
        
        private readonly UIOptionLeftRight _view;
        
        public OptionBarController( UIOptionLeftRight view, float value, float min = 0, float max = 100, float step = 1, string postfix = "%" ) : base( view )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            Value = value;
            Min = min;
            Max = max;
            _step = step;
            _postfix = postfix;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            IsDirty = false;
            
            _view.OnLeftButtonClicked += LeftButtonClickedHandler;
            _view.OnRightButtonClicked += RightButtonClickedHandler;

            _view.SetCenter( 0 );
            
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

        public void Left()
        {
            Value = Mathf.Clamp( Value - _step, Min, Max );
            
            RefreshUI();

            IsDirty = true;
        }

        public void Right()
        {
            Value = Mathf.Clamp( Value + _step, Min, Max );
            
            RefreshUI();
            
            IsDirty = true;
        }

        private void RefreshUI()
        {
            _view.SetText( $"{Value}{_postfix}" );
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