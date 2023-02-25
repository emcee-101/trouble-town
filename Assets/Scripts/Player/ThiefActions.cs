using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

// Author: Mohammad Zidane
public class ThiefActions : MonoBehaviour
{
    [SerializeField]
    private int stealAmount = 1000;
    private int currentMoney;
    private int pocketMoney;
    private PlayerUI playerUI;

    private GameObject state;
    private global_money globalMoney;
    public bool hasJustStolen;
    public bool pocketMoneyHidden;
    public float stealCooldown;
    public float investigationDuration;
    public float prisonTimeDuration;
    public float wantedStateDuration;
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
        CharacterInputHandler handler = GetComponent<CharacterInputHandler>();
        handler.globalMoneyChange -= stealAmount;
        handler.globalPocketMoneyChange += stealAmount;

        // add Points
        handler.addRobbingPoints();
        setCriminal(true);
        pocketMoneyHidden = false;
        playerUI.durationTimerStealCooldown = stealCooldown;
        playerUI.durationTimerCriminalState = wantedStateDuration;
        
        pocketMoney += stealAmount;
        Debug.Log("Stolen " + stealAmountThisTime.ToString());
        return true;

    }
    public bool hideMoney()
    {
        globalMoney.TotalPocketMoney -= pocketMoney;
        currentMoney                 += pocketMoney;
        pocketMoney = 0;
        pocketMoneyHidden = true;

        // add Points
        GetComponent<CharacterInputHandler>().addStoringMoneyPoints();
        
        return true;

    }
    public bool getInvestigated(){
        
        globalMoney.TotalPocketMoney += pocketMoney;
        pocketMoney = 0;
        // add Points
        GetComponent<CharacterInputHandler>().addGettingCaughtPoints();
        return true;
    }

    public int getPlayerSecuredMoney()
    {
        return currentMoney;
    }
    public void setCriminal(bool criminalStatus){
        GetComponent<CharacterInputHandler>().criminalStatus = criminalStatus;
    }
}

