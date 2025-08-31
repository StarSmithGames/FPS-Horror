using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Core.World.SpawnSystem
{
    public sealed class SpawnPlayerPoint : MonoBehaviour
    {
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 pos = transform.position;

            // Точка спавна
            Gizmos.color = Color.green;
            Gizmos.DrawSphere( pos, 0.1f );

            // Стрелка направления
            Gizmos.color = Color.blue;
            Vector3 forward = transform.forward;
            float shaftLen = 0.5f;
            float headLen = 0.2f;
            float headAngle = 25f;

            // Стержень
            Vector3 tip = pos + forward * shaftLen;
            Gizmos.DrawLine( pos, tip );

            // Наконечник (два луча под углом)
            Vector3 rightDir = Quaternion.AngleAxis( headAngle, transform.up ) * -forward;
            Vector3 leftDir = Quaternion.AngleAxis( -headAngle, transform.up ) * -forward;
            Vector3 upDir = Quaternion.AngleAxis( headAngle, transform.right ) * -forward;
            Vector3 downDir = Quaternion.AngleAxis( -headAngle, transform.right ) * -forward;

            Gizmos.DrawLine( tip, tip + rightDir * headLen );
            Gizmos.DrawLine( tip, tip + leftDir * headLen );
            Gizmos.DrawLine( tip, tip + upDir * headLen );
            Gizmos.DrawLine( tip, tip + downDir * headLen );

            // Подпись
            Handles.Label( pos + Vector3.up * 0.2f, "Player" );
        }
#endif
    }
}