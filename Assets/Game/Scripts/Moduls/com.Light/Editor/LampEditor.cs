using UnityEditor;
using UnityEngine;

namespace Moduls.Light.Editor
{
    [ CustomEditor( typeof( Lamp ) ) ]
    public sealed class LampEditor : UnityEditor.Editor
    {
        private Lamp _target;

        private void OnEnable()
        {
            _target = (Lamp)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if ( GUILayout.Button( _target.IsEnable ? "DISABLE" : "ENABLE" ) )
            {
                _target.Enable( !_target.IsEnable );
                
                EditorUtility.SetDirty( _target );
            }
        }
    }
}