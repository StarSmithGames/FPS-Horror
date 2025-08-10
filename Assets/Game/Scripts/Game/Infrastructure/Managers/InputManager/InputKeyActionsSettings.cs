using UnityEngine;

namespace Game.Managers.InputManager
{
    [ System.Serializable ]
    public sealed class InputKeyActionsSettings
    {
        [ field: Header( "OBSERVE" ) ]
        [ field: SerializeField ] public InputKeyAction InteractAction { get; private set; }
        [ field: SerializeField ] public InputKeyAction InspectAction { get; private set; }
        [ field: SerializeField ] public InputKeyAction OpenCloseAction { get; private set; }
        [ field: Header( "QUICK ACCESS" ) ]
        [ field: SerializeField ] public InputKeyAction LighterAction { get; private set; }
        [ field: Header( "INSPECT" ) ]
        [ field: SerializeField ] public InputKeyAction ReadAction { get; private set; }
        [ field: SerializeField ] public InputKeyAction BackAction { get; private set; }
    }
}