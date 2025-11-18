using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.UI
{
    public sealed class UIOptionKey : UIOption
    {
        [ SerializeField ] private UIKeyBox _keyboard;
        [ SerializeField ] private UIKeyBox _mouse;
        [ SerializeField ] private UIKeyBox _gamepad;
        [ SerializeField ] private Image _back;

        public void SetBackColor( Color color )
        {
            _back.color = color;
        }
    }
}