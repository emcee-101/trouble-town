using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using Fusion;
using Fusion.Sockets;
using System;
using Newtonsoft.Json.Linq;
using static UtilLobby;
using System.Linq;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public List<NetworkPlayer> playerPolicePrefabs;
    public List<NetworkPlayer> playerRobberPrefabs;

    // Mapping between Token ID and Re-created Players
    Dictionary<int, NetworkPlayer> mapTokenIDWithNetworkPlayer;
    UtilLobby lobbyUtilities;

    CharacterInputHandler characterInputHandler;
    SessionListUIHandler sessionListUIHandler;

    void Awake()
    {
        // Create a new Dictionary
        mapTokenIDWithNetworkPlayer = new Dictionary<int, NetworkPlayer>();

        sessionListUIHandler = FindObjectOfType<SessionListUIHandler>(true);
    }

    void Start()
    {

    }

    int GetPlayerToken(NetworkRunner runner, PlayerRef player)
    {
        if (runner.LocalPlayer == player)
        {
            // Just use the local Player Connection Token
            return ConnectionTokenUtils.HashToken(GameManager.instance.GetConnectionToken());
        }
        else
        {
            // Get the Connection Token stored when the Client connects to this Host
            var token = runner.GetPlayerConnectionToken(player);

            if (token != null)
                return ConnectionTokenUtils.HashToken(token);

            Debug.LogError($"GetPlayerToken returned invalid token");

            return 0; // invalid
        }
    }

    public void SetConnectionTokenMapping(int token, NetworkPlayer networkPlayer)
    {
        mapTokenIDWithNetworkPlayer.Add(token, networkPlayer);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            //Get the token for the player
            int playerToken = GetPlayerToken(runner, player);

            Debug.Log($"OnPlayerJoined we are server. Connection token {playerToken}");

            //Check if the token is already recorded by the server. 
            if (mapTokenIDWithNetworkPlayer.TryGetValue(playerToken, out NetworkPlayer networkPlayer))
            {
                Debug.Log($"Found old connection token for token {playerToken}. Assigning controlls to that player");

                networkPlayer.GetComponent<NetworkObject>().AssignInputAuthority(player);

                networkPlayer.Spawned();
            }
            else
            {
                Debug.Log($"Spawning new player for connection token {playerToken}");
                NetworkPlayer spawnedNetworkPlayer = null;

                if (lobbyUtilities == null)
                {
                    GameObject obj = GameObject.FindGameObjectWithTag("State");
                    lobbyUtilities = obj.GetComponent<UtilLobby>();
                }

                bool isPolice = runner.ActivePlayers.Count() == 1;

                NetworkPlayer playerPrefab = isPolice
                    ? playerPolicePrefabs[UnityEngine.Random.Range(0, playerPolicePrefabs.Count())]
                    : playerRobberPrefabs[UnityEngine.Random.Range(0, playerRobberPrefabs.Count())];

                playerPrefab.isHostAndPolice = isPolice;

                if (lobbyUtilities != null)
                {
                    positionData spawnData = lobbyUtilities.GetSpawnData();

                    // Spawning happens in PlayerPrefab->CharacterMovemetnHandler->Spawned() now
                    spawnedNetworkPlayer = runner.Spawn(playerPrefab, inputAuthority: player);
                }
                //else
                //    spawnedNetworkPlayer = runner.Spawn(playerPrefab, Utils.GetRandomSpawnPoint(), Quaternion.identity, player);

                //Store the token for the player
                spawnedNetworkPlayer.token = playerToken;

                //Store the mapping between playerToken and the spawned network player
                mapTokenIDWithNetworkPlayer[playerToken] = spawnedNetworkPlayer;
                }
        }
        else Debug.Log("OnPlayerJoined");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (characterInputHandler == null && NetworkPlayer.Local != null)
            characterInputHandler = NetworkPlayer.Local.GetComponent<CharacterInputHandler>();

        if (characterInputHandler != null)
            input.Set(characterInputHandler.GetNetworkInput());
    }

    // nessaccary Methods with logs
    public void OnConnectedToServer(NetworkRunner runner) { Debug.Log("OnConnectedToServer"); }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { Debug.Log("OnShutdown"); }
    public void OnDisconnectedFromServer(NetworkRunner runner) { Debug.Log("OnDisconnectedFromServer"); }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { Debug.Log("OnConnectRequest"); }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { Debug.Log("OnConnectFailed"); }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        if (sessionListUIHandler == null)
            return;

        if (sessionList.Count == 0)
        {
            Debug.Log("Joined lobb no sessions found");

            sessionListUIHandler.OnNoSessionsFound();
        }
        else
        {
            sessionListUIHandler.ClearList();

            foreach (SessionInfo sessionInfo in sessionList)
            {
                sessionListUIHandler.AddToList(sessionInfo);

                Debug.Log($"Found session {sessionInfo.Name} playerCount {sessionInfo.PlayerCount}");
            }
        }

        sessionListUIHandler.ShowButtons();
    }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    #region CarAndItemLogicHere
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    
    #endregion
    public void OnHostMigrationCleanUp()
    {
        Debug.Log("Spawner OnHostMigrationCleanUp started");

        foreach (KeyValuePair<int, NetworkPlayer> entry in mapTokenIDWithNetworkPlayer)
        {
            NetworkObject networkObjectInDictionary = entry.Value.GetComponent<NetworkObject>();

            if (networkObjectInDictionary.InputAuthority.IsNone)
            {
                Debug.Log($"{Time.time} Found player that has not reconnected. Despawning {entry.Value.nickName}");

                networkObjectInDictionary.Runner.Despawn(networkObjectInDictionary);
            }
        }

        Debug.Log("Spawner OnHostMigrationCleanUp completed");
    }
}
