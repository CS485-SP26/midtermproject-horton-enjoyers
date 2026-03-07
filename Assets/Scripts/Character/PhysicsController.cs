using UnityEngine;

// TODO: Consider the benefits of refactoring to namespace Movement
namespace Character
{
    public class PhysicsController : MovementController
    {
        [SerializeField] float drag = 0.5f;
        [SerializeField] float rotationSpeed = 0.1f;

        Vector3 facingDirection = Vector3.forward;
        
        protected override void Start()
        {
            base.Start();
            rb.linearDamping = drag;
        }

        public void SetFacingDirection(Vector3 direction)
        {
            facingDirection = direction;
        }

        public override float GetHorizontalSpeedPercent()
        {
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            return Mathf.Clamp01(horizontalVelocity.magnitude / maxVelocity);;
        }

        public override void Jump() 
        { 
            // TODO: integrate jump support from week 2-3    
        }

        protected override void FixedUpdate()
        {
            //base.FixedUpdate(); // TODO: remove base.FixedUpdate() when starting your integration
            ApplyMovement();
            ClampVelocity();
            ApplyRotation();
            ApplyJump();
            
        }
        
        void ApplyMovement()
        {
            // TODO integrate your physics from week 2-3
            Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);

            if (movement.magnitude < 0.1f)
            {
                // Kill horizontal velocity immediately when no input
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
                return;
            }

            movement = movement.normalized * acceleration;
            rb.AddForce(movement, ForceMode.Force);
            
        }

        void ApplyJump()
        {
            // TODO integrate your jump logic from week 2-3 
        }

        // TODO integrate collision support from week 2-3 
        
        void ClampVelocity()
        {
            // Clamp horizontal velocity while preserving vertical (for jumping/falling)
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            
            if (horizontalVelocity.magnitude > maxVelocity)
            {
                horizontalVelocity = horizontalVelocity.normalized * maxVelocity;
                rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
            }
        }

        void ApplyRotation()
        {
            Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            if (direction.magnitude < 0.1f) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
        }
    }
}
