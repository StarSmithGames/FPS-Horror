using UnityEngine;

namespace Game.Core.UI
{
    public sealed class UIDynamicScreen : MonoBehaviour
    {
        [ field: SerializeField ] public Transform DialogsRoot { get; private set; }
    }
}