using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandcuffsItem : NetworkBehaviour
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
            if (roundSpawner.spawnedHandcuffsItems.Contains(this))
            {
                Despawn();
                roundSpawner.spawnedHandcuffsItems.Remove(this);
                networkPlayer.hasHandcuffsItem = true;
            }
        }
    }

    public void Despawn()
    {
        runner.Despawn(GetComponent<NetworkObject>());
    }
}
