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

    // Start is called before the first frame update
    void Start()
    {   
        pocketMoney = 0;
        playerUI = GetComponentInParent<PlayerUI>();
        frontBarMoney.fillAmount =  (float)currentMoney / (float)maxMoney;
        backBarMoney.fillAmount =  (float)currentMoney / (float)maxMoney;
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

        GameObject state = GameObject.FindWithTag("State");
        if (state != null)
        {
            global_money statedata = state.GetComponent<global_money>();
            if (statedata != null)
            {
                statedata.GlobalMoney -= stealAmount;
            }
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
        return true;

    }
    private void internalReceiveFunds(int amount){
        currentMoney += amount;
    }

    public bool getInvestigated(){
        playerUI.isBeingInvestigated = true;

         GameObject state = GameObject.FindWithTag("State");
        if (state != null)
        {
            global_money statedata = state.GetComponent<global_money>();
            if (statedata != null)
            {
                statedata.GlobalMoney += pocketMoney;
            }
        }

        return true;
    }
}

