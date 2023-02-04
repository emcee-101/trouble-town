using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed = false;
    private NetworkPlayer networkPlayer;

    //Other components
    LocalCameraHandler localCameraHandler;

    private void Awake()
    {
        localCameraHandler = GetComponentInChildren<LocalCameraHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        networkPlayer = GetComponentInParent<NetworkPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!networkPlayer.isGamePaused)
        {
            //View input
            viewInputVector.x = Input.GetAxis("Mouse X");
            viewInputVector.y = Input.GetAxis("Mouse Y") * -1; //Invert the mouse look

            //Move input
            moveInputVector.x = Input.GetAxis("Horizontal");
            moveInputVector.y = Input.GetAxis("Vertical");

            //Jump
            if (Input.GetButtonDown("Jump"))
                isJumpButtonPressed = true;

            //Set view
            localCameraHandler.SetViewInputVector(viewInputVector);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            networkPlayer.toggleGamePausedState();

        }
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();
        
        //Aim data
        networkInputData.aimForwardVector = localCameraHandler.transform.forward;

        //Move data
        networkInputData.movementInput = moveInputVector;

        //Jump data
        networkInputData.isJumpPressed = isJumpButtonPressed;

        //Reset variables now that we have read their states
        isJumpButtonPressed = false;

        return networkInputData;
    }
}
