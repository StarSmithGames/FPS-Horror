using PuzzlescapeGames.VVM.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.UI.GameScreen
{
    public sealed class UITargetInformer : UIViewFade
    {
        [ field: SerializeField ] public TMPro.TextMeshProUGUI Name { get; private set; }
        [ field: SerializeField ] public List< UIInfoButton > Options { get; private set; } = new();
    }
}