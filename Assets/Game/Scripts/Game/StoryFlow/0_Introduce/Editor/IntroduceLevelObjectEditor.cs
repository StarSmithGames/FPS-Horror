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
        }
    }
}