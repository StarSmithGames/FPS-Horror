using UnityEngine;

namespace Game.Core.Environment
{
    public sealed class FanObject : MonoBehaviour
    {
        [ SerializeField ] private Transform _head;
        [ SerializeField ] private Transform _blades;
        [ Space ]
        [ SerializeField ] private float _headSpeed = 11;
        [ SerializeField ] private float _bladesSpeed = 55f;
        
        private void Update()
        {
            _blades.Rotate( Vector3.forward * -_bladesSpeed * Time.deltaTime * 10 );

            float angle = Mathf.Sin( Time.time * ( Mathf.PI * 2 / _headSpeed ) ) * 45f;
            _head.localRotation = Quaternion.Euler( 0, angle, 0 );
        }
    }
}