using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { pregame, game, aftergame, defaultState };

public class game_state : NetworkBehaviour
{
    public GameObject map;
    public GameObject preMap;

    [Networked(OnChanged = nameof(onGameStateChanged))] public GameState gameState { get; set; } = GameState.defaultState;

    public static void onGameStateChanged(Changed<game_state> changed)
    {

        changed.Behaviour.onGameStateChanged();

    }

    private void onGameStateChanged()
    {
        switch (gameState)
        {
            case GameState.pregame:
                Debug.Log("GAMESTATE: pregame");
                startLobby();
                break;
            case GameState.game:
                Debug.Log("GAMESTATE: game");
                startGame();
                break;
            case GameState.aftergame:
                Debug.Log("GAMESTATE: aftergame");
                endGame();
                break;
            default:
                Debug.Log("Unknown/Default GameState");
                break;
        }
    }

    private void startGame()
    {
        HideAllMaps();
        map.SetActive(true);

        foreach (GameObject player in getAllNetworkPlayers())
        {
            NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();
            networkPlayer.GameStart();
        }

        respawnAllPlayersInActiveMap();
    }

    private void startLobby()
    {
        HideAllMaps();
        preMap.SetActive(true);
        
        foreach (GameObject player in getAllNetworkPlayers())
        {
            NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();
            networkPlayer.LobbyStart();
        }

        respawnAllPlayersInActiveMap();
    }

    private void HideAllMaps()
    {
        map.SetActive(false);
        preMap.SetActive(false);
    }

    private void endGame()
    {
        foreach (GameObject player in getAllNetworkPlayers())
        {
            NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();
            networkPlayer.GameEnd();
        }
    }

    private void respawnAllPlayersInActiveMap()
    {
        foreach (GameObject player in getAllNetworkPlayers())
        {
            CharacterMovementHandler charMove = player.GetComponent<CharacterMovementHandler>();
            charMove.Respawn();
        }
    }

    private GameObject[] getAllNetworkPlayers()
    {
        return GameObject.FindGameObjectsWithTag("Player");
    }
}
