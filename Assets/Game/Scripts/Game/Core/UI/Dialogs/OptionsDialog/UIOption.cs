using TMPro;
using UnityEngine;

namespace Game.Core.UI.OptionsDialog
{
    public class UIOption : MonoBehaviour
    {
        [ SerializeField ] private TextMeshProUGUI _name;
        
        public void SetName( string name )
        {
            _name.text = name;
        }
    }
}