using Game.Core.Entity;
using UnityEditor;
using UnityEngine;

namespace Game.StoryFlow.Introduce.Editor
{
    [ CustomEditor( typeof( IntroduceLevelObject ) ) ]
    public sealed class IntroduceLevelObjectEditor : UnityEditor.Editor
    {
        private IntroduceLevelObject _target;
        
        private void OnEnable()
        {
            _target = (IntroduceLevelObject)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if ( GUILayout.Button( "Find All Entities" ) )
            {
                _target.Items.Clear();
                _target.Items.AddRange( FindObjectsOfType< ItemObject >( true ) );
                
                _target.Puzzles.Clear();
                _target.Puzzles.AddRange( FindObjectsOfType< PuzzleObject >( true ) );
                
                EditorUtility.SetDirty( _target );
            }
        }
    }
}