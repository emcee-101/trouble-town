using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilLobby : MonoBehaviour
{
    public Vector3 GetSpawnLocation()
    {
        SpawnPointScript point = GetSpawnPoint();

        return point.place;
    }

    public SpawnPointScript GetSpawnPoint()
    {
        SpawnPointScript[] spawnPoints = UnityEngine.Object.FindObjectsOfType<SpawnPointScript>();
        Debug.Log($"Found {spawnPoints.Length} SpawnPoints");
        // pick random element
        var element = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

        return element;
    }
}
