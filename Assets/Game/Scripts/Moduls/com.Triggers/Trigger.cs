using System;
using System.Collections.Generic;
using UnityEngine;

namespace Moduls.Physics
{
    public sealed class Trigger : MonoBehaviour
    {
        public event Action< Collider > OnTriggerEntered;
        public event Action< Collider > OnTriggerExited;
        
        [ SerializeField ] private LayerMask _layerMask = -1;
        [ SerializeField ] private List< Collider > _exceptColliders = new();

        public void Enable( bool trigger )
        {
            gameObject.SetActive( trigger );
        }
        
        private void OnTriggerEnter( Collider other )
        {
            if ( !LayersUtils.Contains( _layerMask.value, other.gameObject.layer ) ) return;
            if ( _exceptColliders.Contains( other ) ) return;
            
            OnTriggerEntered?.Invoke( other );
        }

        private void OnTriggerExit( Collider other )
        {
            if ( !LayersUtils.Contains( _layerMask.value, other.gameObject.layer ) ) return;
            if ( _exceptColliders.Contains( other ) ) return;

            OnTriggerExited?.Invoke( other );
        }
    }
}