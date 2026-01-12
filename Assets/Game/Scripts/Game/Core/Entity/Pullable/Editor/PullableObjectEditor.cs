using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Core.Entity.Editor
{
    [ CustomEditor( typeof(PullableObject) ) ]
    public sealed class PullableObjectEditor : UnityEditor.Editor
    {
        private PullableObject _target;
        private Transform _pull;
        private PullableSettings _settings;
        private void OnEnable()
        {
            _target = (PullableObject)target;
            _pull = (Transform)serializedObject.FindProperty( "_pull" ).objectReferenceValue;

            var type = typeof(PullableObject);
            _settings = (PullableSettings)type.GetField( "_settings", BindingFlags.Instance | BindingFlags.NonPublic )?.GetValue( target );
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
                Vector3 pos = _pull.localPosition;
                pos.z = _settings.OpenScalar;

                Undo.RecordObject( _pull, "OPEN" );
                _pull.localPosition = pos;
                EditorUtility.SetDirty( _pull );
            }
            if ( GUILayout.Button( "CLOSE" ) )
            {
                Vector3 pos = _pull.localPosition;
                pos.z = 0;

                Undo.RecordObject( _pull, "CLOSE" );
                _pull.localPosition = pos;
                EditorUtility.SetDirty( _pull );
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if ( GUILayout.Button( "PRE OPEN" ) )
            {
                Vector3 pos = _pull.localPosition;
                pos.z = Random.Range( _settings.OpenScalar * 0.75f, _settings.OpenScalar );

                Undo.RecordObject( _pull, "PRE OPEN" );
                _pull.localPosition = pos;
                EditorUtility.SetDirty( _pull );
            }
            if ( GUILayout.Button( "PRE CLOSE" ) )
            {
                Vector3 pos = _pull.localPosition;
                pos.z = Random.Range( 0f, _settings.OpenScalar * 0.25f );

                Undo.RecordObject( _pull, "PRE CLOSE" );
                _pull.localPosition = pos;
                EditorUtility.SetDirty( _pull );
            }
            GUILayout.EndHorizontal();

            if ( GUILayout.Button( "RANDOMIZE" ) )
            {
                Vector3 pos = _pull.localPosition;
                pos.z = Random.Range( 0f, _settings.OpenScalar );

                Undo.RecordObject( _pull, "RANDOMIZE" );
                _pull.localPosition = pos;
                EditorUtility.SetDirty( _pull );
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}