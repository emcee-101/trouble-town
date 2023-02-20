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

    public bool hasJustStolen;
    public bool isCriminal;
    public bool isInPrison;
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
        if (state != null)
        {
            GetComponent<CharacterInputHandler>().globalMoneyChange += stealAmount;

            //globalMoney.GlobalMoney      -= stealAmount;
            globalMoney.TotalPocketMoney += stealAmount;
            // add Points
            //scoring scorings = state.GetComponent<scoring>();
            //NetworkPlayer player = GetComponent<NetworkPlayer>();
            //scorings.addRobbingPoints(player.nickName.ToString());
        }
        isCriminal = true;
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
        //scoring scorings = state.GetComponent<scoring>();
        //NetworkPlayer player = gameObject.GetComponent<NetworkPlayer>();
        //scorings.addStoringMoneyPoints(player.nickName.ToString());
//
        return true;

    }
    public bool getInvestigated(){
        
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

