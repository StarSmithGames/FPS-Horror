using PuzzlescapeGames.VVM.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.UI
{
    public sealed class UIGameScreen : UIViewFade
    {
        [ field: SerializeField ] public Image TargetPoint { get; private set; }
    }
}