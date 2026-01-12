using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.UI
{
    public sealed class UIOptionModalTextButton : UIOptionText
    {
        [ SerializeField ] private Color _nameNormal;
        [ SerializeField ] private Color _nameSelected;
        [ SerializeField ] private Image _back;
        [ SerializeField ] private Color _backNormal;
        [ SerializeField ] private Color _backSelected;

        public override void Select()
        {
            base.Select();

            SetNameColor( _nameSelected );
            _back.color = _backSelected;
        }

        public override void Deselect()
        {
            base.Deselect();

            SetNameColor( _nameNormal );
            _back.color = _backNormal;
        }
    }
}