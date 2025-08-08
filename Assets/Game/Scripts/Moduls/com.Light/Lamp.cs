using UnityEngine;

namespace Moduls.Light
{
    public sealed class Lamp : MonoBehaviour
    {
        [ field: SerializeField ] public UnityEngine.Light Light { get; private set; }
        [ Space ]
        [ SerializeField ] private MeshRenderer _meshRenderer;
        [ SerializeField ] private Material _disabledMaterial;

        public bool IsEnable => Light.enabled;

        private Material _cachedMaterial;
        
        public void Enable( bool trigger )
        {
            Light.enabled = trigger;

            if ( _meshRenderer != null )
            {
                if ( _cachedMaterial == null )
                {
                    _cachedMaterial = _meshRenderer.sharedMaterial;
                }
                _meshRenderer.sharedMaterial = trigger ? _cachedMaterial : _disabledMaterial;
            }
        }
    }
}