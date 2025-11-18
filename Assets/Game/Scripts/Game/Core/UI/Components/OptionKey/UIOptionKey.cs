using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.UI
{
    public sealed class UIOptionKey : UIOption
    {
        [ field: SerializeField ] public UIKeyBox Keyboard { get; private set; }
        [ field: SerializeField ] public UIKeyBox Mouse { get; private set; }
        [ field: SerializeField ] public UIKeyBox Gamepad { get; private set; }
        [ SerializeField ] private Image _back;

        public void SetBackColor( Color color )
        {
            _back.color = color;
        }
    }
}