using Fusion;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class round_spawner : NetworkBehaviour
{
    private round_timer roundTimer;

    public float secondsForItemWave1AfterRoundStart;
    public float secondsForItemWave2AfterRoundStart;
    public float secondsForItemWave3AfterRoundStart;
    public float secondsForItemWave4AfterRoundStart;
    public float secondsForItemWave5AfterRoundStart;

    private float secondsForItemWave1;
    private float secondsForItemWave2;
    private float secondsForItemWave3;
    private float secondsForItemWave4;
    private float secondsForItemWave5;

    public MoneyBagItem moneyBagItemPrefab;
    public CrowbarItem crowbarItemPrefab;
    public HandcuffsItem handcuffsItemPrefab;
    public PhoneItem phoneItemPrefab;
    public SpeedBoostItem speedBoostItemPrefab;

    UtilLobby lobbyUtils;
    public List<MoneyBagItem> spawnedMoneyBagItems;
    public List<CrowbarItem> spawnedCrowbarItems;
    public List<HandcuffsItem> spawnedHandcuffsItems;
    public List<PhoneItem> spawnedPhoneItems;
    public List<SpeedBoostItem> spawnedSpeedBoostItems;

    private bool itemWave1 = false;
    private bool itemWave2 = false;
    private bool itemWave3 = false;
    private bool itemWave4 = false;
    private bool itemWave5 = false;

    public void Init()
    {
        roundTimer = gameObject.GetComponent<round_timer>();
        lobbyUtils = gameObject.GetComponent<UtilLobby>();

        secondsForItemWave1 = roundTimer.timeForOneRoundInSeconds - secondsForItemWave1AfterRoundStart;
        secondsForItemWave2 = roundTimer.timeForOneRoundInSeconds - secondsForItemWave2AfterRoundStart;
        secondsForItemWave3 = roundTimer.timeForOneRoundInSeconds - secondsForItemWave3AfterRoundStart;
        secondsForItemWave4 = roundTimer.timeForOneRoundInSeconds - secondsForItemWave4AfterRoundStart;
        secondsForItemWave5 = roundTimer.timeForOneRoundInSeconds - secondsForItemWave5AfterRoundStart;
    }

    public override void FixedUpdateNetwork()
    {
        if (roundTimer.timer.RemainingTime(roundTimer.networkRunnerInScene) < secondsForItemWave1 && !itemWave1)
        {
            itemWave1 = true;
            SpawnWave(1, 1, 1, 1, 1);
        }

        if (roundTimer.timer.RemainingTime(roundTimer.networkRunnerInScene) < secondsForItemWave2 && !itemWave2)
        {
            itemWave2 = true;
            SpawnWave(1, 1, 1, 1, 1);
        }

        if (roundTimer.timer.RemainingTime(roundTimer.networkRunnerInScene) < secondsForItemWave3 && !itemWave3)
        {
            itemWave3 = true;
            SpawnWave(1, 1, 1, 1, 1);
        }

        if (roundTimer.timer.RemainingTime(roundTimer.networkRunnerInScene) < secondsForItemWave4 && !itemWave4)
        {
            itemWave4 = true;
            SpawnWave(1, 1, 1, 1, 1);
        }

        if (roundTimer.timer.RemainingTime(roundTimer.networkRunnerInScene) < secondsForItemWave5 && !itemWave5)
        {
            itemWave5 = true;
            SpawnWave(1, 1, 1, 1, 1);
        }
    }

    public void DespawnItems()
    {
        foreach (MoneyBagItem item in spawnedMoneyBagItems)
        {
            item.Despawn();
        }
        spawnedMoneyBagItems.Clear();

        foreach (CrowbarItem item in spawnedCrowbarItems)
        {
            item.Despawn();
        }
        spawnedCrowbarItems.Clear();

        foreach (HandcuffsItem item in spawnedHandcuffsItems)
        {
            item.Despawn();
        }
        spawnedHandcuffsItems.Clear();

        foreach (SpeedBoostItem item in spawnedSpeedBoostItems)
        {
            item.Despawn();
        }
        spawnedSpeedBoostItems.Clear();

        foreach (PhoneItem item in spawnedPhoneItems)
        {
            item.Despawn();
        }
        spawnedPhoneItems.Clear();
    }

    private void SpawnWave(int moneyBagAmount, int crowbarAmount, int handcuffsAmount, int phoneAmount, int speedBoostAmount)
    {
        DespawnItems();
                      
        System.Random rand = new System.Random();
        List<positionData> shuffledSpawnPoints = lobbyUtils.GetAllItemSpawnData().OrderBy(_ => rand.Next()).ToList();

        positionData spawnPoint;
        Vector3 position;

        for (int i = 0; i < moneyBagAmount; i++)
        {
            if (shuffledSpawnPoints.Count >= 1)
            {
                spawnPoint = shuffledSpawnPoints[0];
                shuffledSpawnPoints.Remove(spawnPoint);
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

        for (int i = 0; i < crowbarAmount; i++)
        {
            if (shuffledSpawnPoints.Count >= 1)
            {
                spawnPoint = shuffledSpawnPoints[0];
                shuffledSpawnPoints.Remove(spawnPoint);
                position = new Vector3(
                    spawnPoint.returnPos().x,
                    spawnPoint.returnPos().y,
                    spawnPoint.returnPos().z
                );

                spawnedCrowbarItems.Add(roundTimer.networkRunnerInScene.Spawn(crowbarItemPrefab, position));
            }
            else
                Debug.LogWarning("No ItemSpawnPoint left to Spawn CrowbarItem");
        }

        for (int i = 0; i < handcuffsAmount; i++)
        {
            if (shuffledSpawnPoints.Count >= 1)
            {
                spawnPoint = shuffledSpawnPoints[0];
                shuffledSpawnPoints.Remove(spawnPoint);
                position = new Vector3(
                    spawnPoint.returnPos().x,
                    spawnPoint.returnPos().y,
                    spawnPoint.returnPos().z
                );

                spawnedHandcuffsItems.Add(roundTimer.networkRunnerInScene.Spawn(handcuffsItemPrefab, position));
            }
            else
                Debug.LogWarning("No ItemSpawnPoint left to Spawn HandcuffsItem");
        }

        for (int i = 0; i < phoneAmount; i++)
        {
            if (shuffledSpawnPoints.Count >= 1)
            {
                spawnPoint = shuffledSpawnPoints[0];
                shuffledSpawnPoints.Remove(spawnPoint);
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
            if (shuffledSpawnPoints.Count >= 1)
            {
                spawnPoint = shuffledSpawnPoints[0];
                shuffledSpawnPoints.Remove(spawnPoint);
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
