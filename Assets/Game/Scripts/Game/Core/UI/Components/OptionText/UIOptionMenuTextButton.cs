using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.UI
{
    public sealed class UIOptionMenuTextButton : UIOptionText
    {
        [ SerializeField ] private Image _point;
        [ SerializeField ] private Color _normal;
        [ SerializeField ] private Color _selected;

        public override void Select()
        {
            base.Select();

            SetNameColor( _selected );
            _point.gameObject.SetActive( true );
        }

        public override void Deselect()
        {
            base.Deselect();

            SetNameColor( _normal );
            _point.gameObject.SetActive( false );
        }
    }
}