namespace Game.Core.Player
{
    public sealed class PlayerStates
    {
        public bool IsGrounded { get; set; }
        
        public bool IsCrouching { get; set; }
        
        public bool IsClimbing { get; set; }
        
        public bool IsSteppingStairs { get; set; }
    }
}