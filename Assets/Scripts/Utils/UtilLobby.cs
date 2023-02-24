using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

public enum spawnType
{
    PRISON,
    LOBBY,
    GAME
}
public class UtilLobby : MonoBehaviour
{
   

    public positionData GetPlayerSpawnData(spawnType type)
    {

        IPlayerSpawnPointScript[] spawnPoints;

        if (type == spawnType.LOBBY) { spawnPoints = (IPlayerSpawnPointScript[]) UnityEngine.Object.FindObjectsOfType<PlayerSpawnPointScriptForLobby>(); }
        else if(type == spawnType.GAME) { spawnPoints = (IPlayerSpawnPointScript[]) UnityEngine.Object.FindObjectsOfType<PlayerSpawnPointScriptForGame>(); }
        else if(type == spawnType.PRISON) { spawnPoints = (IPlayerSpawnPointScript[])UnityEngine.Object.FindObjectsOfType<PlayerSpawnPointScriptForPrison>(); }

        // default actíon
        else { spawnPoints = (IPlayerSpawnPointScript[])UnityEngine.Object.FindObjectsOfType<PlayerSpawnPointScriptForGame>(); }

        // pick random element
        IPlayerSpawnPointScript resultPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

        positionData data = new positionData(resultPoint.place, resultPoint.angle);

        return data;
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

    public List<positionData> GetAllItemSpawnData()
    {
        ItemSpawnPointScript[] points = GetItemSpawnPoints();
        List<positionData> data = new List<positionData>();

        foreach (ItemSpawnPointScript point in points)
        {
            data.Add(new positionData(point.place, point.angle));
        }

        return data;
    }
    public ItemSpawnPointScript[] GetItemSpawnPoints()
    {
        ItemSpawnPointScript[] spawnPoints = FindObjectsOfType<ItemSpawnPointScript>();
        return spawnPoints;
    }
}
