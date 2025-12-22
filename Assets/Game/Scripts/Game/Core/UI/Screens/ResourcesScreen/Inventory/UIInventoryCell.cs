using Game.Systems.InventorySystem;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.UI.ResourcesScreen
{
    public sealed class UIInventoryCell : MonoBehaviour
    {
        [ SerializeField ] private Image _icon;
        [ SerializeField ] private GameObject _opened;
        [ SerializeField ] private GameObject _locked;

        public void Set( InventoryItemModel item )
        {
            _icon.sprite = item.Icon;
        }

        public void SetLock( bool trigger )
        {
            _opened.SetActive( !trigger );
            _locked.SetActive( trigger );
        }
    }
}