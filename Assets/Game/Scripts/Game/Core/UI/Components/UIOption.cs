using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Core.UI
{
    public class UIOption : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler, ISelectHandler, IDeselectHandler
    {
        public event Action< UIOption > OnPointerEntered;
        public event Action< UIOption > OnPointerExited;
        public event Action< UIOption > OnButtonClicked;
        
        [ SerializeField ] private TextMeshProUGUI _name;
        
        public bool IsSelected { get; private set; }
        
        public void SetName( string name )
        {
            _name.text = name;
        }

        public void SetNameColor( Color color )
        {
            _name.color = color;
        }
        
        public virtual void Select()
        {
            IsSelected = true;
        }

        public virtual void Deselect()
        {
            IsSelected = false;
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
            OnButtonClicked?.Invoke( this );
        }

        public void OnSelect( BaseEventData eventData )
        {
            OnPointerEntered?.Invoke( this );
        }

        public void OnDeselect( BaseEventData eventData )
        {
            OnPointerExited?.Invoke( this );
        }

        public void OnSubmit( BaseEventData eventData )
        {
            OnButtonClicked?.Invoke( this );
        }
    }
}