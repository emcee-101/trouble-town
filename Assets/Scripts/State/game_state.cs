using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameState { pregame, game, aftergame, defaultState };

public class game_state : NetworkBehaviour
{
    private NetworkRunner networkRunner;
    public NetworkPlayer host;
    public int hostID;

    public GameObject map;
    public GameObject preMap;

    [Networked(OnChanged = nameof(onGameStateChanged))] public GameState gameState { get; set; } = GameState.defaultState;

    private void Start()
    {
        FindObjectOfType<round_spawner>().Init();
        networkRunner = FindObjectOfType<NetworkRunner>();
    }

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
        if (networkRunner == null)
        {
            networkRunner = FindObjectOfType<NetworkRunner>();
        }

        networkRunner.SessionInfo.IsOpen = false;

        foreach (NetworkPlayer networkPlayer in FindObjectsOfType<NetworkPlayer>())
        {
            networkPlayer.GameStart();
        }

        // start the round timer
        gameObject.GetComponent<round_timer>().startTimer();

        respawnAllPlayersInActiveMap();
    }

    private void startLobby()
    {
        if (networkRunner == null)
        {
            networkRunner = FindObjectOfType<NetworkRunner>();
        }

        networkRunner.SessionInfo.IsOpen = true;

        gameObject.GetComponent<scoring>().initScores();

        foreach (NetworkPlayer networkPlayer in FindObjectsOfType<NetworkPlayer>())
        {
            networkPlayer.LobbyStart();
        }
        
        respawnAllPlayersInActiveMap();
    }

    private void endGame()
    {
        float biggestScore = 0.0f;
        bool policeWon = false;
        NetworkPlayer winnerPlayer = null;

        GameObject state = gameObject;

        state.GetComponent<round_timer>().stopTimer();
        
        if ((state.GetComponent<global_money>().GlobalMoney > 0))
        {

            policeWon = true;

        } 

        NetworkDictionary<string, float> list = state.GetComponent<scoring>().scorings;


        foreach (NetworkPlayer networkPlayer in FindObjectsOfType<NetworkPlayer>())
        {
            string name = networkPlayer.nickName.ToString();
            float score = list.Get(name);

            // decide winner
            if (score >= biggestScore) {

                winnerPlayer = networkPlayer;
                biggestScore = score;

            }
        }

        foreach (NetworkPlayer networkPlayer in FindObjectsOfType<NetworkPlayer>())
        {
            if(networkPlayer.nickName.ToString() == winnerPlayer.nickName.ToString())
                networkPlayer.hasWon = true;

            networkPlayer.policeWon = policeWon;
            networkPlayer.gameEnded = true;

        }


    }

    private void respawnAllPlayersInActiveMap()
    {
        foreach (CharacterMovementHandler charMove in FindObjectsOfType<CharacterMovementHandler>())
        {
            charMove.Respawn();
        }
    }
}
