using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Mohammad Zidane
public class PoliceActions : MonoBehaviour
{
    [SerializeField] private float investigationDurationDefault = 5.0f;
    [SerializeField] private float investigationDurationReductionWithPhoneItem = 3.5f;
    private float investigationDuration;

    [SerializeField] private float prisonTimeDurationDefault = 20.0f;
    [SerializeField] private float prisonTimeDurationAdditionWithHandcuffsItem = 15.0f;
    private float prisonTimeDuration;

    private NetworkPlayer netPlayer;

    void Start()
    {
        netPlayer = GetComponent<NetworkPlayer>();
    }

    void Update()
    {

    }

    public bool investigatePlayer(NetworkPlayer _np)
    {
        investigationDuration = investigationDurationDefault;
        if (netPlayer.hasPhoneItem)
        {
            investigationDuration -= investigationDurationReductionWithPhoneItem;
        }

        prisonTimeDuration = prisonTimeDurationDefault;
        if (netPlayer.hasHandcuffsItem)
        {
            prisonTimeDuration += prisonTimeDurationAdditionWithHandcuffsItem;
        }
        StartCoroutine(waiter(_np));
        
        GetComponent<CharacterInputHandler>().addCatchingRobberPoints();

        return true;
    }
    
    IEnumerator waiter(NetworkPlayer _np)
    {
        // Send "isBeingInvestigated" to the remote player
        _np.isBeingInvestigated = true;
        // If police has phone item, it should be consumed at this point
        netPlayer.hasPhoneItem = false;
        GetComponent<CharacterController>().enabled = false;
        //Wait for 5 seconds
        yield return new WaitForSeconds(investigationDuration);
        GetComponent<CharacterController>().enabled = true;
        _np.isBeingInvestigated = false;

        if (_np.isCriminal){
            // If police has Handcuffs item, it should be consumed at this point
            netPlayer.hasHandcuffsItem = false;

            _np.supposedToGoToPrison = true;
            _np.prisonTimeDuration = prisonTimeDuration;
            yield return new WaitForSeconds(prisonTimeDuration);
            _np.supposedToGoToPrison = false;
        }   
    }
}

