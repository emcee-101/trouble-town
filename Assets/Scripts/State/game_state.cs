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
        String playerName = "";
        float biggestMoney = 0.0f;
        bool policeWon;

        GameObject state = GameObject.FindGameObjectWithTag("State");
        
        if (state.GetComponent<global_money>().GlobalMoney <= 0)
        {

            Log.Info("Police lost - all the money's gone!!!");
            policeWon = false;

        } else
        {

            Log.Info("Theres still money in da Bank - Police won");
            policeWon = true;

        }

        foreach (GameObject player in getAllNetworkPlayers())
        {
            NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();

            
            // decide winner
            if(networkPlayer.GetComponentInParent<PlayerMoney>().currentMoney > biggestMoney) {

                playerName = networkPlayer.nickName.ToString();
                biggestMoney = networkPlayer.GetComponentInParent<PlayerMoney>().currentMoney;

            }

            // Nothing happens here atm
            networkPlayer.GameEnd();
        }

        Log.Info($"Player named {playerName} won with a money count of {biggestMoney}");


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
