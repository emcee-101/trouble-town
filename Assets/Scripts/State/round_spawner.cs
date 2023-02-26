using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class round_spawner : NetworkBehaviour
{
    private round_timer roundTimer;

    public float secondsForItemWave1;
    public float secondsForItemWave2;
    public float secondsForItemWave3;
    public float secondsForItemWave4;
    public float secondsForItemWave5;

    public MoneyBagItem moneyBagItemPrefab;
    public PhoneItem phoneItemPrefab;
    public SpeedBoostItem speedBoostItemPrefab;

    UtilLobby lobbyUtils;
    public List<MoneyBagItem> spawnedMoneyBagItems;
    public List<PhoneItem> spawnedPhoneItems;
    public List<SpeedBoostItem> spawnedSpeedBoostItems;

    bool itemWave1 = false;
    bool itemWave2 = false;
    bool itemWave3 = false;
    bool itemWave4 = false;
    bool itemWave5 = false;

    public void Init()
    {
        roundTimer = gameObject.GetComponent<round_timer>();
        lobbyUtils = gameObject.GetComponent<UtilLobby>();

        secondsForItemWave1 = roundTimer.timeForOneRoundInSeconds - 30;
        secondsForItemWave2 = roundTimer.timeForOneRoundInSeconds - 150;
        secondsForItemWave3 = roundTimer.timeForOneRoundInSeconds - 200;
        secondsForItemWave4 = roundTimer.timeForOneRoundInSeconds - 250;
        secondsForItemWave5 = roundTimer.timeForOneRoundInSeconds - 300;
    }

    public override void FixedUpdateNetwork()
    {
        if (roundTimer.timer.RemainingTime(roundTimer.networkRunnerInScene) < secondsForItemWave1 && !itemWave1)
        {
            itemWave1 = true;
            SpawnWave(1, 1, 1);
        }

        if (roundTimer.timer.RemainingTime(roundTimer.networkRunnerInScene) < secondsForItemWave1 && !itemWave1)
        {
            itemWave2 = true;
            SpawnWave(1, 1, 1);
        }

        if (roundTimer.timer.RemainingTime(roundTimer.networkRunnerInScene) < secondsForItemWave1 && !itemWave1)
        {
            itemWave3 = true;
            SpawnWave(1, 1, 1);
        }

        if (roundTimer.timer.RemainingTime(roundTimer.networkRunnerInScene) < secondsForItemWave1 && !itemWave1)
        {
            itemWave4 = true;
            SpawnWave(1, 1, 1);
        }

        if (roundTimer.timer.RemainingTime(roundTimer.networkRunnerInScene) < secondsForItemWave1 && !itemWave1)
        {
            itemWave5 = true;
            SpawnWave(1, 1, 1);
        }
    }

    public void DespawnItems()
    {
        foreach (MoneyBagItem item in spawnedMoneyBagItems)
        {
            item.Despawn();
        }
        foreach (SpeedBoostItem item in spawnedSpeedBoostItems)
        {
            item.Despawn();
        }
        foreach (PhoneItem item in spawnedPhoneItems)
        {
            item.Despawn();
        }
    }

    private void SpawnWave(int moneyBagAmount, int phoneAmount, int speedBoostAmount)
    {
        DespawnItems();

        Debug.Log("Spawning Wave ....");
        
        List<positionData> spawnPoints = lobbyUtils.GetAllItemSpawnData();
        positionData spawnPoint;
        Vector3 position;

        for (int i = 0; i < moneyBagAmount; i++)
        {
            if (spawnPoints.Count >= 1)
            {
                spawnPoint = spawnPoints[0];
                spawnPoints.Remove(spawnPoint);
                position = new Vector3(
                    spawnPoint.returnPos().x,
                    spawnPoint.returnPos().y,
                    spawnPoint.returnPos().z
                );

                spawnedMoneyBagItems.Add(roundTimer.networkRunnerInScene.Spawn(moneyBagItemPrefab, position));
            }
            else
                Debug.LogWarning("No ItemSpawnPoint left to Spawn MoneyBagItem");
        }

        for (int i = 0; i < phoneAmount; i++)
        {
            if (spawnPoints.Count >= 1)
            {
                spawnPoint = spawnPoints[0];
                spawnPoints.Remove(spawnPoint);
                position = new Vector3(
                    spawnPoint.returnPos().x,
                    spawnPoint.returnPos().y,
                    spawnPoint.returnPos().z
                );

                spawnedPhoneItems.Add(roundTimer.networkRunnerInScene.Spawn(phoneItemPrefab, position));
            }
            else
                Debug.LogWarning("No ItemSpawnPoint left to Spawn PhoneItem");
        }

        for (int i = 0; i < speedBoostAmount; i++)
        {
            if (spawnPoints.Count >= 1)
            {
                spawnPoint = spawnPoints[0];
                spawnPoints.Remove(spawnPoint);
                position = new Vector3(
                    spawnPoint.returnPos().x,
                    spawnPoint.returnPos().y,
                    spawnPoint.returnPos().z
                );

                spawnedSpeedBoostItems.Add(roundTimer.networkRunnerInScene.Spawn(speedBoostItemPrefab, position));
            }
            else
                Debug.LogWarning("No ItemSpawnPoint left to Spawn SpeedBoostItem");
        }
    }
}
