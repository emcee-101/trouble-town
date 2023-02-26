using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public TextMeshProUGUI playerNickNameTM;

    public static NetworkPlayer Local { get; set; }
    public Transform playerModel;

    [Networked(OnChanged = nameof(OnNickNameChanged))]
    public NetworkString<_16> nickName { get; set; }

    bool isPublicJoinMessageSent = false;
    public bool isHostAndPolice = false;

    public bool hasMoneyBagItem = false;
    public bool hasCrowbarItem = false;
    public bool hasHandcuffsItem = false;
    public bool hasPhoneItem = false;
    public bool hasSpeedBoostItem = false;

    [Networked(OnChanged = nameof(OnChangeBeingInvestigated))]
    public NetworkBool isBeingInvestigated { get; set; } = false;

    [Networked]
    public NetworkBool supposedToGoToPrison { get; set; } = false;
    [Networked]
    public NetworkBool isCriminal { get; set; } = false;
    [Networked]
    public float prisonTimeDuration { get; set; } = 20;
    public bool isInPrison = false;

    private GameObject map;
    private GameObject preMap;

    public GameObject myHideout = null;
    private GameObject state;

    [Networked]
    public int playerNumber { get; set; } = -1 ;

    public LocalCameraHandler localCameraHandler;
    public GameObject localUI;
    public GameObject lobbyUI;
    public Button lobbyUIStartButton;
    public GameObject gameUI;
    public GameObject endUI;
    public GameObject gamePausedBackgroundUI;

    private ActicateScoreboard scoreUI;

    public bool isGamePaused = false;

    private miniMapScript miniMapCam;

    NetworkInGameMessages networkInGameMessages;

    private ThiefActions thiefActions;


    private bool endUIactivated = false;

    // Information about the end of the game
    [Networked]
    public NetworkBool hasWon { get; set; } = false;
    [Networked]
    public NetworkBool policeWon { get; set; } = false;

    [Networked]
    public NetworkBool gameEnded { get; set; } = false;

    public AudioMixer mixer;

    void Awake()
    {
        networkInGameMessages = GetComponent<NetworkInGameMessages>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        map = FindObjectOfType<game_state>().map;
        preMap = FindObjectOfType<game_state>().preMap;
    }
    
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
        if (Object.HasInputAuthority)
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

            if (PlayerPrefs.HasKey("volume"))
                mixer.SetFloat("masterAudio", PlayerPrefs.GetFloat("volume"));



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
            HideUis();

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

    public override void FixedUpdateNetwork()
    {
        if (gameEnded && Object.HasInputAuthority)
        {
            //Debug.Log("game ended!!!");

            if (!isGamePaused)
                toggleGamePausedState();

            if(!endUIactivated)
            {
                endUIactivated = true;
                HideUis();
                endUI.SetActive(true);
                scoreUI.activated = false;

                endUI.GetComponent<endUI>().showEndUIValues(hasWon, policeWon, gameEnded);

            }

        } 
        // Deactivates endUI appropriately
        else if (endUIactivated)
        {
            endUIactivated = false;
            HideUis();
            endUI.SetActive(false);
            scoreUI.activated = true;

        }

        if (Object.HasInputAuthority && playerNumber != -1 && myHideout == null && !isHostAndPolice)
        {
            //Debug.Log("i was called correctly");
            state = GameObject.FindGameObjectWithTag("State");
            //if (state == null) { Debug.Log("state is null for some reason"); };
            // Player gets his/her own Hideout
            myHideout = state.GetComponent<hideoout_dispatcher>().dispatchHideout(playerNumber);

            if(myHideout == null) Debug.Log("Hideout not set correctly");
            else myHideout.SetActive(true);
        }


        // teleport to prison
        if (!isInPrison && supposedToGoToPrison) {

            gameObject.GetComponent<CharacterMovementHandler>().teleportToPrison();
            isInPrison = true;

        } else if (isInPrison && !supposedToGoToPrison){

            gameObject.GetComponent<CharacterMovementHandler>().teleportBackToMap();
            isInPrison = false;
        }



    }

    public void PlayerLeft(PlayerRef player)
    {
        if (Object.HasStateAuthority)
        {
            GameObject.FindGameObjectWithTag("State").GetComponent<scoring>().removePlayer(nickName.ToString());


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
        if (!Object.HasInputAuthority){
            return;
        }
        if (isGamePaused)
            toggleGamePausedState();

        HideAllMaps();
        map.SetActive(true);

        HideUis();
        gameUI.SetActive(true);
        GetComponent<PlayerUI>().Init();
        if (scoreUI != null)
            scoreUI.activated = true;
    }

    public void LobbyStart()
    {
        gameEnded = false;
        if (isGamePaused)
            toggleGamePausedState();

        HideAllMaps();
        preMap.SetActive(true);

        HideUis();
        if (isHostAndPolice && Object.HasInputAuthority)
        {
            lobbyUI.SetActive(true);
            lobbyUIStartButton.interactable = false;
        }
    }

    private void HideUis()
    {
        lobbyUI.SetActive(false);
        gameUI.SetActive(false);
        endUI.SetActive(false);

        if (scoreUI != null)
            scoreUI.activated = false;
    }

    private void HideAllMaps()
    {
        if (map == null)
        {
            map = FindObjectOfType<game_state>().map;
            preMap = FindObjectOfType<game_state>().preMap;
        }
        map.SetActive(false);
        preMap.SetActive(false);
    }

    public static void OnChangeBeingInvestigated(Changed<NetworkPlayer> changed)
    {
        changed.Behaviour.OnChangeBeingInvestigated();
    }
    private void OnChangeBeingInvestigated()
    {
        //if (!isBeingInvestigated) return;
        ThiefActions tA = GetComponent<ThiefActions>();
        tA.getInvestigated();
    }
}
