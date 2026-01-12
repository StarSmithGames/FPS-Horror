using TMPro;
using UnityEngine;

namespace Game.Core.UI
{
    public class UIOptionText : UIOption
    {
        [ SerializeField ] private TextMeshProUGUI _name;
        
        public void SetName( string name )
        {
            _name.text = name;
        }

        public void SetNameColor( Color color )
        {
            _name.color = color;
        }
    }
}