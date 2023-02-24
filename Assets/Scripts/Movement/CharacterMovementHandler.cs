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
    GameObject states;
    

    [Networked]
    NetworkBool isWalking { get; set; } = false;
    private NetworkMecanimAnimator _networkAnimator;

    bool wantsToTeleport = false;
    positionData destination;

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

            // teleport to spawn
            if (wantsToTeleport)
            {
                wantsToTeleport = false;
                networkCharacterControllerPrototypeCustom.teleport(destination);
            }


            // IF PLAYER == HOST -> change global values
            NetworkObject netObj = gameObject.GetComponent<NetworkObject>();

            if (netObj.HasStateAuthority)
            {

                if(states == null) states = GameObject.FindGameObjectWithTag("State");

                states.GetComponent<global_money>().GlobalMoney += networkInputData.globalMoneyChange;
                //Debug.Log(states.GetComponent<global_money>().GlobalMoney);

                string name = networkInputData.playerName.ToString();
                if (!states.GetComponent<scoring>().checkIfRegistered(name) && name != "" && name != "FAILURE")
                {
                    states.GetComponent<scoring>().registerPlayer(name);
                }
                if (name != "" && name != "FAILURE")
                    states.GetComponent<scoring>().addPoints(name, networkInputData.scoreChange);
            }

            // set Animation state -> is a synchronised value provided by the "Network Mecanim Animator" assigned to this instance
            if ((moveDirection.x != 0.0f || moveDirection.y != 0.0f) && IsProxy != true)
            {
                _networkAnimator.Animator.SetBool("isWalking", true);
            }
            else
            {
                _networkAnimator.Animator.SetBool("isWalking", false);
            }
    

            //Check if we've fallen off the world.
            CheckFallRespawn();
        }

    }

    void CheckFallRespawn()
    {
        if (transform.position.y < -12) {
            Respawn();
        }
    }

    override public void Spawned()
    {
        networkCharacterControllerPrototypeCustom = GetComponent<NetworkCharacterControllerPrototypeCustom>();

        if (states == null) states = GameObject.FindGameObjectWithTag("State");

        if (gameObject.GetComponent<NetworkObject>().HasStateAuthority)
            states.GetComponent<game_state>().gameState = GameState.pregame;

        _networkAnimator = GetComponent<NetworkMecanimAnimator>();

        GetComponentInParent<NetworkPlayer>().LobbyStart();
        FindObjectOfType<round_spawner>().Init();
        Respawn();
    }

    public void Respawn()
    {
        
        
         if (states == null) states = GameObject.FindGameObjectWithTag("State");
         lobbyUtils = states.GetComponent<UtilLobby>();

         positionData spawnPoint;

        if (states.GetComponent<game_state>().gameState == GameState.pregame)
        {
            // 1st spawn
            spawnPoint = lobbyUtils.GetPlayerSpawnData(true);

            //Debug.Log("point: " + spawnPoint.returnPos());

            gameObject.GetComponent<NetworkCharacterControllerPrototypeCustom>().teleport(spawnPoint);


        } else
        {
            // 2nd spawn when round starts....
            destination = lobbyUtils.GetPlayerSpawnData(false);
            wantsToTeleport = true;

        }


    }
}
