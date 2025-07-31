using UnityEngine;

namespace Game.Core
{
    public sealed class PositionBinding : MonoBehaviour
    {
        [ SerializeField ] private Transform _target;

        private void Update() => transform.position = _target.position;
    }
}