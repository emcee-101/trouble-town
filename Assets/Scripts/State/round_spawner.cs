using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class round_spawner : NetworkBehaviour
{
    private round_timer roundTimer;

    public float secondsForItemWave1;

    public MoneyBagItem MoneyBagItemPrefab;
    public PhoneItem phoneItemPrefab;
    public SpeedBoostItem speedBoostItemPrefab;

    UtilLobby lobbyUtils;
    public List<MoneyBagItem> spawnedMoneyBagItems;
    public List<PhoneItem> spawnedPhoneItems;
    public List<SpeedBoostItem> spawnedSpeedBoostItems;

    bool itemWave1 = false;

    public void Init()
    {
        roundTimer = gameObject.GetComponent<round_timer>();
        lobbyUtils = gameObject.GetComponent<UtilLobby>();

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

        spawnedMoneyBagItems.Add(roundTimer.networkRunnerInScene.Spawn(MoneyBagItemPrefab, position));

        spawnPoint = spawnPoints[0];
        spawnPoints.Remove(spawnPoint);
        position = new Vector3(
            spawnPoint.returnPos().x,
            spawnPoint.returnPos().y,
            spawnPoint.returnPos().z
        );

        spawnedPhoneItems.Add(roundTimer.networkRunnerInScene.Spawn(phoneItemPrefab, position));

        spawnPoint = spawnPoints[0];
        spawnPoints.Remove(spawnPoint);
        position = new Vector3(
            spawnPoint.returnPos().x,
            spawnPoint.returnPos().y,
            spawnPoint.returnPos().z
        );

        spawnedSpeedBoostItems.Add(roundTimer.networkRunnerInScene.Spawn(speedBoostItemPrefab, position));
    }
}
