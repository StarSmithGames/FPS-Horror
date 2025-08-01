namespace Game.Core.Player
{
    public sealed class PlayerStates
    {
        public bool IsBlocked { get; set; } = false;
        
        public bool IsGrounded { get; set; }
        
        public bool IsJumping { get; set; }
        
        public bool IsCrouching { get; set; }
        
        public bool IsClimbing { get; set; }
        
        public bool IsSteppingStairs { get; set; }
    }
}