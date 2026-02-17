using UnityEngine;
using UnityEngine.InputSystem;

namespace HW2
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private WateringCan wateringCan;
        [SerializeField] private Collider groundCollider;
        [SerializeField] private float minJumpHeight = 0.5f;
        // Assign the Rigidbody in the inspector
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Animator animator;
        Vector2 moveInput;
        bool jumpInput;
        bool waterInput;
        bool dropInput;
        private bool landingTriggered = false;


        // movement
        float acceleration = 10f;
        float maxSpeed = 5f;
        int remainingJumps = 2;
        float jumpForce = 5f;
        bool onGround;

        

        // wall jump related
        Vector3 wallNormal;
        [SerializeField] float wallJumpTimeout = 2f;
        bool canWallJump;
        float wallJumpTimer;
        float wallJumpHorizontalForce = 3f;
        float wallJumpVerticalForce = 7f;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
            // optionally initialize things here
            rb = GetComponent<Rigidbody>();
            rb.angularDamping = 15f;
            Debug.Assert(rb, "This playercontroller requires a Rigidbody.");

            PlayerInput pi = GetComponent<PlayerInput>();
            Debug.Assert(pi, "This playercontroller requires a PlayerInput.");

            animator = GetComponent<Animator>();
            Debug.Assert(animator, "This playercontroller requires a Animator.");


        }

        public void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
            Debug.Log("Move Triggered: " + moveInput);
        }

        public void OnJump(InputValue value)
        {
            jumpInput = value.isPressed;
            Debug.Log("Jump Triggered: " + jumpInput);
            jumpInput = true;
        }

        public void OnWater(InputValue value)
        {
            waterInput = value.isPressed;
            Debug.Log("Water Triggered: " + waterInput);
            animator.SetTrigger("Water");
        }

        // Update is called once per frame
        void Update()
        {
            // TODO: add conditional logic to apply
            // movement and jump inputs to the
            // Rigidbody rb.AddForce
            

            // wall jump logic
            if (canWallJump && jumpInput)
            {
                Debug.Log("Wall Jump Initiated");
                // reset movement before applying new movement
                rb.linearVelocity = Vector3.zero;

                // calculate direction and magnitude of wall jump
                Vector3 jumpDir = wallNormal * wallJumpHorizontalForce + Vector3.up * wallJumpVerticalForce;
                rb.AddForce(jumpDir, ForceMode.Impulse);

                canWallJump = false;
                jumpInput= false;
                animator.SetTrigger("Jump");
            }
            // normal jump logic
            else if (jumpInput && remainingJumps > 0)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpInput = false;
                remainingJumps -= 1;
                onGround = false;
                animator.SetTrigger("Jump");
            }

            // update wall jump timer
            if (canWallJump)
            {
                wallJumpTimer -= Time.deltaTime;

                // missed timewindow, no wall jump
                if (wallJumpTimer <= 0f)
                {
                    canWallJump = false;
                    Debug.Log("Wall Jump Timeout");
                }  

            }

            
        }

        void FixedUpdate()
        {
            if (moveInput.magnitude > 0.01f)
            {
                Vector3 accel = new Vector3(moveInput.x, 0f, moveInput.y);
                accel *= acceleration;
                rb.AddForce(accel, ForceMode.Acceleration);
            }

            Vector3 currentVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            // Debug.Log("Current velocity = " + currentVelocity.magnitude);
            if (currentVelocity.magnitude > maxSpeed)
            {
                Vector3 overrideVelocity = currentVelocity.normalized * maxSpeed;
                rb.linearVelocity = new Vector3(overrideVelocity.x,  rb.linearVelocity.y, overrideVelocity.z);
            }

            
            float speedBlend = rb.linearVelocity.magnitude / maxSpeed;
            animator.SetFloat("Speed", speedBlend);

            Vector3 facing = rb.linearVelocity;
            facing.y = 0f;
            if (rb.linearVelocity.magnitude > 1f)
            {

                // turn to current velocity direction
                transform.LookAt(transform.position + facing);

            }
        }
            

        
        void OnCollisionEnter(Collision collision)
        {

            onGround = collision.gameObject.CompareTag("Ground");
            if (onGround)
            {
                // TODO: add support for resetting jump logic
                remainingJumps = 2;
            }

            foreach (ContactPoint contact in collision.contacts)
            {
                // use dot product of object and world vector3.up to find object orientation in the world, close to 0 is wall close to +/- 0.5 is ground/ceiling
                float dotOrientation = Vector3.Dot(contact.normal, Vector3.up);

                if (Mathf.Abs(dotOrientation) < 0.2f && !onGround)
                {
                    wallNormal = contact.normal;
                    wallJumpTimer = wallJumpTimeout;
                    canWallJump = true;
                    Debug.Log("Can Wall Jump");
                }
            }
        }

        void OnDrop(InputValue value)
        {
            dropInput = value.isPressed;

            wateringCan.Drop();
        }

        void OnTriggerStay(Collider collider)
        {

            if(landingTriggered) return; // prevent retriggering

            Vector3 closestPoint = collider.ClosestPointOnBounds(transform.position);
            Vector3 normal = (transform.position - closestPoint).normalized;
            float dot = Vector3.Dot(normal, Vector3.up);

            if (dot >= 0.7)
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // base layer = 0
                if (stateInfo.IsName("Jump"))
                {
                    // Trigger landing animation
                    animator.SetTrigger("Land");
                    landingTriggered = true;
                    Debug.Log("Landing Triggered");
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            landingTriggered = false; // reset for the next jump
        }
    }    
}
