using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { pregame, game, aftergame };

public class game_state : NetworkBehaviour
{
    public GameObject map;
    public GameObject preMap;

    [Networked(OnChanged = nameof(onGameStateChanged))] public GameState gameState { get; set; } = GameState.pregame;

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
                Debug.Log("Unknown GameState");
                break;
        }
    }

    private void startGame()
    {
        map.SetActive(true);
        preMap.SetActive(false);
        respawnAllPlayersInActiveMap();
    }

    private void startLobby()
    {
        map.SetActive(false);
        preMap.SetActive(true);
        respawnAllPlayersInActiveMap();
    }

    private void endGame()
    {
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
