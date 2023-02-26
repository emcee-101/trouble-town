using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

// Author: Mohammad Zidane
public class ThiefActions : MonoBehaviour
{
    [SerializeField] private int stealAmountDefault = 1000;
    [SerializeField] private int stealAmountExtraWithBagItem = 500;
    public int currentMoney;
    public int pocketMoney;
    
    [HideInInspector] public float currentStealCooldown = 0.0f;
    [HideInInspector] public float currentTimerCriminalState;

    private PlayerUI playerUI;
    private NetworkPlayer netPlayer;

    private GameObject state;
    private global_money globalMoney;
    public bool pocketMoneyHidden;
    public float stealCooldown;
    public float investigationDuration;
    public float prisonTimeDuration;
    public float criminalStateDurationAfterMoneyIsHidden;

    [SerializeField]
    private float speedBoostDuration = 20.0f;
    private float currentSpeedBoostDuration = 0.0f;

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
        netPlayer = GetComponent<NetworkPlayer>();
        playerAudio = GetComponent<PlayerAudio>();
        if(state == null) { Debug.Log("State Object was not found"); }
    }

    // Update is called once per frame
    void Update()
    {
        // Reduce steal cooldown as time goes.
        if (currentStealCooldown > 0) 
        {
            currentStealCooldown -= Time.deltaTime;
            currentStealCooldown = Mathf.Clamp(currentStealCooldown, 0, 999);
        }

        // Reduce criminal state cooldown after the stolen money is hidden, and
        // disable criminal status after the cooldown time has passed
        if (netPlayer.isCriminal && pocketMoneyHidden)
        {
            currentTimerCriminalState -= Time.deltaTime;
            if (currentTimerCriminalState <= 0){
                setCriminal(false);
            }
        }

        if (netPlayer.hasSpeedBoostItem)
        {
            NetworkCharacterControllerPrototypeCustom nCCPC = GetComponent<NetworkCharacterControllerPrototypeCustom>();
            nCCPC.maxSpeed += 5.0f;
            currentSpeedBoostDuration += Time.deltaTime;
            if (currentSpeedBoostDuration > speedBoostDuration){
                currentSpeedBoostDuration = 0.0f;
                nCCPC.maxSpeed -= 5.0f;
                netPlayer.hasSpeedBoostItem = false;
            }
        }

    }

    public bool rubBank()
    {
        // check if cooldown exists
        if ((currentStealCooldown > 0)) 
        {
            Debug.Log("Steal Cooldown Running");
            return false;
        }

        int stealAmountThisTime = stealAmountDefault;
        if (netPlayer.hasMoneyBagItem)
        {
            stealAmountThisTime += stealAmountExtraWithBagItem;
            netPlayer.hasMoneyBagItem = false;
        }
        state = GameObject.FindWithTag("State");
        globalMoney = state.GetComponent<global_money>();
        
        if (globalMoney.GlobalMoney < stealAmountThisTime)
        {
            if (globalMoney.GlobalMoney <= 0){
                return false;
            }
            stealAmountThisTime = globalMoney.GlobalMoney;
        }
        CharacterInputHandler handler = GetComponent<CharacterInputHandler>();
        handler.globalPocketMoneyChange += stealAmountThisTime;
        handler.globalMoneyChange -= stealAmountThisTime;

        // add Points
        handler.addRobbingPoints();
        setCriminal(true);
        pocketMoneyHidden = false;
        currentStealCooldown = stealCooldown;
        currentTimerCriminalState = criminalStateDurationAfterMoneyIsHidden;
        
        pocketMoney += stealAmountThisTime;
         // Play SFX
        playerAudio.ownAudio.PlayOneShot(playerAudio.stolenTheBank);
        return true;

    }

    public bool hideMoney()
    {   
        if (pocketMoney == 0) {
            return false;
        }

        CharacterInputHandler handler = GetComponent<CharacterInputHandler>();
        handler.globalPocketMoneyChange -= pocketMoney;

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
        
        CharacterInputHandler handler = GetComponent<CharacterInputHandler>();
        handler.globalMoneyChange += pocketMoney;
        handler.globalPocketMoneyChange -= pocketMoney;

        pocketMoney = 0;
        // add Points
        GetComponent<CharacterInputHandler>().addGettingCaughtPoints();
        return true;
    }

    public void setCriminal(bool criminalStatus){
        GetComponent<CharacterInputHandler>().criminalStatus = criminalStatus;
    }
}

