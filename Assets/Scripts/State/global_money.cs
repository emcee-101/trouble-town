using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class global_money : NetworkBehaviour
{
    // https://doc.photonengine.com/en-us/fusion/current/manual/network-object/network-behaviour#networked_state

    public Boolean logStateAuthority = false;
    [Networked(OnChanged = nameof(onMoneyChanged))] public int GlobalMoney { get; set; } = 10000;
    [Networked(OnChanged = nameof(onMoneyChanged))] public int TotalPocketMoney { get; set; } = 0;


    public static void onMoneyChanged(Changed<global_money> changed)
    {
        changed.Behaviour.onMoneyChanged();
    }

    public void onMoneyChanged()
    {
        // Check if in Bounds -> if < 0 : Round ended | else : okay
        if (GlobalMoney <= 0 && TotalPocketMoney <= 0)
        {
            game_state game_manager = GetComponent<game_state>();
            Log.Info("Game has ended due to the Bank being robbed empty - the robbers win!!");
            game_manager.gameState = GameState.aftergame;
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
