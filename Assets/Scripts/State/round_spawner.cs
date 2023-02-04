using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class round_spawner : NetworkBehaviour
{
    private NetworkRunner networkRunnerInScene;
    private round_timer roundTimer;
    private TickTimer timer;

    public float secondsForItemWave1;

    public BlueRobberItem blueRobberItemPrefab;
    public RedPoliceItem redPoliceItemPrefab;

    UtilLobby lobbyUtils;
    public List<BlueRobberItem> spawnedBlueRobberItems;
    public List<RedPoliceItem> spawnedRedPoliceItems;

    bool itemWave1 = false;

    private void Start()
    {
        networkRunnerInScene = FindObjectOfType<NetworkRunner>();
        roundTimer = FindObjectOfType<round_timer>();
        timer = roundTimer.timer;
        lobbyUtils = GameObject.FindGameObjectWithTag("State").GetComponent<UtilLobby>();

        secondsForItemWave1 = roundTimer.timeForOneRoundInSeconds - 10;
        Debug.Log("seconds:" + secondsForItemWave1);
    }

    public override void FixedUpdateNetwork()
    {
        if (timer.RemainingTime(networkRunnerInScene) < secondsForItemWave1 && !itemWave1)
        {
            itemWave1 = true;
            SpawnWave1();
        }
    }

    private void SpawnWave1()
    {
        positionData spawnPoint = lobbyUtils.GetItemSpawnData();
        Vector3 position = new Vector3(
            spawnPoint.returnPos().x,
            spawnPoint.returnPos().y,
            spawnPoint.returnPos().z
        );

        spawnedRedPoliceItems.Add(networkRunnerInScene.Spawn(redPoliceItemPrefab, position));

        spawnPoint = lobbyUtils.GetItemSpawnData();
        position = new Vector3(
            spawnPoint.returnPos().x,
            spawnPoint.returnPos().y,
            spawnPoint.returnPos().z
        );

        spawnedBlueRobberItems.Add(networkRunnerInScene.Spawn(blueRobberItemPrefab, position));
    }
}
