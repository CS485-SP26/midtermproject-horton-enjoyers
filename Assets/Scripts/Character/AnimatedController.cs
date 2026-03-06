using Character;
using UnityEngine;

namespace Character {
    public class AnimatedController : MonoBehaviour
    {
        [SerializeField] float moveSpeed; // useful to observe for debugging
        MovementController moveController;
        PhysicsController physicsController;
        Animator animator;
        protected Animator Animator { get { return animator; } }
        void Start()
        {
            animator = GetComponent<Animator>();
            moveController = GetComponent<MovementController>();
            physicsController = GetComponent<PhysicsController>();
        }

        public void SetTrigger(string name)
        {
            animator.SetTrigger(name);
        }

        public void SetTool(string toolName) { }
        void Update()
        {
            moveSpeed = physicsController.GetHorizontalSpeedPercent();
            animator.SetFloat("Speed", moveSpeed);
        }
    }
}
