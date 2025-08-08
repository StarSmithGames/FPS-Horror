using UnityEditor;
using UnityEngine;

namespace Game.Core.Environment.Editor
{
    [ CustomEditor( typeof( RoomObject ) ) ]
    public sealed class RoomObjectEditor : UnityEditor.Editor
    {
        private RoomObject _target;

        private void OnEnable()
        {
            _target = (RoomObject)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if ( GUILayout.Button( _target.IsCeilLampsEnabled ? "DISABLE LIGHT" : "ENABLE LIGHT" ) )
            {
                _target.EnableCeilLamps( !_target.IsCeilLampsEnabled );
                
                EditorUtility.SetDirty( _target );
            }
        }
    }
}