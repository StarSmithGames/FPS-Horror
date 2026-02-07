using Game.Core.World.LibrarySystem;
using TMPro;
using UnityEngine;

namespace Game.Core.UI.ResourcesScreen
{
    public sealed class UILibraryOption : UIOption
    {
        [ SerializeField ] private TextMeshProUGUI _text;
        [ SerializeField ] private GameObject _separator;
        [ SerializeField ] private GameObject _selected;

        public LibraryItem Item { get; private set; }
        
        public void Set( LibraryItem item )
        {
            Item = item;
        }

        public void SetText( string text )
        {
            _text.text = text;
        }
        
        public override void Select()
        {
            base.Select();
            _separator.SetActive( false );
            _selected.SetActive( true );
        }

        public override void Deselect()
        {
            base.Deselect();
            _separator.SetActive( true );
            _selected.SetActive( false );
        }
    }
}