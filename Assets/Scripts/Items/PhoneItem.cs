using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneItem : NetworkBehaviour
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
            if (roundSpawner.spawnedPhoneItems.Contains(this))
            {
                roundSpawner.spawnedPhoneItems.Remove(this);
                runner.Despawn(GetComponent<NetworkObject>());
                networkPlayer.hasPhoneItem = true;
            }

        }
    }
}
