using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPoliceItem : NetworkBehaviour
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

        if (networkPlayer != null && networkPlayer.isHostAndPolice)
        {
            if (roundSpawner.spawnedRedPoliceItems.Contains(this))
            {
                roundSpawner.spawnedRedPoliceItems.Remove(this);
                runner.Despawn(GetComponent<NetworkObject>());
                networkPlayer.hasRedPoliceItem = true;
            }
            
        }
    }
}
