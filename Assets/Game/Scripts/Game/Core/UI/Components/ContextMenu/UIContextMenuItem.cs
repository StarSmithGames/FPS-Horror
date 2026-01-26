using Game.Systems.InventorySystem.ContextMenu;
using PuzzlescapeGames.Localization;
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
        
        [ SerializeField ] private Image _icon;
        [ SerializeField ] private RectTransform _iconRect;
        [ SerializeField ] private TextMeshProUGUI _name;
        [ SerializeField ] private GameObject _selected;
        
        public void Set( ContextMenuItem item, string name )
        {
            _icon.sprite = item.Icon;
            _iconRect.sizeDelta = item.IconSize;
            _name.text = name;
            
            _selected.SetActive( false );
        }

        public void OnPointerEnter( PointerEventData eventData )
        {
            _selected.SetActive( true );
        }

        public void OnPointerExit( PointerEventData eventData )
        {
            _selected.SetActive( false );
        }

        public void OnPointerClick( PointerEventData eventData )
        {
            
        }
    }
}