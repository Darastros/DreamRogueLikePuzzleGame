using System;
using Unity.Mathematics;
using UnityEngine;

namespace MovementControllers
{
    public class PlatformerMovementController : MonoBehaviour, IMovementController
    {
        [SerializeField] private Rigidbody2D m_rigidbody2D;
        [SerializeField] private CapsuleCollider2D m_collider2D;
        
        /// <summary>
        /// CONFIGURABLE FIELDS
        /// </summary>
        ///
        [Header("LAYERS")] [Tooltip("Set this to the layer your player is on")]
        public LayerMask PlayerLayer;

        [Header("INPUT")] [Tooltip("Makes all Input snap to an integer. Prevents gamepads from walking slowly. Recommended value is true to ensure gamepad/keybaord parity.")]
        public bool SnapInput = true;

        [Tooltip("Minimum input required before you mount a ladder or climb a ledge. Avoids unwanted climbing using controllers"), Range(0.01f, 0.99f)]
        public float VerticalDeadZoneThreshold = 0.3f;

        [Tooltip("Minimum input required before a left or right is recognized. Avoids drifting with sticky controllers"), Range(0.01f, 0.99f)]
        public float HorizontalDeadZoneThreshold = 0.1f;

        [Header("MOVEMENT")] [Tooltip("The top horizontal movement speed")]
        public float MaxSpeed = 14;

        [Tooltip("The player's capacity to gain horizontal speed")]
        public float Acceleration = 120;

        [Tooltip("The pace at which the player comes to a stop")]
        public float GroundDeceleration = 60;

        [Tooltip("Deceleration in air only after stopping input mid-air")]
        public float AirDeceleration = 30;

        [Tooltip("A constant downward force applied while grounded. Helps on slopes"), Range(0f, -10f)]
        public float GroundingForce = -1.5f;

        [Tooltip("The detection distance for grounding and roof detection"), Range(0f, 0.5f)]
        public float GrounderDistance = 0.05f;

        [Header("JUMP")]
        
        public AnimationCurve jumpDynamicMax;

        [Tooltip("The maximum vertical movement speed")]
        public float MaxFallSpeed = 40;

        [Tooltip("The player's capacity to gain fall speed. a.k.a. In Air Gravity")]
        public float FallAcceleration = 110;

        [Tooltip("The gravity multiplier added when jump is released early")]
        public float JumpEndEarlyGravityModifier = 3;

        [Tooltip("The time before coyote jump becomes unusable. Coyote jump allows jump to execute even after leaving a ledge")]
        public float CoyoteTime = .15f;

        [Tooltip("The amount of time we buffer a jump. This allows jump input before actually hitting the ground")]
        public float JumpBuffer = .2f;
        ///
        ///
        ///
        
        
        private Vector2 m_frameVelocity;
        private bool m_cachedQueryStartInColliders;
        private float m_frameLeftGrounded = float.MinValue;
        private bool m_grounded;
        private bool m_jumping;
        private bool m_coyoteUsable;
        private bool m_bufferedJumpUsable;
        private bool m_endedJumpEarly;
        private bool m_jumpToConsume;
        private bool m_jumpHold;
        private bool m_exitJump;
        private float m_timeJumpWasPressed;
        private float m_jumpTimer;
        private float m_jumpHoldTimer;
        private float m_jumpInitHeight;
        
        private Action<bool, float> GroundedChanged;
        private Action Jumped;
        
        private FrameInput m_frameInput = new FrameInput();
        private bool HasBufferedJump => m_bufferedJumpUsable && Time.time < m_timeJumpWasPressed + JumpBuffer;
        private bool CanUseCoyote => m_coyoteUsable && !m_grounded && Time.time < m_frameLeftGrounded + CoyoteTime;

#if UNITY_EDITOR
        private void OnValidate()
        {
            TryAcquireRequiredComponents();
        }
        #endif
        
        private void Awake()
        {
            TryAcquireRequiredComponents();
        }
        
        private void FixedUpdate()
        {
            CheckCollisions();

            HandleJump();
            HandleDirection();
            HandleVerticality();
            
            ApplyMovement();
        }
        
        private void ExecuteJump()
        {
            m_endedJumpEarly = false;
            m_timeJumpWasPressed = 0;
            m_bufferedJumpUsable = false;
            m_coyoteUsable = false;
            m_jumping = true;
            m_jumpTimer = 0.0f;
            m_jumpHoldTimer = 0.0f;
            m_jumpHold = true;
            m_exitJump = false;
            m_jumpInitHeight = transform.position.y;
            //m_frameVelocity.y = JumpPower;
            Jumped?.Invoke();
        }
        
