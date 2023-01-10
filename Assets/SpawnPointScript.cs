using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class SpawnPointScript : MonoBehaviour
{
    public Vector3 place { get; set; }

    private void Awake()
    {
        place = transform.position;
    }
}
