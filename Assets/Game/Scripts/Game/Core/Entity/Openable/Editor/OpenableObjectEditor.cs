using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Game.Core.Entity.Editor
{
    [ CustomEditor( typeof(OpenableObject) ) ]
    public sealed class OpenableObjectEditor : UnityEditor.Editor
    {
        private OpenableObject _target;
        private Transform _door;
        private OpenableSettings _settings;
        private void OnEnable()
        {
            _target = (OpenableObject)target;
            _door = (Transform)serializedObject.FindProperty( "_door" ).objectReferenceValue;

            var type = typeof(OpenableObject);
            _settings = (OpenableSettings)type.GetField( "_settings", BindingFlags.Instance | BindingFlags.NonPublic )?.GetValue( target );
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            GUILayout.Space( 10 );
            if ( _target.Colliders != null && _target.Colliders.Count > 0 )
            {
                bool isEnabled = _target.Colliders.All( ( x ) => x.enabled );
                if ( GUILayout.Button( isEnabled ? "DISABLE" : "ENABLE" ) )
                {
                    _target.EnableCollider( !isEnabled );

                    EditorUtility.SetDirty( _target );
                }
            }

            GUILayout.Space( 10 );
            GUILayout.BeginHorizontal();
            if ( GUILayout.Button( "OPEN" ) )
            {
                Undo.RecordObject( _target, "OPEN" );
                _target.SetToOpen();
                EditorUtility.SetDirty( _target );
            }

            if ( GUILayout.Button( "CLOSE" ) )
            {
                Undo.RecordObject( _target, "CLOSE" );
                _target.SetToClose();
                EditorUtility.SetDirty( _target );
            }
            GUILayout.EndHorizontal();
            
            // GUILayout.BeginHorizontal();
            // if ( GUILayout.Button( "PRE OPEN" ) )
            // {
            //     Vector3 pos = _pull.localPosition;
            //     pos.z = Random.Range( _settings.OpenScalar * 0.75f, _settings.OpenScalar );
            //
            //     Undo.RecordObject( _pull, "PRE OPEN" );
            //     _pull.localPosition = pos;
            //     EditorUtility.SetDirty( _pull );
            // }
            //
            // if ( GUILayout.Button( "PRE CLOSE" ) )
            // {
            //     Vector3 pos = _pull.localPosition;
            //     pos.z = Random.Range( 0f, _settings.OpenScalar * 0.25f );
            //
            //     Undo.RecordObject( _pull, "PRE CLOSE" );
            //     _pull.localPosition = pos;
            //     EditorUtility.SetDirty( _pull );
            // }
            // GUILayout.EndHorizontal();
        }
    }
}