using PuzzlescapeGames.VVM.UI;
using UnityEngine;

namespace Game.Core.UI.PauseScreen
{
    public sealed class UIPauseScreen : UIViewFade
    {
        [ field: SerializeField ] public UIOptionButton ContinueButton { get; private set; }
        [ field: SerializeField ] public UIOptionButton OptionsButton { get; private set; }
        [ field: SerializeField ] public UIOptionButton ExitButton { get; private set; }
    }
}