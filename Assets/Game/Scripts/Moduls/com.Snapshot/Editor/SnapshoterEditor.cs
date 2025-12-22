using UnityEditor;
using UnityEngine;

namespace com.Snapshot
{
    [ CustomEditor( typeof(Snapshoter) ) ]
    public sealed class SnapshoterEditor : Editor
    {
        private Snapshoter _snapshoter;
        private RenderTexture _previewRT;

        private Vector2 _dragStart;
        private bool _dragging;

        private void OnEnable()
        {
            _snapshoter = (Snapshoter)target;
        }

        private void OnDisable()
        {
            if (_previewRT != null)
            {
                _previewRT.Release();
                if ( _snapshoter != null )
                {
                    _snapshoter.Camera.targetTexture = null;
                }
                DestroyImmediate(_previewRT);
                _previewRT = null;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if ( _snapshoter == null || _snapshoter.Camera == null )
                return;

            if ( _snapshoter.Resolution.x <= 0 || _snapshoter.Resolution.y <= 0 )
                return;

            var res = _snapshoter.Resolution;

            // Пересоздание RT при изменении resolution
            if ( _previewRT == null || _previewRT.width != res.x || _previewRT.height != res.y )
            {
                if ( _previewRT != null )
                {
                    _previewRT.Release();
                    DestroyImmediate( _previewRT );
                }

                _previewRT = new RenderTexture( res.x, res.y, 16 ) { name = "Preview" };
            }

            // ――― РЕНДЕР КАМЕРЫ ―――
            Camera cam = _snapshoter.Camera;

            cam.targetTexture = _previewRT;
            cam.Render();

            GUILayout.Space( 10 );
            GUILayout.Label( $"Sun:{_snapshoter.Sun.transform.eulerAngles}", EditorStyles.boldLabel );
            GUILayout.Label( $"Rotation:{_snapshoter.Item.eulerAngles}", EditorStyles.boldLabel );
            GUILayout.Label( "Preview (Drag to rotate):", EditorStyles.boldLabel );

            float aspect = (float)res.y / res.x;
            float maxWidth = EditorGUIUtility.currentViewWidth - 40f;
            float width = Mathf.Min( res.x, maxWidth );
            float height = width * aspect;

            // ――― РАМКА ―――
            Rect previewRect;
            EditorGUILayout.BeginVertical( EditorStyles.helpBox );
            {
                previewRect = GUILayoutUtility.GetRect( width, height );
                EditorGUI.DrawPreviewTexture( previewRect, _previewRT, null, ScaleMode.ScaleToFit );
            }
            EditorGUILayout.EndVertical();

            HandleMouseEvents( previewRect );

            GUILayout.Space( 10 );

            if ( GUILayout.Button( "Take Snapshot" ) )
            {
                _snapshoter.TakeSnapshot();
            }
        }

        private void HandleMouseEvents( Rect rect )
        {
            Event e = Event.current;

            if ( _snapshoter.Item == null )
                return;

            // --- НАЧАЛО ДРАГА ---
            if ( e.type == EventType.MouseDown && rect.Contains( e.mousePosition ) )
            {
                _dragging = true;
                _dragStart = e.mousePosition;
                e.Use();
            }

            // --- ПРОЦЕСС ДРАГА ---
            if ( _dragging && e.type == EventType.MouseDrag )
            {
                Vector2 delta = e.mousePosition - _dragStart;
                _dragStart = e.mousePosition;

                float speed = e.shift ? 0.6f : 0.3f;

                // вращаем объект
                _snapshoter.Item.Rotate( Vector3.up, -delta.x * speed, Space.World );
                _snapshoter.Item.Rotate( Vector3.right, delta.y * speed, Space.World );

                e.Use();
                GUI.changed = true;
            }

            // --- КОНЕЦ ДРАГА ---
            if ( e.type == EventType.MouseUp )
            {
                _dragging = false;
            }
        }
    }
}