using Game.Systems.InventorySystem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Core.UI.ResourcesScreen
{
    public sealed class UIInventoryCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public event Action< UIInventoryCell > OnPointerEntered;
        public event Action< UIInventoryCell > OnPointerExited;
        public event Action< UIInventoryCell > OnPointerClicked;
        
        [ SerializeField ] private Image _icon;
        [ SerializeField ] private GameObject _equiped;
        [ SerializeField ] private TextMeshProUGUI _count;
        [ SerializeField ] private GameObject _counter;
        [ SerializeField ] private GameObject _opened;
        [ SerializeField ] private GameObject _locked;

        public ItemModel Item { get; private set; }
        
        public void Set( ItemModel item )
        {
            Item = item;
            
            _icon.sprite = item.Config.Icon;
            _count.text = $"{item.Quantity}";
            
            _counter.SetActive( false );
            _equiped.SetActive( false );
        }

        public void SetLock( bool trigger )
        {
            _opened.SetActive( !trigger );
            _locked.SetActive( trigger );
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