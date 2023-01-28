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



    public positionData GetSpawnData() {

        SpawnPointScript point = GetSpawnPoint();
        positionData data = new positionData(point.place, point.angle);

        return data;
    }
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
        SpawnPointScript resultPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        foreach (var item in spawnPoints)
        {
            Debug.Log("LIST: " + item.place);
        }
        Debug.Log("returned: " + resultPoint.place);
        return resultPoint;
    }
}
