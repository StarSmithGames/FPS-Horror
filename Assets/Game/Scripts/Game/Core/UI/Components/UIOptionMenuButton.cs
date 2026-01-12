using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.UI
{
    public class UIOptionMenuButton : UIOption
    {
        [ SerializeField ] private Image _icon;
        [ SerializeField ] private Image _point;
        [ SerializeField ] private Color _normal;
        [ SerializeField ] private Color _selected;

        public override void Select()
        {
            base.Select();

            _icon.color = _selected;
            _point.color = _selected;
        }

        public override void Deselect()
        {
            base.Deselect();

            _icon.color = _normal;
            _point.color = _normal;
        }
    }
}