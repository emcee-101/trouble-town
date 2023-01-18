using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CharacterMovementHandler : NetworkBehaviour
{
    //Other components
    NetworkCharacterControllerPrototypeCustom networkCharacterControllerPrototypeCustom;
    NetworkInGameMessages networkInGameMessages;
    UtilLobby lobbyUtils = null;

    positionData spawnPoint;

    private void Awake()
    {
        networkCharacterControllerPrototypeCustom = GetComponent<NetworkCharacterControllerPrototypeCustom>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public override void FixedUpdateNetwork()
    {
        //Get the input from the network
        if (GetInput(out NetworkInputData networkInputData))
        {
            //Rotate the transform according to the client aim vector
            transform.forward = networkInputData.aimForwardVector;

            //Cancel out rotation on X axis as we don't want our character to tilt
            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, rotation.eulerAngles.z);
            transform.rotation = rotation;

            //Move
            Vector3 moveDirection = transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x;
            moveDirection.Normalize();

            networkCharacterControllerPrototypeCustom.Move(moveDirection);

            //Jump
            if (networkInputData.isJumpPressed)
                networkCharacterControllerPrototypeCustom.Jump();

            //Check if we've fallen off the world.
            CheckFallRespawn();
        }

    }

    void CheckFallRespawn()
    {
        if (transform.position.y < -12) {

            if(lobbyUtils == null) { 
                
                GameObject obj = GameObject.FindGameObjectWithTag("State");
                lobbyUtils = obj.GetComponent<UtilLobby>();
            }
            

            if(lobbyUtils != null)
            { 
                transform.position = spawnPoint.returnPos();
                transform.rotation = spawnPoint.returnAngle();
            }
            else { transform.position = Utils.GetRandomSpawnPoint(); }
            
        }
    }

    override public void Spawned() {

        Log.Info("Spawned called");

        if (lobbyUtils == null)
        {

            GameObject obj = GameObject.FindGameObjectWithTag("State");
            lobbyUtils = obj.GetComponent<UtilLobby>();
        }


        if (lobbyUtils != null) { 
            spawnPoint = lobbyUtils.GetSpawnData();

            transform.position = spawnPoint.returnPos();
            transform.rotation = spawnPoint.returnAngle();
        }
        else { transform.position = Utils.GetRandomSpawnPoint(); }
    }

}
