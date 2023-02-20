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

    round_spawner roundSpawner;

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

            GameObject obj = GameObject.FindGameObjectWithTag("State");
            roundSpawner = obj.GetComponent<round_spawner>();

            int activePlayers = runner.ActivePlayers.Count();

            bool isPolice = activePlayers == 1;

            NetworkPlayer playerPrefab;

            if (isPolice)
            {
                playerPrefab = playerPolicePrefabs[UnityEngine.Random.Range(0, playerPolicePrefabs.Count())];
            }
            else
            {
                playerPrefab = playerRobberPrefabs[UnityEngine.Random.Range(0, playerRobberPrefabs.Count())];
            }

            playerPrefab.isHostAndPolice = isPolice;

            Debug.Log("Activeplayers: " + activePlayers + " / " + runner.SessionInfo.MaxPlayers);

            

            // Spawning happens in PlayerPrefab->CharacterMovemetnHandler->Spawned() now
            NetworkPlayer spawnedObject = runner.Spawn(playerPrefab, inputAuthority: player);

            if (activePlayers >= runner.SessionInfo.MaxPlayers)
            {
                FindObjectOfType<game_state>().host.lobbyUIStartButton.interactable = true;
            }

            if (spawnedObject.isHostAndPolice)
            {
                FindObjectOfType<game_state>().host = spawnedObject;
                FindObjectOfType<game_state>().hostID = player.PlayerId;
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
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {
        if (runner.ActivePlayers.Count() < runner.SessionInfo.MaxPlayers)
        {
            FindObjectOfType<game_state>().host.lobbyUIStartButton.interactable = false;
            if (FindObjectOfType<game_state>().hostID == player.PlayerId)
            {
                Debug.Log("HOST LEFT THE GAME");
            }
        }
    }
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
