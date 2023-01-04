using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : Interactable
{
  
    protected override void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);

        GameObject state = GameObject.FindWithTag("State");

        if (state != null)
        {
            global_money statedata = state.GetComponent<global_money>();
            if (statedata != null)
            {

                statedata.GlobalMoney -= statedata.moneyStolenPerSteal;

            }
        }

    }
}
