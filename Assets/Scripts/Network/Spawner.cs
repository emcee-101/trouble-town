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

    UtilLobby lobbyUtilities;

    CharacterInputHandler characterInputHandler;
    SessionListUIHandler sessionListUIHandler;

    void Awake()
    {
        sessionListUIHandler = FindObjectOfType<SessionListUIHandler>(true);
    }

    void Start()
    {

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Debug.Log($"OnPlayerJoined we are server. Spawning player");

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
}
