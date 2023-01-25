using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMoney : MonoBehaviour
{
    [SerializeField]
    public float currentMoney;
    public float maxMoney;
    public float stealAmount = 1000;
    public float stealCooldown;
    [SerializeField]
    private float currentStealCooldown;
    private float pocketMoney;
    private float lerpTimer;
    private float delayTimer;
    private float chipSpeed = 30f;
    public Image frontBarMoney;
    public Image backBarMoney;
    public TextMeshProUGUI moneyText;
    
    private PlayerUI playerUI;

    // Start is called before the first frame update
    void Start()
    {   
        pocketMoney = 0;
        playerUI = GetComponent<PlayerUI>();
        frontBarMoney.fillAmount = currentMoney / maxMoney;
        backBarMoney.fillAmount = currentMoney / maxMoney;
    }

    // Update is called once per frame
    void Update()
    {
        currentMoney = Mathf.Clamp(currentMoney, 0, maxMoney);
        UpdateMoneyUI();
        UpdateCooldown();
    }
    private void UpdateMoneyUI()
    {
        frontBarMoney.fillAmount = currentMoney / maxMoney;
        backBarMoney.color = Color.white;
        backBarMoney.fillAmount = (currentMoney + pocketMoney) / maxMoney;
        moneyText.text = Mathf.Round(currentMoney) + "+" + Mathf.Round(pocketMoney) + " $";

    }
    private void UpdateCooldown()
    {
        // Reduce cooldown as time goes...
        //Debug.Log(currentStealCooldown);
        if (currentStealCooldown > 0) 
        {

            currentStealCooldown -= Time.deltaTime;
            currentStealCooldown = Mathf.Clamp(currentStealCooldown, 0, 999);

            playerUI.UpdateCooldown(currentStealCooldown.ToString("0"));
        }
    }
    public void deductFunds(float amount)
    {
        currentMoney -= amount;
        lerpTimer = 0f;
    }
    public bool rubBank()
    {
        // check if cooldown exists
        if (currentStealCooldown > 0) 
        {
            Debug.LogFormat("Cooldown Running, Time Left: {0}", currentStealCooldown);
            return false;
        }

        GameObject state = GameObject.FindWithTag("State");
        if (state != null)
        {
            global_money statedata = state.GetComponent<global_money>();
            if (statedata != null)
            {
                statedata.GlobalMoney -= statedata.moneyStolenPerSteal;
            }
        }
        pocketMoney = stealAmount;
        currentStealCooldown = stealCooldown;
        playerUI.isCriminal = true;
        return true;

    }
    public bool hideMoney()
    {
        // check if cooldown exists
        internalReceiveFunds(pocketMoney);
        pocketMoney = 0;
        return true;

    }
    private void internalReceiveFunds(float amount){
        currentMoney += amount;
        lerpTimer = 0f;
    }
}
