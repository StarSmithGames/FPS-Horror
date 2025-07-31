using UnityEngine;

namespace Game.Core.Player
{
    [ CreateAssetMenu( fileName = "PlayerConfig", menuName = "Game/PlayerConfig" ) ]
    public sealed class PlayerConfig : ScriptableObject
    {
        [ field: SerializeField ] public LookSettings LookSettings { get; private set; }
        
        [ field: Header( "Ground" ) ]
        [ field: Tooltip( "Every object with this layer will be detected as ground, so you will be able to walk on it" ) ]
        [ field: SerializeField ] public LayerMask GroundLayer { get; private set; }

        [ field: Tooltip( "Distance from the bottom of the player to detect ground" ) ]
        [ field: Min( 0 ) ]
        [ field: SerializeField ] public float GroundCheckDistance { get; private set; } = 1.2f;

        [ field: Header( "Sliding" ) ]
        [ field: Tooltip( "When true, player will be allowed to slide." ) ]
        [ field: SerializeField ] public bool AllowSliding { get; private set; }
        [Tooltip("Force added on sliding."), SerializeField]
        private float slideForce = 400;
        [ field: Tooltip( "If true, the player will be able to move while sliding." ) ]
        [ field: SerializeField ] public bool AllowMoveWhileSliding { get; private set; }
        [Range(0, 1f), SerializeField, Tooltip("Force applied to counter movement when sliding")] private float slideFrictionForceAmount;
        
        [ field: Header( "Movement") ]
        [ field: Min( 0.01f ) ]
        [ field: Tooltip( "Max speed the player can reach. Velocity is clamped by this value." ) ]
        [ field: SerializeField ] public float MaxSpeedAllowed { get; private set; } = 20f;
        [ field: Min( 0.01f ) ]
        [ field: SerializeField ] public float RunSpeed { get; private set; } = 10f;
        [ field: Min( 0.01f ) ]
        [ field: SerializeField ] public float WalkSpeed { get; private set; } = 5f;
        [ field: Min( 0.01f ) ]
        [ field: SerializeField ] public float CrouchSpeed { get; private set; } = 3f;
        [ field: Tooltip( "Capacity to gain speed." ) ]
        [ field: SerializeField ] public float Acceleration { get; private set; } = 4500;
        
        
        [ field: Tooltip("Maximum slope angle that you can walk through.") ]
        [ field: Range(10, 80) ]
        [ field: SerializeField ] public float MaxSlopeAngle { get; private set; } = 35f;


        [ field: Header( "Jump" ) ]
        [ field: Tooltip( "Enable this if your player can jump." ) ]
        [ field: SerializeField ] public bool AllowJump { get; private set; }

        [ field: Tooltip( "Amount of jumps you can do without touching the ground" ) ]
        [ field: Min( 1 ) ]
        [ field: SerializeField ] public int MaxJumps { get; private set; }

        [Tooltip("Gains jump amounts when wallrunning.")] public bool resetJumpsOnWallrun;

        [Tooltip("Gains jump amounts when wallrunning.")] public bool resetJumpsOnWallBounce;

        [Tooltip("Gains jump amounts when grapple starts.")] public bool resetJumpsOnGrapple;

        [Tooltip("Double jump will reset fall damage, only if your player controller is optable to take fall damage")] public bool doubleJumpResetsFallDamage;

        // [Tooltip("Method to apply on jumping when the player is not grounded, related to the directional jump")]
        // public DirectionalJumpMethod directionalJumpMethod;

        [Tooltip("Force applied on an object in the direction of the directional jump"), SerializeField]
        private float directionalJumpForce;

        [Tooltip("The higher this value is, the higher you will get to jump."), SerializeField]
        private float jumpForce = 550f;

        [ field: Tooltip( "How much control you own while you are not grounded. Being 0 = no control of it, 1 = Full control." ) ]
        [ field: Range( 0, 1 ) ]
        [ field: SerializeField ] public float ControlAirborne { get; private set; } = .5f;

        [Tooltip("Turn this on to allow the player to crouch while jumping")]
        public bool allowCrouchWhileJumping;

        [Tooltip(" Allow the player to jump mid-air")]
        public bool canJumpWhileCrouching;

        [Tooltip("Interval between jumping")][Min(.25f), SerializeField] private float jumpCooldown = .25f;

        [Range(0, .3f), Tooltip("Coyote jump allows users to perform more satisfactory and responsive jumps, especially when jumping off surfaces")] public float coyoteJumpTime;
    }
}