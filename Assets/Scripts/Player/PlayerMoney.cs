using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMoney : MonoBehaviour
{
    public float currentMoney;
    public float maxMoney;

    private float lerpTimer;
    private float delayTimer;
    private float chipSpeed = 30f;
    public Image frontBarMoney;
    public Image backBarMoney;
    public TextMeshProUGUI moneyText;
    
    // Start is called before the first frame update
    void Start()
    {
        frontBarMoney.fillAmount = currentMoney / maxMoney;
        backBarMoney.fillAmount = currentMoney / maxMoney;
    }

    // Update is called once per frame
    void Update()
    {
        currentMoney = Mathf.Clamp(currentMoney, 0, maxMoney);
        UpdateMoneyUI();
    }
    public void UpdateMoneyUI()
    {
        //Debug.Log(currentMoney);
        float hFraction = currentMoney / maxMoney;
        float fillF = frontBarMoney.fillAmount;
        float fillB = backBarMoney.fillAmount;
        if (fillF < hFraction)
        {
            delayTimer += Time.deltaTime;
            backBarMoney.fillAmount = hFraction;
            if (delayTimer > 0)
            {
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / chipSpeed;
                frontBarMoney.fillAmount = Mathf.Lerp(fillF, backBarMoney.fillAmount, percentComplete);

            }
            //backBarMoney.color = Color.green;
            //lerpTimer += Time.deltaTime;
            //float percentComplete = lerpTimer / chipSpeed;
        }
        if (fillB > hFraction)
        {
            frontBarMoney.fillAmount = hFraction;
            backBarMoney.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backBarMoney.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        moneyText.text = Mathf.Round(currentMoney) + "/" + Mathf.Round(maxMoney) + "$";
    }
    public void deductFunds(float amount)
    {
        currentMoney -= amount;
        lerpTimer = 0f;
    }
    public void receiveFunds(float amount)
    {
        currentMoney += amount;
        lerpTimer = 0f;
    }
}
