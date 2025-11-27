using UnityEngine;

namespace Game.Managers.InputManager
{
    [ System.Serializable ]
    public sealed class InputKeyActionsSettings
    {
        [ field: Header( "OBSERVE" ) ]
        [ field: SerializeField ] public InputKeyAction InteractAction { get; private set; }
        [ field: SerializeField ] public InputKeyAction InspectAction { get; private set; }
        [ field: Header( "QUICK ACCESS" ) ]
        [ field: SerializeField ] public InputKeyAction LighterAction { get; private set; }
        [ field: Header( "INSPECT" ) ]
        [ field: SerializeField ] public InputKeyAction ReadAction { get; private set; }
        [ field: SerializeField ] public InputKeyAction BackAction { get; private set; }
        [ field: Header( "CONTROLS" ) ]
        [ field: SerializeField ] public Sprite GamepadMovementTop { get; private set; }
        [ field: SerializeField ] public Sprite GamepadMovementBottom { get; private set; }
        [ field: SerializeField ] public Sprite GamepadMovementLeft { get; private set; }
        [ field: SerializeField ] public Sprite GamepadMovementRight { get; private set; }
        [ field: SerializeField ] public Sprite GamepadSprint { get; private set; }
        [ field: SerializeField ] public Sprite GamepadCrouch { get; private set; }
        [ field: SerializeField ] public Sprite GamepadInteract { get; private set; }
    }
}