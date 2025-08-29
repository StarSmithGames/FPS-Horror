using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

namespace Game.Core.UI
{
    public sealed class UIOptionButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler, ISelectHandler, IDeselectHandler
    {
        public event Action< UIOptionButton > OnButtonPointerEntered;
        public event Action< UIOptionButton > OnButtonPointerExited;
        public event Action< UIOptionButton > OnButtonClicked;

        [ SerializeField ] private Button _button;
        [ SerializeField ] private Image _point;
        [ SerializeField ] private TextMeshProUGUI _text;
        [ SerializeField ] private Color _normal;
        [ SerializeField ] private Color _selected;
        
        public bool IsSelected { get; private set; }

        public void Select()
        {
            IsSelected = true;
            
            _text.color = _selected;
            _point.gameObject.SetActive( true );
        }

        public void Deselect()
        {
            IsSelected = false;
            
            _text.color = _normal;
            _point.gameObject.SetActive( false );
        }
        
        public void OnPointerEnter( PointerEventData eventData )
        {
            OnButtonPointerEntered?.Invoke( this );
        }

        public void OnPointerExit( PointerEventData eventData )
        {
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