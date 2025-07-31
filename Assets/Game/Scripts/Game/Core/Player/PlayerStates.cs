using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerStates
    {
        public bool IsGrounded { get; set; }
        
        public bool IsCrouching { get; set; }
        
        public bool IsClimbing { get; set; }
        
        public bool IsSteppingStairs { get; set; }
        
        
        public float Yaw => Rotation.eulerAngles.y;
        public Vector3 Forward => Rotation * Vector3.forward;
        public Vector3 Right => Rotation * Vector3.right;
        public Vector3 Up => Rotation * Vector3.up;
        
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
    }
}