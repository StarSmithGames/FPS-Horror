using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Core.UI
{
    public sealed class UIKeyBox : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler, ISelectHandler, IDeselectHandler
    {
        public event Action< UIKeyBox > OnButtonPointerEntered;
        public event Action< UIKeyBox > OnButtonPointerExited;
        public event Action< UIKeyBox > OnButtonClicked;

        [ SerializeField ] private Image _back;
        [ SerializeField ] private TextMeshProUGUI _key;
        
        public bool IsSelected { get; private set; }
        
        public void SetName( string key )
        {
            _key.text = key;
        }

        public void SetNameColor( Color color )
        {
            _key.color = color;
        }
        
        public void Select()
        {
            IsSelected = true;
        }

        public void Deselect()
        {
            IsSelected = false;
        }
        
        public void OnPointerEnter( PointerEventData eventData )
        {
            _back.color = new( 0, 0, 0, 0.7f );
            
            OnButtonPointerEntered?.Invoke( this );
        }

        public void OnPointerExit( PointerEventData eventData )
        {
            _back.color = new( 0, 0, 0, 1f );
            
            OnButtonPointerExited?.Invoke( this );
        }

        public void OnPointerClick( PointerEventData eventData )
        {
            OnButtonClicked?.Invoke( this );
        }

        public void OnSelect( BaseEventData eventData )
        {
            OnButtonPointerEntered?.Invoke( this );
        }

        public void OnDeselect( BaseEventData eventData )
        {
            OnButtonPointerExited?.Invoke( this );
        }

        public void OnSubmit( BaseEventData eventData )
        {
            OnButtonClicked?.Invoke( this );
        }
    }
}