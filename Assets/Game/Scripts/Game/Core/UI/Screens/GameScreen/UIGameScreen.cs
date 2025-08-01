using PuzzlescapeGames.VVM.UI;
using UnityEngine;

namespace Game.Core.UI
{
    public sealed class UIGameScreen : UIViewFade
    {
        [ field: SerializeField ] public UITargetPoint TargetPoint { get; private set; }
        [ field: SerializeField ] public UITargetInformer TargetInformer { get; private set; }
    }
}