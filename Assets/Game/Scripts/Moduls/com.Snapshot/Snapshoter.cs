using System.IO;
using UnityEngine;

namespace com.Snapshot
{
    public sealed class Snapshoter : MonoBehaviour
    {
        public Camera Camera => _camera;
        public Light Sun => _sun;
        public Transform Item => _item;
        public Vector2Int Resolution => _resolution;

        [ SerializeField ] private Camera _camera;
        [ SerializeField ] private Light _sun;
        [ SerializeField ] private Transform _item;
        [ SerializeField ] private Vector2Int _resolution;

        private bool _takeSnapshot = false;

        public void TakeSnapshot()
        {
            if ( _camera == null )
            {
                Debug.LogError( "[Snapshoter] Camera is not assigned" );
                return;
            }

            if ( _resolution.x <= 0 || _resolution.y <= 0 )
            {
                Debug.LogError( "[Snapshoter] Resolution is invalid" );
                return;
            }

            // Создаём временный RenderTexture нужного размера
            RenderTexture rt = RenderTexture.GetTemporary( _resolution.x, _resolution.y, 16, RenderTextureFormat.ARGB32 );
            rt.antiAliasing = 4;
            RenderTexture previousActive = RenderTexture.active;
            RenderTexture previousTarget = _camera.targetTexture;

            try
            {
                _camera.targetTexture = rt;
                _camera.Render();

                RenderTexture.active = rt;

                // Считываем пиксели
                Texture2D tex = new Texture2D( rt.width, rt.height, TextureFormat.ARGB32, false );
                tex.ReadPixels( new Rect( 0, 0, rt.width, rt.height ), 0, 0 );
                tex.Apply();

                byte[] bytes = tex.EncodeToPNG();

#if UNITY_EDITOR
                string dir = Application.dataPath + "/Screenshots";
#else
                string dir = Application.persistentDataPath + "/Screenshots";
#endif

                if ( !Directory.Exists( dir ) )
                    Directory.CreateDirectory( dir );

                string fileName = $"Item_{_resolution.x}x{_resolution.y}_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
                string fullPath = Path.Combine( dir, fileName );

                File.WriteAllBytes( fullPath, bytes );

#if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
#endif

                Debug.Log( $"[Snapshoter] Saved screenshot to: {fullPath}" );

#if UNITY_EDITOR
                Object.DestroyImmediate( tex );
#else
                Object.Destroy(tex);
#endif
            }
            finally
            {
                _camera.targetTexture = previousTarget;
                RenderTexture.active = previousActive;
                RenderTexture.ReleaseTemporary( rt );
            }
        }
    }
}