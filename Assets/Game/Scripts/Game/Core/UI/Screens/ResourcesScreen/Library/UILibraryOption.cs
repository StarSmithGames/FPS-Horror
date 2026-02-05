using TMPro;
using UnityEngine;

namespace Game.Core.UI.ResourcesScreen
{
    public sealed class UILibraryOption : UIOption
    {
        [ SerializeField ] private TextMeshProUGUI _text;
        
        public void SetText( string text ) => _text.text = text;
    }
}