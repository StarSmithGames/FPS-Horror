using PuzzlescapeGames.VVM.UI;
using UnityEngine;

namespace Game.Core.UI.MenuScreen
{
    public sealed class UIMenuScreen : UIViewFade
    {
        [ field: SerializeField ] public UIOptionButton StartButton { get; private set; }
        [ field: SerializeField ] public UIOptionButton OptionsButton { get; private set; }
        [ field: SerializeField ] public UIOptionButton ExitButton { get; private set; }
    }
}