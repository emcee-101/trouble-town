using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CharacterMovementHandler : NetworkBehaviour
{
    Vector2 viewInput;

    //Rotation
    float cameraRotationX = 0;

    //Other components
    NetworkCharacterControllerPrototypeCustom networkCharacterControllerPrototypeCustom;
    Camera localCamera;

    private void Awake()
    {
        networkCharacterControllerPrototypeCustom = GetComponent<NetworkCharacterControllerPrototypeCustom>();
        localCamera = GetComponentInChildren<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        cameraRotationX += viewInput.y * Time.deltaTime * networkCharacterControllerPrototypeCustom.viewUpDownRotationSpeed;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90, 90);

        localCamera.transform.localRotation = Quaternion.Euler(cameraRotationX, 0, 0);
    }

    // Update-Function a cross the network (server is place where to move players etc)
    public override void FixedUpdateNetwork()
    {
        // get input from network
        if (GetInput(out NetworkInputData networkInputData))
        {
            // transform view-rotation
            networkCharacterControllerPrototypeCustom.Rotate(networkInputData.rotationInput);

            // transform position
            Vector3 moveDirection = transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x;
            moveDirection.Normalize();
            networkCharacterControllerPrototypeCustom.Move(moveDirection);

            // jump if nesseccary
            if (networkInputData.isJumpPressed)
                networkCharacterControllerPrototypeCustom.Jump();
        }
    }

    public void SetViewInputVector(Vector2 viewInput)
    {
        this.viewInput = viewInput;
    }
}
