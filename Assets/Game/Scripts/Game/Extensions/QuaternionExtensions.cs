using UnityEngine;

namespace Game.Extensions
{
    public static class QuaternionExtensions
    {
        public static float Yaw( this Quaternion rotation ) => rotation.eulerAngles.y;
        public static Vector3 Forward( this Quaternion rotation ) => rotation * Vector3.forward;
        public static Vector3 Right( this Quaternion rotation ) => rotation * Vector3.right;
        public static Vector3 Up( this Quaternion rotation ) => rotation * Vector3.up;
    }
}