using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class global_money : NetworkBehaviour
{
    // https://doc.photonengine.com/en-us/fusion/current/manual/network-object/network-behaviour#networked_state

    public Boolean logStateAuthority = false;
    public int moneyStolenPerSteal = 1000;
    [Networked(OnChanged = nameof(onGlobalMoneyChanged))] public int GlobalMoney { get; set; } = 10000;


    public static void onGlobalMoneyChanged(Changed<global_money> changed)
    {

        changed.Behaviour.onGlobalMoneyChanged();

    }

    public void onGlobalMoneyChanged()
    {
        Log.Info("Money still there: " + GlobalMoney);

        // Check if in Bounds -> if < 0 : Round ended | else : okay
        if (GlobalMoney <= 0)
        {
            Log.Info("round has ended officially");
        }
        else {
            Log.Info("Theres still Money at the Bank!");
        }
    }

    // Once per time the Object gets Updated (synchronised once every Server-Tick)
    override public void FixedUpdateNetwork()
    {
        

        if (logStateAuthority) {

            NetworkObject netObj = gameObject.GetComponent<NetworkObject>();

            if (netObj.HasStateAuthority)
            {

                Log.Info("The current StateAuthority is this one - global Money is: " + this.GlobalMoney);


            }
            else Log.Info("not the StateAuthority :(");
        }


    }

    //override Spawned()
    //override Despawned(bool hasState)
    //override Render()

}
