using UnityEngine;
using UnityEngine.InputSystem;
using Cameras;
using Character;

public class PlayerCameraController : PlayerController 
{
    [SerializeField] OverShoulder overShoulder;
    
    void OnLook(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        Vector3 euler = new Vector3(0f, input.x, 0f);
        Quaternion rotation = Quaternion.Euler(euler);
        overShoulder.AddRotation(rotation);
    }

    public override void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        Vector3 direction = new Vector3(input.x, 0f, input.y);
        Vector3 cameraForward = overShoulder.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();
        Vector3 cameraRight = overShoulder.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();
        Vector3 worldDirection = cameraForward * input.y + cameraRight * input.x;

        moveController.Move(new Vector2(worldDirection.x, worldDirection.z));
    }
}