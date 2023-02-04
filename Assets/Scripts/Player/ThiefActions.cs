using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThiefActions : MonoBehaviour
{
    [SerializeField]
    public int currentMoney;
    public int maxMoney;
    public int stealAmount = 1000;

    public Image frontBarMoney;
    public Image backBarMoney;
    public TextMeshProUGUI moneyText;
    
    private int pocketMoney;
    private PlayerUI playerUI;

    private GameObject state;

    // Start is called before the first frame update
    void Start()
    {   
        pocketMoney = 0;
        playerUI = GetComponentInParent<PlayerUI>();
        frontBarMoney.fillAmount =  (float)currentMoney / (float)maxMoney;
        backBarMoney.fillAmount =  (float)currentMoney / (float)maxMoney;

        state = GameObject.FindWithTag("State");
        if(state == null) { Debug.Log("State Object was not found because Niklas is stupid"); }
    }

    // Update is called once per frame
    void Update()
    {
        currentMoney = Mathf.Clamp(currentMoney, 0, maxMoney);
        UpdateMoneyUI();
    }
    private void UpdateMoneyUI()
    {
        frontBarMoney.fillAmount = currentMoney / maxMoney;
        backBarMoney.color = Color.white;
        backBarMoney.fillAmount = (currentMoney + pocketMoney) / maxMoney;
        moneyText.text = Mathf.Round(currentMoney) + "+" + Mathf.Round(pocketMoney) + " $";

    }
    
    public bool rubBank()
    {
        // check if cooldown exists
        if (playerUI.isOnStealCooldown()) 
        {
            Debug.Log("Steal Cooldown Running");
            return false;
        }


        if (state != null)
        {
            global_money statedata = state.GetComponent<global_money>();
            statedata.GlobalMoney -= stealAmount;

            // add Points
            scoring scorings = state.GetComponent<scoring>();
            NetworkPlayer player = state.GetComponent<NetworkPlayer>();
            scorings.addRobbingPoints(player.nickName.ToString());

        }
        pocketMoney += stealAmount;
        playerUI.hasRecentlyStolen = true;
        return true;

    }
    public bool hideMoney()
    {
        // check if cooldown exists
        internalReceiveFunds(pocketMoney);
        pocketMoney = 0;
        playerUI.pocketMoneyHidden = true;

        // add Points
        scoring scorings = state.GetComponent<scoring>();
        NetworkPlayer player = state.GetComponent<NetworkPlayer>();
        scorings.addStoringMoneyPoints(player.nickName.ToString());

        return true;

    }
    private void internalReceiveFunds(int amount){
        currentMoney += amount;
    }

    public bool getInvestigated(){
        playerUI.isBeingInvestigated = true;

        pocketMoney = 0;

        // reduce Points
        scoring scorings = state.GetComponent<scoring>();
        NetworkPlayer player = state.GetComponent<NetworkPlayer>();
        scorings.addGettingCaughtPoints(player.nickName.ToString());

        return true;
    }
}

