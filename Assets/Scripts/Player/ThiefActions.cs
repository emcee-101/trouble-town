using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThiefActions : MonoBehaviour
{
    [SerializeField]
    private int stealAmount = 1000;
    private int currentMoney;
    private int pocketMoney;
    private PlayerUI playerUI;

    private GameObject state;
    private global_money globalMoney;

    // Start is called before the first frame update
    void Start()
    {   
        state = GameObject.FindWithTag("State");
        globalMoney = state.GetComponent<global_money>();
        //totalPocketMoney = globalMoney.TotalPocketMoney;
        currentMoney = 0;
        pocketMoney = 0;
        playerUI = GetComponent<PlayerUI>();
        if(state == null) { Debug.Log("State Object was not found because Niklas is stupid"); }
    }

    // Update is called once per frame
    void Update()
    {
    }
    public bool rubBank()
    {
        state = GameObject.FindWithTag("State");
        globalMoney = state.GetComponent<global_money>();
        // check if cooldown exists
        if (playerUI.isOnStealCooldown()) 
        {
            Debug.Log("Steal Cooldown Running");
            return false;
        }
        int stealAmountThisTime = stealAmount;
        if (globalMoney.GlobalMoney < stealAmount)
        {
            if (globalMoney.GlobalMoney <= 0){
                return false;
            }
            stealAmountThisTime = globalMoney.GlobalMoney;
        }
        if (state != null)
        {
            globalMoney.GlobalMoney      -= stealAmount;
            globalMoney.TotalPocketMoney += stealAmount;
            // add Points
            //scoring scorings = state.GetComponent<scoring>();
            //NetworkPlayer player = GetComponent<NetworkPlayer>();
            //scorings.addRobbingPoints(player.nickName.ToString());
        }
        pocketMoney += stealAmount;
        playerUI.hasJustStolen = true;
        Debug.Log("Stolen " + stealAmountThisTime.ToString());
        return true;

    }
    public bool hideMoney()
    {
        globalMoney.TotalPocketMoney -= pocketMoney;
        currentMoney                 += pocketMoney;
        pocketMoney = 0;
        playerUI.pocketMoneyHidden = true;

        // add Points
        //scoring scorings = state.GetComponent<scoring>();
        //NetworkPlayer player = gameObject.GetComponent<NetworkPlayer>();
        //scorings.addStoringMoneyPoints(player.nickName.ToString());
//
        return true;

    }
    public bool getInvestigated(){
        playerUI.isBeingInvestigated = true;
        
        globalMoney.TotalPocketMoney += pocketMoney;
        pocketMoney = 0;

        // reduce Points
        //scoring scorings = state.GetComponent<scoring>();
        //NetworkPlayer player = state.GetComponent<NetworkPlayer>();
        //scorings.addGettingCaughtPoints(player.nickName.ToString());

        return true;
    }

    public int getPlayerSecuredMoney()
    {
        return currentMoney;
    }
}

