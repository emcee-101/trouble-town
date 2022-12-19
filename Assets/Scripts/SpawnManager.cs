using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] Items;
    public int ItemIndex;
   /* private float spawnRangeX = 45;
    private float spawnPosZ = 45;
    private float startDelay = 2;
    private float spawnInterval = 10;*/


    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("SpawnRandomItem", startDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(Items[ItemIndex], new Vector3(20,0,25),
                Items[ItemIndex].transform.rotation);

        }  
    }

   /* Für das zufällige spawnen später:
    * 
    * void SpawnRandomItem()
    {
        int ItemIndex = Random.Range(0, Items.Length);
        Vector3 spawnPos = new Vector3(Random.Range(-xSpawnRange, xSpawnRange), 0, zSpawnPos);

        Instantiate(Items[ItemIndex], spawnPos,
            Items[ItemIndex].transform.rotation);
    }*/
}
