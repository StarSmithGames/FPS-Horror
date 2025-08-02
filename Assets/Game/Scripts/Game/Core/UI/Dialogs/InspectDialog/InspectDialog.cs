using PuzzlescapeGames.VVM.UI;
using UnityEngine;

namespace Game.Core.UI.InspectDialog
{
    public sealed class InspectDialog : UIViewFade
    {
        [ field: SerializeField ] public TMPro.TextMeshProUGUI ExamineName { get; private set; }
        [ field: SerializeField ] public GameObject ControlButtons { get; private set; } 
        [ field: SerializeField ] public UIInfoButton ReadButton { get; private set; }
        [ field: SerializeField ] public UIInfoButton CancelButton1 { get; private set; }
        [ field: SerializeField ] public UIInfoButton CancelButton2 { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public CanvasGroup ExamineCanvasGroup { get; private set; }
        [ field: SerializeField ] public TMPro.TextMeshProUGUI ExamineText { get; private set; }
    }
}