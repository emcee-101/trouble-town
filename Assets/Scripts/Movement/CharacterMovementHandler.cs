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
    Animator animator;
    private void Awake()
    {
        networkCharacterControllerPrototypeCustom = GetComponent<NetworkCharacterControllerPrototypeCustom>();
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

            if (moveDirection.x != 0.0f || moveDirection.y != 0.0f)
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }

            networkCharacterControllerPrototypeCustom.Move(moveDirection);

            //Jump
            if (networkInputData.isJumpPressed)
                networkCharacterControllerPrototypeCustom.Jump();

            // IF PLAYER == HOST -> change global values
            NetworkObject netObj = gameObject.GetComponent<NetworkObject>();

            if (netObj.HasStateAuthority)
            {
                GameObject states = GameObject.FindGameObjectWithTag("State");

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
        if(gameObject.GetComponent<NetworkObject>().HasStateAuthority)
            FindObjectOfType<game_state>().gameState = GameState.pregame;

        animator = gameObject.GetComponent<Animator>();

        GetComponentInParent<NetworkPlayer>().LobbyStart();
        FindObjectOfType<round_spawner>().Init();
        Respawn();
    }

    public void Respawn()
    {
        if (lobbyUtils == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("State");
            lobbyUtils = obj.GetComponent<UtilLobby>();
        }

        if (lobbyUtils != null)
        {
            positionData spawnPoint = lobbyUtils.GetPlayerSpawnData();

            // lets test if this works better
            transform.position = new Vector3(spawnPoint.returnPos().x, spawnPoint.returnPos().y, spawnPoint.returnPos().z);
            // transform.position = spawnPoint.returnPos();
            
            transform.rotation = new Quaternion(spawnPoint.returnAngle().x, spawnPoint.returnAngle().y, spawnPoint.returnAngle().z, spawnPoint.returnAngle().w);
        }
    }
}
