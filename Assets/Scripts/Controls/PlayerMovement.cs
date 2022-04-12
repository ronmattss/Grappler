using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    public class PlayerMovement : MonoBehaviour
    {
    
        // Player Based Movement Logic will be here
        private Rigidbody2D m_Rigidbody2D;
        private DistanceJoint2D m_DistanceJoint2D;
        public float acceleration = 3f;
        public float deceleration = 2f;

        public float airSpeed = 2f;
        public float airDrag = 1f;

        public float topSpeed = 20f;
        public float airTopSpeed = 15f;

        public float jumpForce = 10f;
        public float gravity = -9.81f;
        public float groundDistance = 1f;
        public float wallJumpLerp = 10;
        public float dashSpeed = 20;
        public float slideSpeed = 5;
        public LayerMask groundMask;
        public Transform groundCheck;
        
        public bool isGrounded;
        public bool isJumping;
        public bool isFalling;
        public bool isCrouching;
        public bool isWalking;
        public bool isDashing;
        public bool canMove;
        public bool canJump;
        public bool wallGrab;
        public bool wallJumped;
        
        //Movement Context

        private Vector3 m_CurrentMovement;
        private Vector3 m_CurrentAirMovement;
        bool m_MovementInputIsActive;
        bool m_IsMoving;
        
        //Jump Context
        public float fallMultiplier = 2.5f;
        public float lowJumpMultiplier = 2f;
        

        void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_DistanceJoint2D = GetComponent<DistanceJoint2D>();

        }



        bool CheckGrounded()
        {
            RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundDistance, groundMask);
            if (hit.collider)
            {
                this.gameObject.transform.parent = hit.transform;
            }
            else
            {
                this.gameObject.transform.parent = null;

            }
           return Physics2D.CircleCast(groundCheck.position, groundDistance, Vector2.down, groundDistance, groundMask);
        }
        
        /// <summary>
        /// Move player depending on movement input
        /// </summary>
        /// <param name="dir"></param>

        void Walk(Vector2 dir)
        {
        

            if (!canMove) return;
            if (isGrounded)
            {
                m_Rigidbody2D.velocity = new Vector2( m_Rigidbody2D.velocity.x + (dir.x * acceleration), m_Rigidbody2D.velocity.y);
                SetTopSpeed(topSpeed);
            }
            else if (!isGrounded)
            {
                m_Rigidbody2D.velocity = new Vector2( m_Rigidbody2D.velocity.x + (dir.x * airSpeed), m_Rigidbody2D.velocity.y);
                SetTopSpeed(airTopSpeed);

            }
            else if (!m_IsMoving && !isGrounded)
            {
                m_Rigidbody2D.velocity = new Vector2( m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y);
            }


        }
        
        void ClampVelocity(float speed)
        {
            if (m_Rigidbody2D.velocity.x > speed)
            {
                m_Rigidbody2D.velocity = new Vector2(speed, m_Rigidbody2D.velocity.y);
            }
            else if (m_Rigidbody2D.velocity.x < -speed)
            {
                m_Rigidbody2D.velocity = new Vector2(-speed, m_Rigidbody2D.velocity.y);
            }
        }
        
        
        // CLAMP THE VELOCITY
        
        
        void SetTopSpeed(float speed)
        {
            if (m_Rigidbody2D.velocity.x > speed)
            {
                m_Rigidbody2D.velocity = new Vector2(speed, m_Rigidbody2D.velocity.y);
            }
            else if (m_Rigidbody2D.velocity.x < -speed)
            {
                m_Rigidbody2D.velocity = new Vector2(-speed, m_Rigidbody2D.velocity.y);
            }
        }
        
        void Jump(Vector2 dir)
        {
            if (!canJump) return;
            var velocity = m_Rigidbody2D.velocity;
            velocity = new Vector2(velocity.x, 0);
            velocity += dir * jumpForce;
            m_Rigidbody2D.velocity = velocity;
            canJump = false;
        }

        void BetterJump()
        {
            if (m_DistanceJoint2D.enabled) return;
            if(m_Rigidbody2D.velocity.y < 0)
            {
                m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }else if(m_Rigidbody2D.velocity.y > 0 && !PlayerBaseControls.Instance.GetJumpInput())
            {
                m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
        
        void GroundCheck()
        {
            isGrounded = CheckGrounded();
            if (isGrounded)
            {
                isJumping = false;
                isFalling = false;
                if (!PlayerBaseControls.Instance.GetJumpInput())
                {
                    canJump = true;
                }
            }
            else
            {
                isFalling = true;
            }
        }

        void IncreaseDrag()
        {
            if (isGrounded || m_DistanceJoint2D.enabled)
            {
                m_Rigidbody2D.drag = 0;
            }
            else if(!isGrounded && !m_DistanceJoint2D.enabled)
            {
                if(m_Rigidbody2D.drag < airDrag)
                {
                    m_Rigidbody2D.drag += Time.deltaTime;
                }
            }
        }
        // Get Momentum of player

        Vector3 DeceleratePlayer(float speed)
        {
            if (m_Rigidbody2D.velocity.x > 0)
            {
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x - speed, m_Rigidbody2D.velocity.y);
            }
            else if (m_Rigidbody2D.velocity.x < 0)
            {
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x + speed, m_Rigidbody2D.velocity.y);
            }
            return m_Rigidbody2D.velocity;
        }

        float DeceleratePlayerHorizontally(float speed)
        {
            if (m_Rigidbody2D.velocity.x > 0)
            {
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x - speed, m_Rigidbody2D.velocity.y);
            }
            else if (m_Rigidbody2D.velocity.x < 0)
            {
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x + speed, m_Rigidbody2D.velocity.y);
            }
            return m_Rigidbody2D.velocity.x;
        }
        
        

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            GroundCheck();
        Walk(PlayerBaseControls.Instance.GetMovementInput());
        IncreaseDrag();
        if (PlayerBaseControls.Instance.GetJumpInput() && isGrounded)
        {
            Jump(Vector2.up);
            isJumping = true;
            isFalling = false;
        }


        BetterJump();
        ClampVelocity(isGrounded ? topSpeed : airTopSpeed);

        }


        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(groundCheck.position, groundDistance);
        }
    }
}
