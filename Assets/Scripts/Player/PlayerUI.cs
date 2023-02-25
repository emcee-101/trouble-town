using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Author: Mohammad Zidane
public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public TextMeshProUGUI cooldownText;
    public Animator fadeAnimator;
    public bool animatorRunning;

    public float fadeSceneSwitchDuration;
    public float animationDuration = 2.0f;
    public float moneyTotal;
    public float moneyLeft;
    public float totalPocketMoney;

    [Header("Intense Overlay")]
    public Image intenseOverlay;
    public TextMeshProUGUI warnMessage;

    public Image frontBarMoney;
    public Image backBarMoney;
    public TextMeshProUGUI moneyText;

    private NetworkPlayer netPlayer;
    private ThiefActions thiefActions;

    private GameObject state;
    private global_money globalMoney;

    public AudioSource ownAudio;
    public AudioClip catchedByPolice;

    [Header("LobbyUI")]
    [SerializeField]
    public TextMeshProUGUI playerCountText;

    public void Init()
    {
        durationTimerTransistionOnHold = 2.0f;
        durationTimerSceneTransition = fadeSceneSwitchDuration;
        moneyTotal = globalMoney.GlobalMoney;
        frontBarMoney.fillAmount =  1f;
        backBarMoney.fillAmount  =  1f;
        warnMessage.enabled = true;
        durationTimerStealCooldown = 0;
        intenseOverlay.color = new Color(intenseOverlay.color.r, intenseOverlay.color.g, intenseOverlay.color.b, 0.6f);
    }
    
    private float fadeSpeed = 0.5f;
    private float durationTimerTransistionOnHold;
    private float durationTimerSceneTransition;
    private float durationTimerAnimation = 0;

    public  float durationTimerStealCooldown;
    public float durationTimerCriminalState;
    public float durationTimerInvestigation;
    public float durationTimerPrison;
    
    private bool currentlyPlayingCriminalCatched = false;
    private CharacterController cc;

    void Start(){
        state = GameObject.FindWithTag("State");
        globalMoney = state.GetComponent<global_money>();
        //transform.position = new Vector3(-0.72f,2.07f,1.61f);
    }

    // Update is called once per frame
    void Update()
    {
        if ((netPlayer == null) && GetComponent<NetworkPlayer>() != null){
            netPlayer = GetComponent<NetworkPlayer>();
        }
        if ((thiefActions == null) && GetComponent<ThiefActions>() != null){
            thiefActions = GetComponent<ThiefActions>();
        }
        if (cc == null) {
            cc = GetComponentInParent<CharacterController>();
        }
        UpdateMoneyUI();
        cooldownText.text = "";
        warnMessage.text = "";
        intenseOverlay.enabled = false;
        CheckEnterClickFromHost();

        if (!netPlayer.isHostAndPolice)
        {
            updateThiefUI();
        }
    }

    private void updateThiefUI()
    {
        cc.enabled = true;
        if (netPlayer.isCriminal && !netPlayer.isInPrison) {
            cc.enabled = true;
            intenseOverlay.enabled = true;
            warnMessage.text = "Now you are criminal! Stay away from policeman*in";
            UpdateIntenseOverlay();
            UpdateCriminalStatus();
        }

        if (durationTimerStealCooldown > 0) {
            UpdateStealingCooldown();
        }

        if (netPlayer.isBeingInvestigated) {
            // disable character controller so the player cannot move meanwhile
            cc.enabled = false;
            warnMessage.text = "You are being investigated by policeman!";
            UpdateBeingInvestigated();
        }
        if (netPlayer.isBeingInvestigated && netPlayer.isCriminal){
            StartCoroutine(playCriminalCatched());
        }
        if (netPlayer.isInPrison) {
            cc.enabled = true;
            thiefActions.setCriminal(false);
            //black.color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
            //ownAudio.Stop();
            warnMessage.text = "You are in prison!";
            UpdateWhileInPrison();
        }
    }

    private void CheckEnterClickFromHost(){
        if (Input.GetKeyDown(KeyCode.Return) && GetComponentInParent<NetworkPlayer>().isHostAndPolice)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("State");
            game_state gameState = obj.GetComponent<game_state>();

            if (gameState.gameState == GameState.pregame)
            {
                onStartClick();
            }
            else if (gameState.gameState == GameState.game)
            {
                gameState.gameState = GameState.aftergame;
            }
            else if (gameState.gameState == GameState.aftergame)
            {
                gameState.gameState = GameState.pregame;
            }
            
        }
    }

    private void UpdateMoneyUI()
    {
        moneyLeft = globalMoney.GlobalMoney;
        totalPocketMoney = globalMoney.TotalPocketMoney;
        frontBarMoney.fillAmount = (float)(moneyLeft / moneyTotal);
        backBarMoney.color = Color.white;
        
        backBarMoney.fillAmount = (moneyLeft + totalPocketMoney) / moneyTotal;
        moneyText.text = Mathf.Round(moneyLeft) + "$ Left";

    }

    private void UpdateStealingCooldown()
    {
        // Reduce cooldown as time goes...
        //Debug.Log(currentStealCooldown);
        if (durationTimerStealCooldown > 0) 
        {
            durationTimerStealCooldown -= Time.deltaTime;
            durationTimerStealCooldown = Mathf.Clamp(durationTimerStealCooldown, 0, 999);
            string guiTimer = durationTimerStealCooldown.ToString("0");
            cooldownText.text = cooldownText.text = "Steal Cooldown: " + guiTimer;
        }
    }

    public void UpdateIntenseOverlay()
    {
        // GUI loop for intense mode
        float factor = Time.deltaTime * fadeSpeed;
        durationTimerAnimation += Time.deltaTime;
        if (animationDuration > durationTimerAnimation)
        {
            float tempAlpha = intenseOverlay.color.a;
            tempAlpha -= factor;
            intenseOverlay.color = new Color(intenseOverlay.color.r, intenseOverlay.color.g, intenseOverlay.color.b, tempAlpha);
            
            warnMessage.transform.localScale += new Vector3(0.1f * factor, 0.1f * factor, 0.1f * factor);
        }
        if (durationTimerAnimation > animationDuration)
        {
            float tempAlpha = intenseOverlay.color.a;
            tempAlpha += factor;
            intenseOverlay.color = new Color(intenseOverlay.color.r, intenseOverlay.color.g, intenseOverlay.color.b, tempAlpha);

            warnMessage.transform.localScale -= new Vector3(0.1f * factor, 0.1f * factor, 0.1f * factor);
        }
        if (durationTimerAnimation > animationDuration * 2)
        {
            durationTimerAnimation = 0;
        }

    }

    public void UpdateCriminalStatus(){
        if (thiefActions.pocketMoneyHidden && netPlayer.isCriminal)
        {
            durationTimerCriminalState -= Time.deltaTime;
            string guiTimer = durationTimerCriminalState.ToString("0");
            cooldownText.text = "No longer criminal in " + guiTimer;
            if (durationTimerCriminalState <= 0){
                thiefActions.setCriminal(false);
                durationTimerCriminalState = thiefActions.wantedStateDuration;
            }
        }
    }

    public void UpdateBeingInvestigated()
    {
        durationTimerInvestigation -= Time.deltaTime;
        cooldownText.text = "Being investigated... ";
    }

    public void UpdateWhileInPrison(){
        durationTimerPrison -= Time.deltaTime;
        string guiTimer = durationTimerPrison.ToString("0");
        cooldownText.text = "Leaving the prison in " + guiTimer;
        if (durationTimerPrison < 0){
            durationTimerPrison = thiefActions.prisonTimeDuration;
            //transform.position = Utils.GetRandomSpawnPoint();
            cc.enabled = true;
        }
    }

    public void updatePlayerCount(int playerCount, int maxPlayers)
    {
        playerCountText.text = $"{playerCount}/{maxPlayers} Player";
    }

    public void onStartClick()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("State");
        game_state gameState = obj.GetComponent<game_state>();

        gameState.gameState = GameState.game;
    }

    public bool isOnStealCooldown()
    {
        return (durationTimerStealCooldown > 0);
    }

    IEnumerator playCriminalCatched()
     {
         
         if (!currentlyPlayingCriminalCatched)
         {
            currentlyPlayingCriminalCatched = true;
            ownAudio.clip = catchedByPolice;
            // Play the sound
            ownAudio.Play();
            fadeAnimator.SetTrigger("FadeToBlack");
            yield return new WaitForSeconds(9);
            ownAudio.Stop();
            fadeAnimator.SetTrigger("FadeBack");
            currentlyPlayingCriminalCatched = false;
         }
         
     }
}
