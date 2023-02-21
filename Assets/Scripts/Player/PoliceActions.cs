using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Mohammad Zidane
public class PoliceActions : MonoBehaviour
{
    public float investigationDuration;
    // Start is called before the first frame update
    void Start()
    {
        investigationDuration = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool investigatePlayer(NetworkPlayer _np)
    {
        
        StartCoroutine(waiter(_np));
        
        GetComponent<CharacterInputHandler>().addCatchingRobberPoints();

        return true;
    }
    
    IEnumerator waiter(NetworkPlayer _np)
    {
        _np.isBeingInvestigated = true;
        GetComponent<CharacterController>().enabled = false;
        //Wait for 5 seconds
        yield return new WaitForSeconds(investigationDuration);
        GetComponent<CharacterController>().enabled = true;
        _np.isBeingInvestigated = false;
    }

}

