using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

namespace Game.Core.Entity
{
    public sealed class PullableObject : OpenCloseDynamicObject
    {
        [ SerializeField ] private Transform _pull;
        [ SerializeField ] private PullableSettings _settings;
        
        private Vector3 _closedPosition;
        private Vector3 _openPosition;
        
        protected override void Awake()
        {
            base.Awake();
            
            _closedPosition = Vector3.zero;
            _openPosition = new Vector3( 0, 0, _settings.OpenScalar );
            
            if ( _settings.IsLocked )
            {
                IsOpen = false;
                _pull.localPosition = Vector3.zero;
            }
            else
            {
                float distance = Vector3.Distance( _pull.localPosition, _openPosition );
                IsOpen = distance < _settings.OpenScalar / 2f;
            }
        }
        
        public override void Open()
        {
            OpenAsync().Forget();
        }

        public override void Close()
        {
            CloseAsync().Forget();
        }
        
        private async UniTask OpenAsync()
        {
            if ( IsOpen ) return;
            IsOpen = true;

            await AnimateAsync( true );
        }

        private async UniTask CloseAsync()
        {
            if ( !IsOpen ) return;
            IsOpen = false;

            await AnimateAsync( false );
        }

        private async UniTask AnimateAsync( bool opening )
        {
            Vector3 from = opening ? _pull.localPosition : _openPosition;
            Vector3 to = opening ? _openPosition : _closedPosition;

            float range = Vector3.Distance( from, to );
            float fraction = Mathf.Clamp01( range / _settings.OpenScalar );

            await LerpPosition( _pull, from, to, _settings.Duration * fraction );
        }

        private async UniTask LerpPosition( Transform target, Vector3 from, Vector3 to, float duration )
        {
            float elapsed = 0f;
            while ( elapsed < duration )
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01( elapsed / duration );
                target.localPosition = Vector3.Lerp( from, to, t );

                await UniTask.Yield();
            }

            target.localPosition = to;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            bool isEnabled = _colliders.All( ( x ) => x.enabled );

            Color color = isEnabled ? Color.green : Color.red;
            color.a = 0.33f;
            Gizmos.color = color;

            foreach ( var col in _colliders )
            {
                Gizmos.DrawWireCube( col.bounds.center, col.bounds.size );
            }
        }
#endif
    }
}