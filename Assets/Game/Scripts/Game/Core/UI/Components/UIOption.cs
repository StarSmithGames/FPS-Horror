using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Core.UI
{
    public class UIOption : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler, ISelectHandler, IDeselectHandler
    {
        public event Action< UIOption > OnPointerEntered;
        public event Action< UIOption > OnPointerExited;
        public event Action< UIOption > OnButtonClicked;
        
        public bool IsSelected { get; private set; }
        
        public virtual void Select()
        {
            IsSelected = true;
        }

        public virtual void Deselect()
        {
            IsSelected = false;
        }

        public virtual void OnPointerEnter( PointerEventData eventData )
        {
            OnPointerEntered?.Invoke( this );
        }

        public virtual void OnPointerExit( PointerEventData eventData )
        {
            OnPointerExited?.Invoke( this );
        }

        public virtual void OnPointerClick( PointerEventData eventData )
        {
            OnButtonClicked?.Invoke( this );
        }

        public virtual void OnSelect( BaseEventData eventData )
        {
            OnPointerEntered?.Invoke( this );
        }

        public virtual void OnDeselect( BaseEventData eventData )
        {
            OnPointerExited?.Invoke( this );
        }

        public virtual void OnSubmit( BaseEventData eventData )
        {
            OnButtonClicked?.Invoke( this );
        }
    }
}