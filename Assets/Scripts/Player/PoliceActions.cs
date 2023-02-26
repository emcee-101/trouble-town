using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Mohammad Zidane
public class PoliceActions : MonoBehaviour
{
    public float investigationDurationDefault = 5.0f;
    public float investigationDurationDifferenceWithPhoneItem = 2.5f;

    public float prisonTimeDefault = 20.0f;
    public float prisonTimeBuffWithHandcuffsItem = 10.0f;

    private NetworkPlayer ownNetworkPlayer;

    void Start()
    {
        ownNetworkPlayer = GetComponent<NetworkPlayer>();
    }

    public bool investigatePlayer(NetworkPlayer _np)
    {
        
        StartCoroutine(waiter(_np));
        
        GetComponent<CharacterInputHandler>().addCatchingRobberPoints();

        return true;
    }
    
    IEnumerator waiter(NetworkPlayer _np)
    {
        float investigationDuration = investigationDurationDefault;
        if (ownNetworkPlayer.hasPhoneItem)
        {
            investigationDuration -= investigationDurationDifferenceWithPhoneItem;
            ownNetworkPlayer.hasPhoneItem = false;
        }

        float prisonTime = prisonTimeDefault;
        if (ownNetworkPlayer.hasHandcuffsItem)
        {
            prisonTime += prisonTimeBuffWithHandcuffsItem;
            ownNetworkPlayer.hasHandcuffsItem = false;
        }

        _np.isBeingInvestigated = true;

        GetComponent<CharacterController>().enabled = false;
        //Wait for 5 seconds
        yield return new WaitForSeconds(investigationDuration);
        GetComponent<CharacterController>().enabled = true;
        _np.isBeingInvestigated = false;

        if (_np.isCriminal){
            _np.supposedToGoToPrison = true;
            yield return new WaitForSeconds(prisonTime);
            _np.supposedToGoToPrison = false;
        }   
    }
}

