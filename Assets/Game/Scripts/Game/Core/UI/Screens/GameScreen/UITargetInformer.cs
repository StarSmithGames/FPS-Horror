using PuzzlescapeGames.VVM.UI;
using UnityEngine;

namespace Game.Core.UI.GameScreen
{
    public sealed class UITargetInformer : UIViewFade
    {
        [ field: SerializeField ] public TMPro.TextMeshProUGUI Name { get; private set; }
        [ field: SerializeField ] public UIInfoButton Button1 { get; private set; }
        [ field: SerializeField ] public UIInfoButton Button2 { get; private set; }
    }
}