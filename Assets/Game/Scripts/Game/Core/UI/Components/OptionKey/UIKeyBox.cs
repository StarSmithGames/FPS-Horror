using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Core.UI
{
    public sealed class UIKeyBox : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler, ISelectHandler, IDeselectHandler
    {
        public event Action< UIKeyBox > OnButtonClicked;

        [ SerializeField ] private Image _back;
        [ SerializeField ] private TextMeshProUGUI _key;
        [ SerializeField ] private Image _icon;
        
        public bool IsBlocked { get; private set; }
        public bool IsSelected { get; private set; }

        public void Block( bool trigger )
        {
            IsBlocked = trigger;

            if ( IsBlocked )
            {
                _back.color = new( 0.5f, 0.5f, 0.5f, 1f );
            }
            else
            {
                _back.color = new( 0, 0, 0, 1f );
            }
        }
        
        public void SetType( bool isIcon )
        {
            _key.gameObject.SetActive( !isIcon );
            _icon.gameObject.SetActive( isIcon );
        }
        
        public void SetKey( string key )
        {
            _key.text = key;
        }

        public void SetIcon( Sprite icon )
        {
            _icon.sprite = icon;
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
            if ( IsBlocked )
            {
                _back.color = new( 0.5f, 0.5f, 0.5f, 0.7f );
            }
            else
            {
                _back.color = new( 0, 0, 0, 0.7f );
            }
        }

        public void OnPointerExit( PointerEventData eventData )
        {
            if ( IsBlocked )
            {
                _back.color = new( 0.5f, 0.5f, 0.5f, 1f );
            }
            else
            {
                _back.color = new( 0, 0, 0, 1f );
            }
        }

        public void OnPointerClick( PointerEventData eventData )
        {
            if ( IsBlocked ) return;
            
            OnButtonClicked?.Invoke( this );
        }

        public void OnSelect( BaseEventData eventData )
        {
        }

        public void OnDeselect( BaseEventData eventData )
        {
        }

        public void OnSubmit( BaseEventData eventData )
        {
            if ( IsBlocked ) return;
            
            OnButtonClicked?.Invoke( this );
        }
    }
}