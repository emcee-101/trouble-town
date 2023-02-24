using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPointScriptForGame : MonoBehaviour, IPlayerSpawnPointScript
{
    public Vector3 place { get; set; }
    public Quaternion angle { get; set; }

    private void Awake()
    {
        place = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        angle = transform.rotation;
    }
}
