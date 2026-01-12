using PuzzlescapeGames.VVM.UI;
using UnityEngine;

namespace Game.Core.UI.PauseScreen
{
    public sealed class UIPauseScreen : UIViewFade
    {
        [ field: SerializeField ] public UIOptionMenuTextButton ContinueButton { get; private set; }
        [ field: SerializeField ] public UIOptionMenuTextButton OptionsButton { get; private set; }
        [ field: SerializeField ] public UIOptionMenuTextButton ExitButton { get; private set; }
    }
}