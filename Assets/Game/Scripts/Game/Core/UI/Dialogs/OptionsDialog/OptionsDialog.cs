using PuzzlescapeGames.VVM.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class OptionsDialog : UIViewFade
    {
        [ field: SerializeField ] public List< UITab > Tabs { get; private set; } = new();
        [ field: SerializeField ] public List< Transform > TabsRoots { get; private set; }
        [ field: SerializeField ] public List< Transform > Contents { get; private set; }
        [ field: SerializeField ] public ScrollRect ScrollRect { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public TabsSettings TabsSettings { get; private set; }
        [ Space ]
        [ SerializeField ] private List< UIGamepadTip > _gamepadTips = new();

        public void SetTips( int type )
        {
            for ( int i = 0; i < _gamepadTips.Count; i++ )
            {
                _gamepadTips[ i ].SetType( type );
            }
        }
    }
}