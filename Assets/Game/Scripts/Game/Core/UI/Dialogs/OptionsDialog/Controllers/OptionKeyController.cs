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
        }

        public override void Dispose()
        {
            base.Dispose();
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
    }
}