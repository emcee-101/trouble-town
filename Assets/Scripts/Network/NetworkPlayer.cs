using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public TextMeshProUGUI playerNickNameTM;

    public static NetworkPlayer Local { get; set; }
    public Transform playerModel;

    [Networked(OnChanged = nameof(OnNickNameChanged))]
    public NetworkString<_16> nickName { get; set; }

    bool isPublicJoinMessageSent = false;
    public bool isHostAndPolice = false;
    public bool hasBlueRobberItem = false;
    public bool hasRedPoliceItem = false;
    
    public bool getInvestigated = false;

    public LocalCameraHandler localCameraHandler;
    public GameObject localUI;

    public GameObject lobbyUI;
    public GameObject gameUI;
    public GameObject endUI;
    public GameObject gamePausedBackgroundUI;

    private ActicateScoreboard scoreUI;

    public bool isGamePaused = false;

    private miniMapScript miniMapCam;

    NetworkInGameMessages networkInGameMessages;

    void Awake()
    {
        networkInGameMessages = GetComponent<NetworkInGameMessages>();
    }

    // Start is called before the first frame update
    private void Start() { }
    
    private void Update()
    {
        
        if (FindObjectOfType<game_state>().gameState == GameState.pregame)
        {
            //Update LobbyUI for HostAndPolice
            foreach (NetworkPlayer nP in FindObjectsOfType<NetworkPlayer>())
            {
                if (nP.isHostAndPolice)
                {
                    NetworkRunner runner = FindObjectOfType<NetworkRunner>();
                    int activePlayers = 0;
                    foreach (PlayerRef pR in runner.ActivePlayers)
                        activePlayers++;
                    
                    nP.GetComponentInParent<PlayerUI>()
                        .updatePlayerCount(activePlayers, runner.SessionInfo.MaxPlayers);
                    break;
                }
            }
        }
    }

    public void toggleGamePausedState()
    {
        isGamePaused = !isGamePaused;

        Cursor.visible = isGamePaused;
        gamePausedBackgroundUI.SetActive(isGamePaused);

        if (isGamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;

            //Sets the layer of the local players model
            Utils.SetRenderLayerInChildren(playerModel, LayerMask.NameToLayer("LocalPlayerModel"));
                        
            //Enable 1 audio listner
            AudioListener audioListener = GetComponentInChildren<AudioListener>(true);
            audioListener.enabled = true;
            
            // Enable the local camera
            localCameraHandler.localCamera.enabled = true;

            // Detach camera if enabled
            localCameraHandler.transform.parent = null;

            // Enable UI for local player
            localUI.SetActive(true);

            scoreUI = GetComponentInChildren<ActicateScoreboard>();

            // Enable Minimap for you
            miniMapCam = GetComponentInChildren<miniMapScript>();
            if (miniMapCam != null) miniMapCam.enable = true;

            RPC_SetNickName(PlayerPrefs.GetString("PlayerNickname"));

            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                NetworkRunner runner = FindObjectOfType<NetworkRunner>();
                PlayerUI playerUI = player.GetComponent<PlayerUI>();
                playerUI.updatePlayerCount(runner.SessionInfo.PlayerCount, runner.SessionInfo.MaxPlayers);
            }



            Debug.Log("Spawned local player");
        }
        else
        {
            //Disable the camera if we are not the local player
            localCameraHandler.localCamera.enabled = false;

            // Disable Minimap for every other player
            miniMapCam = GetComponentInChildren<miniMapScript>();
            if (miniMapCam != null) miniMapCam.enable = false;

            // Disable UI for remote player
            localUI.SetActive(false);

            //Only 1 audio listner is allowed in the scene so disable remote players audio listner
            AudioListener audioListener = GetComponentInChildren<AudioListener>();
            audioListener.enabled = false;

            Debug.Log("Spawned remote player");
        }

        //Set the Player as a player object
        Runner.SetPlayerObject(Object.InputAuthority, Object);

        //Make it easier to tell which player is which.
        transform.name = $"Player_{Object.Id}";
}

    public void PlayerLeft(PlayerRef player)
    {

        // deregister Player from Scoring
        GameObject obj = GameObject.FindGameObjectWithTag("State");
        scoring Scores = obj.GetComponent<scoring>();
        Scores.removePlayer(nickName.ToString());


        if (Object.HasStateAuthority)
        {
            if (Runner.TryGetPlayerObject(player, out NetworkObject playerLeftNetworkObject))
            {
                if (playerLeftNetworkObject == Object)
                    Local.GetComponent<NetworkInGameMessages>()
                        .SendInGameRPCMessage(playerLeftNetworkObject.GetComponent<NetworkPlayer>()
                        .nickName.ToString(), "left");
            }
        }
        if (player == Object.InputAuthority)
            Runner.Despawn(Object);
    }

    static void OnNickNameChanged(Changed<NetworkPlayer> changed)
    {
        Debug.Log($"{Time.time} OnNickNameChanged value {changed.Behaviour.nickName}");

        changed.Behaviour.OnNickNameChanged();
    }

    private void OnNickNameChanged()
    {
        Debug.Log($"Nickname changed for player to {nickName} for player {gameObject.name}");

        playerNickNameTM.text = nickName.ToString();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetNickName(string nickName, RpcInfo info = default)
    {
        Debug.Log($"[RPC] SetNickName {nickName}");
        this.nickName = nickName;

        if (!isPublicJoinMessageSent)
        {
            networkInGameMessages.SendInGameRPCMessage(nickName, "joined");

            isPublicJoinMessageSent = true;
        }
    }

    public void GameStart()
    {
        if (isGamePaused)
            toggleGamePausedState();

        HideUisAndSetGameUnPause();
        gameUI.SetActive(true);
        GetComponent<PlayerUI>().Init();
        if (scoreUI != null)
            scoreUI.activated = true;
    }

    public void LobbyStart()
    {
        if (isGamePaused)
            toggleGamePausedState();

        HideUisAndSetGameUnPause();
        if (isHostAndPolice)
        {
            lobbyUI.SetActive(true);
        }


        // register Player for Scoring
        GameObject obj = GameObject.FindGameObjectWithTag("State");
        scoring Scores = obj.GetComponent<scoring>();
        Scores.registerPlayer(nickName.ToString());



    }

    public void GameEnd()
    {
        if (!isGamePaused)
            toggleGamePausedState();

        HideUisAndSetGameUnPause();

        endUI.SetActive(true);
        scoreUI.activated = false;
    }

    private void HideUisAndSetGameUnPause()
    {
        lobbyUI.SetActive(false);
        gameUI.SetActive(false);
        endUI.SetActive(false);

        if (scoreUI != null)
            scoreUI.activated = false;
    }
}
