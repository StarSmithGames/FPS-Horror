using PuzzlescapeGames.VVM.UI;
using UnityEngine;

namespace Game.Core.UI.PauseScreen
{
    public sealed class UIPauseScreen : UIViewFade
    {
        [ field: SerializeField ] public UIOptionMenuButton ContinueButton { get; private set; }
        [ field: SerializeField ] public UIOptionMenuButton OptionsButton { get; private set; }
        [ field: SerializeField ] public UIOptionMenuButton ExitButton { get; private set; }
    }
}