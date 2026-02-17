using UnityEngine;
using UnityEngine.InputSystem;
using Farming;

namespace Character 
{
    [RequireComponent(typeof(PlayerInput))] // Input is required and we don't store a reference
    [RequireComponent(typeof(Farmer))]
    public class PlayerController : MonoBehaviour
    {
        
        
        MovementController moveController;
        AnimatedController animatedController;
        [SerializeField] private TileSelector tileSelector;
        Farmer farmer;

        void Start()
        {
            moveController = GetComponent<MovementController>();
            animatedController = GetComponent<AnimatedController>();
            farmer = GetComponent<Farmer>();

            // TODO: Consider Debug.Assert vs RequireComponent(typeof(...))
            Debug.Assert(animatedController, "PlayerController requires an animatedController");
            Debug.Assert(moveController, "PlayerController requires a MovementController");
            Debug.Assert(tileSelector, "Farmer requires a TileSelector.");
            
            
        }
        public void OnMove(InputValue inputValue)
        {
            Vector2 inputVector = inputValue.Get<Vector2>();
            moveController.Move(inputVector);
        }

        public void OnJump(InputValue inputValue)
        {
            moveController.Jump();
        }

        public void OnInteract(InputValue value)
        {
            Debug.Log("Interact Command Recieved");
            FarmTile tile = tileSelector.GetSelectedTile();
            farmer.TryFarming(tile);
        }
        
    }
}