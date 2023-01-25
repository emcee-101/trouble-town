using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;
    [SerializeField]
    private TextMeshProUGUI cooldownText;
    public bool isCriminal;
    public bool pocketMoneyHidden;

    [Header("Intense Overlay")]
    [SerializeField]
    public Image intenseOverlay;
    public TextMeshProUGUI criminalIndicator;
    public float duration;
    public float fadeSpeed;
    public float wantedStateDuration;

    private float durationTimer;
    private float durationTimerCriminalState;
        // Start is called before the first frame update 
    void Start()
    {
        durationTimerCriminalState = 0;
        intenseOverlay.color = new Color(intenseOverlay.color.r, intenseOverlay.color.g, intenseOverlay.color.b, 0.6f);
        cooldownText.text = "Current Cooldown: ";
    }

    // Update is called once per frame
    void Update()
    {
        if (isCriminal) {
            intenseOverlay.enabled = true;
            criminalIndicator.enabled = true;
            UpdateIntenseOverlay();
        }
        else
        {
            criminalIndicator.enabled = false;
            intenseOverlay.enabled = false;
        }
        UpdateCriminalStatus();
    }

    public void UpdateIntenseOverlay()
    {
        float factor = Time.deltaTime * fadeSpeed;
        durationTimer += Time.deltaTime;
        if (duration > durationTimer)
        {
            float tempAlpha = intenseOverlay.color.a;
            tempAlpha -= factor;
            intenseOverlay.color = new Color(intenseOverlay.color.r, intenseOverlay.color.g, intenseOverlay.color.b, tempAlpha);
            
            criminalIndicator.transform.localScale += new Vector3(0.1f * factor, 0.1f * factor, 0.1f * factor);

        }
        if (durationTimer > duration)
        {
            float tempAlpha = intenseOverlay.color.a;
            tempAlpha += factor;
            intenseOverlay.color = new Color(intenseOverlay.color.r, intenseOverlay.color.g, intenseOverlay.color.b, tempAlpha);

            criminalIndicator.transform.localScale -= new Vector3(0.1f * factor, 0.1f * factor, 0.1f * factor);
        }
        if (durationTimer > duration * 2)
        {
            durationTimer = 0;
        }

    }
    public void UpdateCriminalStatus(){
        if (pocketMoneyHidden && isCriminal)
        {
            durationTimerCriminalState += Time.deltaTime;
            if (durationTimerCriminalState > wantedStateDuration){
                isCriminal = false;
                durationTimerCriminalState = 0;
            }
        }
    }

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
    public void UpdateCooldown(string timer)
    {
        cooldownText.text = cooldownText.text = "Current Cooldown: " + timer;
    }

}
