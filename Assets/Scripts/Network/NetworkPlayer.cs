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

    // Remote Client Token Hash
    [Networked] public int token { get; set; }

    bool isPublicJoinMessageSent = false;
    public bool isHostAndPolice = false;

    public LocalCameraHandler localCameraHandler;
    public GameObject localUI;

    public GameObject lobbyUI;
    public GameObject gameUI;

    private miniMapScript miniMapCam;

    NetworkInGameMessages networkInGameMessages;

    void Awake()
    {
        networkInGameMessages = GetComponent<NetworkInGameMessages>();
    }

    // Start is called before the first frame update
    void Start()
    {

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

            // Enable Minimap for you
            miniMapCam = GetComponentInChildren<miniMapScript>();
            if (miniMapCam != null) miniMapCam.enable = true;

            RPC_SetNickName(GameManager.instance.playerNickName);

            GameObject obj = GameObject.FindGameObjectWithTag("State");
            game_state gameState = obj.GetComponent<game_state>();

            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                PlayerUI playerUI = player.GetComponent<PlayerUI>();

                // playerUI.updatePlayerCount(gameState.sessionInfo);
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
        transform.name = $"P_{Object.Id}";
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (Object.HasStateAuthority)
        {
            if (Runner.TryGetPlayerObject(player, out NetworkObject playerLeftNetworkObject))
            {
                if (playerLeftNetworkObject == Object)
                    Local.GetComponent<NetworkInGameMessages>().SendInGameRPCMessage(playerLeftNetworkObject.GetComponent<NetworkPlayer>().nickName.ToString(), "left (" + "" + ")");
            }
        }
        if (player == Object.InputAuthority)
            Runner.Despawn(Object);
    }

    static void OnNickNameChanged(Changed<NetworkPlayer> changed)
    {
        Debug.Log($"{Time.time} OnHPChanged value {changed.Behaviour.nickName}");

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

    void OnDestroy()
    {
        //Get rid of the local camera if we get destroyed as a new one will be spawned with the new Network player
        if (localCameraHandler != null)
            Destroy(localCameraHandler.gameObject);
    }

    public void GameStart()
    {
        HideUIs();
        gameUI.SetActive(true);
    }

    public void LobbyStart()
    {
        HideUIs();
        if (isHostAndPolice)
        {
            lobbyUI.SetActive(true);
        }
    }

    public void GameEnd()
    {
        Log.Info("Game has just ended!!");
    }

    private void HideUIs()
    {
        lobbyUI.SetActive(false);
        gameUI.SetActive(false);
    }
}
