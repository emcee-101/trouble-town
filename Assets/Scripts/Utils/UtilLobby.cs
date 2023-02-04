using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct positionData
{
    public positionData(Vector3 pos, Quaternion angle)
    {
        rotation = angle;
        place = pos;
    }

    public Vector3 returnPos() { return place; }
    public Quaternion returnAngle() { return rotation; }

    Quaternion rotation { get; set; }
    Vector3 place { get; set; }
}
public class UtilLobby : MonoBehaviour
{
    public positionData GetPlayerSpawnData() {

        PlayerSpawnPointScript point = GetPlayerSpawnPoint();
        positionData data = new positionData(point.place, point.angle);

        return data;
    }
    public Vector3 GetPlayerSpawnLocation()
    {
        PlayerSpawnPointScript point = GetPlayerSpawnPoint();

        return point.place;
    }

    public PlayerSpawnPointScript GetPlayerSpawnPoint()
    {
        PlayerSpawnPointScript[] spawnPoints = UnityEngine.Object.FindObjectsOfType<PlayerSpawnPointScript>();
        // pick random element
        PlayerSpawnPointScript resultPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        return resultPoint;
    }

    public positionData GetItemSpawnData()
    {

        ItemSpawnPointScript point = GetItemSpawnPoint();
        positionData data = new positionData(point.place, point.angle);

        return data;
    }
    public Vector3 GetItemSpawnLocation()
    {
        ItemSpawnPointScript point = GetItemSpawnPoint();

        return point.place;
    }

    public ItemSpawnPointScript GetItemSpawnPoint()
    {
        ItemSpawnPointScript[] spawnPoints = UnityEngine.Object.FindObjectsOfType<ItemSpawnPointScript>();
        // pick random element
        ItemSpawnPointScript resultPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        return resultPoint;
    }
}
