using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public TextMeshProUGUI cooldownText;
    
    public Image black;
    public float fadeSceneSwitchDuration;

    public float moneyTotal;
    public float moneyLeft;
    public float totalPocketMoney;

    public bool hasJustStolen;
    public bool isCriminal;
    public bool isBeingInvestigated;
    public bool ownAudioPlaying;
    public bool isInPrison;
    public bool pocketMoneyHidden;
    public float stealCooldown;
    [Header("Intense Overlay")]
    public Image intenseOverlay;
    public TextMeshProUGUI warnMessage;
    public float investigationDuration;
    public float prisonTimeDuration;
    public float duration;
    public float fadeSpeed;
    public float wantedStateDuration;
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
        durationTimerCriminalState = wantedStateDuration;
        intenseOverlay.color = new Color(intenseOverlay.color.r, intenseOverlay.color.g, intenseOverlay.color.b, 0.6f);
    }
    
    private float durationTimerTransistionOnHold;
    private float durationTimerSceneTransition;
    private float durationTimerAnimation;

    private float durationTimerStealCooldown;
    private float durationTimerCriminalState;
    private float durationTimerInvestigation;
    private float durationTimerPrison;

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
        UpdateMoneyUI();
        cooldownText.text = "";
        warnMessage.text = "";
        intenseOverlay.enabled = false;
        
        CheckEnterClickFromHost();

        if (hasJustStolen){
            isCriminal = true;
            pocketMoneyHidden = false;
            durationTimerStealCooldown = stealCooldown;
            durationTimerCriminalState = wantedStateDuration;
            hasJustStolen = false;
        }

        if (isCriminal && !isInPrison) {
            if (!ownAudioPlaying){
                ownAudio.PlayOneShot(catchedByPolice);
                ownAudioPlaying = true;
            }
            UpdateFadeToBlack();
            intenseOverlay.enabled = true;
            warnMessage.text = "Now you are criminal! Stay away from policeman*in";
            UpdateIntenseOverlay();
            UpdateCriminalStatus();
        }

        if (durationTimerStealCooldown > 0) {
            UpdateStealingCooldown();
        }

        if (isBeingInvestigated) {
            warnMessage.text = "You are being investigated by policeman!";
            UpdateBeingInvestigated();
        }
        if (isBeingInvestigated && isCriminal){
            if (!ownAudioPlaying){
                ownAudio.PlayOneShot(catchedByPolice);
                ownAudioPlaying = true;
            }
            UpdateFadeToBlack();
        }
        if (isInPrison) {
            if (ownAudioPlaying){
                ownAudio.Stop();
                ownAudioPlaying = false;
            }
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
        moneyText.text = Mathf.Round(moneyLeft) + "+" + Mathf.Round(totalPocketMoney) + " $";

    }

    private void UpdateFadeToBlack()
    {
        if (durationTimerSceneTransition < fadeSceneSwitchDuration){
            durationTimerSceneTransition += Time.deltaTime;
            black.color = new Color(255.0f, 255.0f, 255.0f, (durationTimerSceneTransition/fadeSceneSwitchDuration));
            return;
        }
        else if (durationTimerSceneTransition < fadeSceneSwitchDuration + 2){
            durationTimerSceneTransition += Time.deltaTime;
            black.color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
            return;
        }
        durationTimerSceneTransition = 0;
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
        if (duration > durationTimerAnimation)
        {
            float tempAlpha = intenseOverlay.color.a;
            tempAlpha -= factor;
            intenseOverlay.color = new Color(intenseOverlay.color.r, intenseOverlay.color.g, intenseOverlay.color.b, tempAlpha);
            
            warnMessage.transform.localScale += new Vector3(0.1f * factor, 0.1f * factor, 0.1f * factor);
        }
        if (durationTimerAnimation > duration)
        {
            float tempAlpha = intenseOverlay.color.a;
            tempAlpha += factor;
            intenseOverlay.color = new Color(intenseOverlay.color.r, intenseOverlay.color.g, intenseOverlay.color.b, tempAlpha);

            warnMessage.transform.localScale -= new Vector3(0.1f * factor, 0.1f * factor, 0.1f * factor);
        }
        if (durationTimerAnimation > duration * 2)
        {
            durationTimerAnimation = 0;
        }

    }
    public void UpdateCriminalStatus(){
        if (pocketMoneyHidden && isCriminal)
        {
            durationTimerCriminalState -= Time.deltaTime;
            if (durationTimerCriminalState <= 0){
                isCriminal = false;
                durationTimerCriminalState = wantedStateDuration;
            }
        }
    }
    public void UpdateBeingInvestigated(){
        cc = GetComponentInParent<CharacterController>();
        cc.enabled = false;
        durationTimerInvestigation -= Time.deltaTime;
        string guiTimer = durationTimerInvestigation.ToString("0");
        cooldownText.text = "Being investigated... " + guiTimer;
        if (durationTimerInvestigation < 0){
            durationTimerInvestigation = investigationDuration;
            isBeingInvestigated = false;
            cc.enabled = true;
            if (isCriminal){
                transform.position = new Vector3(43.915f,3.259f,13.179f);
                isInPrison = true;
                durationTimerPrison = prisonTimeDuration;
                black.color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
            }
        }
    }

    public void UpdateWhileInPrison(){
        durationTimerPrison -= Time.deltaTime;
        string guiTimer = durationTimerPrison.ToString("0");
        cooldownText.text = "Leaving the prison in " + guiTimer;
        if (durationTimerPrison < 0){
            durationTimerPrison = prisonTimeDuration;
            isCriminal = false;
            //transform.position = Utils.GetRandomSpawnPoint();
            isInPrison = false;
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

}
