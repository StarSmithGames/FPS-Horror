using Game.Core.World.InventorySystem;
using PuzzlescapeGames.Extensions;
using UnityEngine;

namespace Game.Core.UI.ResourcesScreen
{
    public sealed class UIInventoryDescription : MonoBehaviour
    {
        [ SerializeField ] private CanvasGroup _canvasGroup;
        [ SerializeField ] private TMPro.TextMeshProUGUI _itemName;
        [ SerializeField ] private TMPro.TextMeshProUGUI _itemType;
        [ SerializeField ] private TMPro.TextMeshProUGUI _itemDescription;

        public void Enable( bool trigger )
        {
            _canvasGroup.Enable( trigger );
        }
        
        public void Set( string name, string description, string type )
        {
            _itemName.text = name;
            _itemType.text = type;
            _itemDescription.text = description;
        }
    }
}