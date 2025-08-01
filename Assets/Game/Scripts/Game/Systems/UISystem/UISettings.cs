using PuzzlescapeGames.VVM;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UISystem
{
    [ System.Serializable ]
    public sealed class UISettings
    {
        [ field: SerializeField ] public List< View > Dialogs { get; private set; } = new();
    }
}