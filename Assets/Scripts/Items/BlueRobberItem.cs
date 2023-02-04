using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueRobberItem : NetworkBehaviour
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

        if (networkPlayer != null && !networkPlayer.isHostAndPolice)
        {
            if (roundSpawner.spawnedBlueRobberItems.Contains(this))
            {
                roundSpawner.spawnedBlueRobberItems.Remove(this);
                runner.Despawn(GetComponent<NetworkObject>());
                networkPlayer.hasBlueRobberItem = true;
            }
        }
    }
}
