using Game.Core.World.InventorySystem.ContextMenu;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Core.UI.ContextMenu
{
    public sealed class UIContextMenuItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public event Action< UIContextMenuItem > OnPointerEntered;
        public event Action< UIContextMenuItem > OnPointerExited;
        public event Action< UIContextMenuItem > OnPointerClicked;
        
        [ SerializeField ] private Image _icon;
        [ SerializeField ] private RectTransform _iconRect;
        [ SerializeField ] private TextMeshProUGUI _name;
        [ SerializeField ] private GameObject _selected;
        
        public MenuItemCommand ItemCommand { get; private set; }
        
        public void Set( MenuItemCommand itemCommand, string name )
        {
            ItemCommand = itemCommand;
            
            _icon.sprite = ItemCommand.Item.Icon;
            _iconRect.sizeDelta = ItemCommand.Item.IconSize;
            _name.text = name;
            
            _selected.SetActive( false );
        }
        
        public void Select()
        {
            _selected.SetActive( true );
        }
        
        public void Deselect()
        {
            _selected.SetActive( false );
        }

        public void OnPointerEnter( PointerEventData eventData )
        {
            OnPointerEntered?.Invoke( this );
        }

        public void OnPointerExit( PointerEventData eventData )
        {
            OnPointerExited?.Invoke( this );
        }

        public void OnPointerClick( PointerEventData eventData )
        {
            OnPointerClicked?.Invoke( this );
        }
    }
}