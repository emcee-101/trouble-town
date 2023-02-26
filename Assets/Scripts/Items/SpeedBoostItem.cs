using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostItem : NetworkBehaviour
{
    private round_spawner roundSpawner;
    private NetworkRunner runner;

    private void Start()
    {
        roundSpawner = FindObjectOfType<round_spawner>();
        runner = FindObjectOfType<NetworkRunner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        NetworkPlayer networkPlayer = other.GetComponent<NetworkPlayer>();

        if (networkPlayer != null)
        {
            if (roundSpawner.spawnedSpeedBoostItems.Contains(this))
            {
                Despawn();
                networkPlayer.hasSpeedBoostItem = true;
            }
        }
    }

    public void Despawn()
    {
        roundSpawner.spawnedSpeedBoostItems.Remove(this);
        runner.Despawn(GetComponent<NetworkObject>());
    }
}
