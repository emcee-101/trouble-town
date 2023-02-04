using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class round_spawner : NetworkBehaviour
{
    private round_timer roundTimer;

    public float secondsForItemWave1;

    public BlueRobberItem blueRobberItemPrefab;
    public RedPoliceItem redPoliceItemPrefab;

    UtilLobby lobbyUtils;
    public List<BlueRobberItem> spawnedBlueRobberItems;
    public List<RedPoliceItem> spawnedRedPoliceItems;

    bool itemWave1 = false;

    public void Init()
    {
        roundTimer = FindObjectOfType<round_timer>();
        lobbyUtils = GameObject.FindGameObjectWithTag("State").GetComponent<UtilLobby>();

        secondsForItemWave1 = roundTimer.timeForOneRoundInSeconds - 5;
    }

    public override void FixedUpdateNetwork()
    {
        if (roundTimer.timer.RemainingTime(roundTimer.networkRunnerInScene) < secondsForItemWave1 && !itemWave1)
        {
            itemWave1 = true;
            SpawnWave1();
        }
    }

    private void SpawnWave1()
    {
        Debug.Log("Spawning Wave 1....");
        List<positionData> spawnPoints = lobbyUtils.GetAllItemSpawnData();

        positionData spawnPoint = spawnPoints[0];
        spawnPoints.Remove(spawnPoint);
        Vector3 position = new Vector3(
            spawnPoint.returnPos().x,
            spawnPoint.returnPos().y,
            spawnPoint.returnPos().z
        );

        spawnedRedPoliceItems.Add(roundTimer.networkRunnerInScene.Spawn(redPoliceItemPrefab, position));

        spawnPoint = spawnPoints[0];
        spawnPoints.Remove(spawnPoint);
        position = new Vector3(
            spawnPoint.returnPos().x,
            spawnPoint.returnPos().y,
            spawnPoint.returnPos().z
        );

        spawnedBlueRobberItems.Add(roundTimer.networkRunnerInScene.Spawn(blueRobberItemPrefab, position));
    }
}
