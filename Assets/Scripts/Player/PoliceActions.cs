using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        _np.isBeingInvestigated = true;
        //waiter(_np);

        GetComponent<CharacterInputHandler>().addCatchingRobberPoints();

        return true;
    }
    
    IEnumerator waiter(NetworkPlayer _np)
    {
        //Wait for 5 seconds
        yield return new WaitForSeconds(investigationDuration);
        _np.isBeingInvestigated = false;
    }

}

