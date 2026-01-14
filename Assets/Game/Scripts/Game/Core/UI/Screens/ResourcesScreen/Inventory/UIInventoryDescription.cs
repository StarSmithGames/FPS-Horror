using Game.Systems.InventorySystem;
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
        
        public void Set( ItemConfig config, ItemDescriptor descriptor )
        {
            _itemName.text = descriptor.GetName( config );
            _itemType.text = descriptor.GetType( config );
            _itemDescription.text = descriptor.GetDescription( config );
        }
    }
}