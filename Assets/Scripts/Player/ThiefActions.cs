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

    private int stealAmountDefault = 1000;
    private int stealAmountExtraWithBagItem = 500;
    public int currentMoney;
    public int pocketMoney;
    [HideInInspector]
    public float currentStealCooldown = 0.0f;
    private PlayerUI playerUI;
    private NetworkPlayer networkPlayer;

    private GameObject state;
    private global_money globalMoney;
    public bool hasJustStolen;
    public bool pocketMoneyHidden;
    public float stealCooldown;
    public float investigationDuration;
    public float prisonTimeDuration;
    public float wantedStateDuration;
    private PlayerAudio playerAudio;
    // Start is called before the first frame update
    void Start()
    {   
        state = GameObject.FindWithTag("State");
        globalMoney = state.GetComponent<global_money>();
        //totalPocketMoney = globalMoney.TotalPocketMoney;
        currentMoney = 0;
        pocketMoney = 0;
        playerUI = GetComponent<PlayerUI>();
        networkPlayer = GetComponent<NetworkPlayer>();
        playerAudio = GetComponent<PlayerAudio>();
        if(state == null) { Debug.Log("State Object was not found"); }
    }

    // Update is called once per frame
    void Update()
    {
        // Reduce cooldown as time goes...
        if (currentStealCooldown > 0) 
        {
            currentStealCooldown -= Time.deltaTime;
            currentStealCooldown = Mathf.Clamp(currentStealCooldown, 0, 999);
        }

    }

    public bool rubBank()
    {
        int stealAmount = stealAmountDefault;
        if (networkPlayer.hasMoneyBagItem)
        {
            stealAmount += stealAmountExtraWithBagItem;
            networkPlayer.hasMoneyBagItem = false;
        }
        state = GameObject.FindWithTag("State");
        globalMoney = state.GetComponent<global_money>();
        // check if cooldown exists
        if ((currentStealCooldown > 0)) 
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
        currentStealCooldown = stealCooldown;
        playerUI.durationTimerCriminalState = wantedStateDuration;
        
        pocketMoney += stealAmount;
         // Play SFX
        playerAudio.ownAudio.PlayOneShot(playerAudio.stolenTheBank);
        return true;

    }

    public bool hideMoney()
    {   
        if (pocketMoney == 0) {
            return false;
        }
        globalMoney.TotalPocketMoney -= pocketMoney;
        currentMoney                 += pocketMoney;
        pocketMoney = 0;
        pocketMoneyHidden = true;

        // add Points
        GetComponent<CharacterInputHandler>().addStoringMoneyPoints();
        // Play SFX
        playerAudio.ownAudio.PlayOneShot(playerAudio.secureTheMoney);

        return true;
    }
    public bool getInvestigated(){
        
        globalMoney.TotalPocketMoney += pocketMoney;
        pocketMoney = 0;
        // add Points
        GetComponent<CharacterInputHandler>().addGettingCaughtPoints();
        return true;
    }

    public void setCriminal(bool criminalStatus){
        GetComponent<CharacterInputHandler>().criminalStatus = criminalStatus;
    }
}

