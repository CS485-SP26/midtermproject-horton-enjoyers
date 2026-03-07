using UnityEngine;
using UnityEngine.InputSystem;
using Farming;
using Core;

namespace Character 
{
    [RequireComponent(typeof(PlayerInput))] // Input is required and we don't store a reference
    [RequireComponent(typeof(Farmer))]
    public class PlayerController : MonoBehaviour
    {
        
        
        protected MovementController moveController;
        AnimatedController animatedController;
        [SerializeField] private TileSelector tileSelector;
        [SerializeField] private Transform handTransform;
        Farmer farmer;
        IInteractable nearbyInteractable;
        ToolPickup nearbyTool;
        ToolPickup heldTool;

        public void SetNearbyInteractable(IInteractable interactable)
        {
            nearbyInteractable = interactable;
        }

        public void SetNearbyTool(ToolPickup tool)
        {
            nearbyTool = tool;
        }

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
        public virtual void OnMove(InputValue inputValue)
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
            if (nearbyInteractable != null)
            {
                nearbyInteractable.Interact();
                return;
            }
            FarmTile tile = tileSelector.GetSelectedTile();
            farmer.TryFarming(tile);
        }

        public void OnPickup(InputValue value)
        {
            if (heldTool != null)
            {
                Vector3 dropPos = transform.position + transform.forward * 1.5f;
                heldTool.Drop(dropPos, farmer);
                heldTool = null;
                return;
            }

            if (nearbyTool != null)
            {
                Debug.Assert(handTransform, "PlayerController needs a handTransform assigned to pick up tools.");
                nearbyTool.Pickup(handTransform, farmer);
                heldTool = nearbyTool;
                nearbyTool = null;
            }
        }

        void OnDisable()
        {
            // Clear tool state so it's clean when the farm scene reloads.
            // Don't call Drop() here — scene teardown may have already destroyed the tool object.
            if (farmer != null)
                farmer.SetActiveTool(ToolType.None);
            heldTool = null;
            nearbyTool = null;
        }

        public void OnCycleSeed()
        {
            GameManager.Instance.playerData.CycleSeed();
        }

    }
}