using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnPointScript : MonoBehaviour
{
    public Vector3 place { get; set; }

    private void Awake()
    {
        place = transform.position;
    }
}
