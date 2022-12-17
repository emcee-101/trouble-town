using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundariesDestroyed : MonoBehaviour{ 
    //falls Map erweitert Werte anpassen

    private float topBound =100;
    private float lowerBound =-6;
    private float leftBound = -56;
    private float rightBound =104;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
            if (transform.position.x > topBound)
            {
                Destroy(gameObject);
            }
            else if (transform.position.x < lowerBound)
            {
                 Destroy(gameObject);
            }
        else if (transform.position.z < leftBound)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z < rightBound)
        {
            Destroy(gameObject);
        }

    }
}
