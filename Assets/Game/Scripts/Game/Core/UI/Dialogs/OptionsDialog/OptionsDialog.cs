using PuzzlescapeGames.VVM.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class OptionsDialog : UIViewFade
    {
        [ field: SerializeField ] public TabsSettings TabsSettings { get; private set; }
        [ field: SerializeField ] public List< UITab > Tabs { get; private set; } = new();
        [ field: SerializeField ] public List< Transform > TabsRoots { get; private set; }
        [ field: SerializeField ] public List< Transform > Contents { get; private set; }
        [ SerializeField ] private ScrollRect _scrollRect;
        [ Space ]
        [ SerializeField ] private List< UIGamepadTip > _gamepadTips = new();
        [ SerializeField ] private UIGamepadTip _gamepadTip;
        [ field: Space ]
        [ field: SerializeField ] public UIEnterKeyModalView EnterKey { get; private set; }

        public void SetScrollRectContent( RectTransform content )
        {
            _scrollRect.content = content;
        }
        
        public void SetTips( int type )
        {
            for ( int i = 0; i < _gamepadTips.Count; i++ )
            {
                _gamepadTips[ i ].SetType( type );
            }
        }

        public void EnableControlTip( bool trigger )
        {
            _gamepadTip.gameObject.SetActive( trigger );
        }

        public void SetControlTip( int type )
        {
            _gamepadTip.SetType( type );
        }
    }
}