        private void HandleJump()
        {
            if (!m_endedJumpEarly && !m_grounded && !m_frameInput.JumpHeld && m_rigidbody2D.velocity.y > 0) m_endedJumpEarly = true;

            if (!m_jumpToConsume && !HasBufferedJump) return;

            if (m_grounded || CanUseCoyote) ExecuteJump();

            m_jumpToConsume = false;
        }
        
        private void HandleDirection()
        {
            if (m_frameInput.Move.x == 0)
            {
                var deceleration = m_grounded ? GroundDeceleration : AirDeceleration;
                m_frameVelocity.x = Mathf.MoveTowards(m_frameVelocity.x, 0, deceleration * GameManager.fixedDeltaTime);
            }
            else
            {
                m_frameVelocity.x = Mathf.MoveTowards(m_frameVelocity.x, m_frameInput.Move.x * MaxSpeed, Acceleration * GameManager.fixedDeltaTime);
            }
        }
        
        private void HandleVerticality()
        {
            if (m_jumping)
            {
                m_jumpTimer += GameManager.deltaTime;
                m_jumpHold &= m_frameInput.JumpHeld;
                
                float jumpDuration = jumpDynamicMax.keys[^1].time;
                m_jumping = m_jumpTimer <= jumpDuration && (m_jumpHold || m_jumpTimer > 0.15f);
                if (m_jumping)
                {
                    float position = jumpDynamicMax.Evaluate(m_jumpTimer);
                    float speed = GameManager.deltaTime <= 0.0f ? 0.0f : (position - (transform.position.y - m_jumpInitHeight)) / GameManager.deltaTime;
                    m_frameVelocity.y = speed;
                    return;
                }
            }
            if (m_grounded && m_frameVelocity.y <= 0f)
            {
                m_frameVelocity.y = GroundingForce;
            }
            else
            {
                var inAirGravity = FallAcceleration;
                if (m_endedJumpEarly && m_frameVelocity.y > 0) inAirGravity *= JumpEndEarlyGravityModifier;
                m_frameVelocity.y = Mathf.MoveTowards(m_frameVelocity.y, -MaxFallSpeed, inAirGravity * GameManager.fixedDeltaTime);
            }
        }
        
        private void ApplyMovement() => m_rigidbody2D.velocity = m_frameVelocity * GameManager.timeFactor;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            Bounds bounds = m_collider2D.bounds ;
            Vector2 size = m_collider2D.size;
            CapsuleDirection2D direction = m_collider2D.direction;
            
            RaycastHit2D[] hitRes = new RaycastHit2D[3];
            var rayGround = m_collider2D.Cast(Vector2.down, hitRes, GrounderDistance);
            bool groundHit = rayGround > 0;
            
            var rayUp =  m_collider2D.Cast(Vector2.up, hitRes, GrounderDistance);
            bool ceilingHit = rayUp > 0;
           
            // Hit a Ceiling
            if (ceilingHit) m_frameVelocity.y = Mathf.Min(0, m_frameVelocity.y);

            // Landed on the Ground
            if (!m_grounded && groundHit && (!m_jumping || m_jumpTimer > 0.1f))
            {
                m_grounded = true;
                m_coyoteUsable = true;
                m_bufferedJumpUsable = true;
                m_jumping = false;
                m_endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(m_frameVelocity.y));
            }
            // Left the Ground
            else if (m_grounded && !groundHit)
            {
                m_grounded = false;
                m_frameLeftGrounded = Time.time;
                GroundedChanged?.Invoke(false, 0);
            }

            Physics2D.queriesStartInColliders = m_cachedQueryStartInColliders;
        }
        
        private void TryAcquireRequiredComponents()
        {
            if(m_rigidbody2D == null)
                TryGetComponent(out m_rigidbody2D);

            if(m_collider2D == null)
                TryGetComponent(out m_rigidbody2D);
        }

        public void Move(Vector2 _wantedDirection)
        {
            m_frameInput.Move = _wantedDirection;
            
            if (SnapInput)
            {
                m_frameInput.Move.x = Mathf.Abs(m_frameInput.Move.x) < HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(m_frameInput.Move.x);
                m_frameInput.Move.y = Mathf.Abs(m_frameInput.Move.y) < VerticalDeadZoneThreshold ? 0 : Mathf.Sign(m_frameInput.Move.y);
            }
        }

        public void Jump(bool _buttonPressed)
        {
            m_frameInput.JumpHeld = _buttonPressed;
            if (_buttonPressed)
            {
                m_frameInput.LastJumpDownFrameStamp = Time.frameCount;
                m_jumpToConsume = true;
                m_timeJumpWasPressed = Time.time;
            }
            else
            {
                m_frameInput.LastJumpDownFrameStamp = int.MinValue;
            }
        }
    }
    public struct FrameInput
    {
        public bool JumpHeld;
        public Vector2 Move;
        public int LastJumpDownFrameStamp;
        public bool JumpDown => LastJumpDownFrameStamp == Time.frameCount;
    }
}