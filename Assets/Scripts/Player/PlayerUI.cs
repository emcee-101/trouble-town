using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public TextMeshProUGUI cooldownText;
    public bool hasRecentlyStolen;
    public bool isCriminal;
    public bool isBeingInvestigated;
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


    [Header("LobbyUI")]
    [SerializeField]
    public TextMeshProUGUI playerCountText;

    public void Init()
    {
        warnMessage.enabled = true;
        durationTimerStealCooldown = 0;
        durationTimerCriminalState = 0;
        intenseOverlay.color = new Color(intenseOverlay.color.r, intenseOverlay.color.g, intenseOverlay.color.b, 0.6f);
    }

    private float durationTimerAnimation;
    private float durationTimerStealCooldown;
    private float durationTimerCriminalState;
    private float durationTimerInvestigation;
    private float durationTimerPrison;
    private CharacterController cc;

    void Start(){
        //transform.position = new Vector3(-0.72f,2.07f,1.61f);
    }

    // Update is called once per frame
    void Update()
    {
        cooldownText.text = "";
        warnMessage.text = "";
        intenseOverlay.enabled = false;
        
        CheckEnterClickFromHost();

        if (hasRecentlyStolen){
            isCriminal = true;
            pocketMoneyHidden = false;
            durationTimerStealCooldown = stealCooldown;
            hasRecentlyStolen = false;
        }

        if (isCriminal) {
            intenseOverlay.enabled = true;
            warnMessage.text = "Now you are criminal! Stay away from policemen";
            UpdateIntenseOverlay();
            UpdateCriminalStatus();
            UpdateStealingCooldown();
        }

        if (isBeingInvestigated) {
            warnMessage.text = "You are being investigated by policeman!";
            UpdateBeingInvestigated();
        }

        if (isInPrison) {
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

    private void UpdateStealingCooldown()
    {
        // Reduce cooldown as time goes...
        //Debug.Log(currentStealCooldown);
        if (durationTimerStealCooldown != 0) 
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
            durationTimerCriminalState += Time.deltaTime;
            if (durationTimerCriminalState > wantedStateDuration){
                isCriminal = false;
                durationTimerCriminalState = 0;
            }
        }
    }
    public void UpdateBeingInvestigated(){
        if (isBeingInvestigated)
        {
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
                isInPrison = true;
                durationTimerPrison = prisonTimeDuration;
                transform.position = new Vector3(43.915f,3.259f,13.179f);
                isCriminal = false;
            }
            }
        }
    }
    public void UpdateWhileInPrison(){
        if (isInPrison)
        {
            durationTimerPrison -= Time.deltaTime;
            string guiTimer = durationTimerPrison.ToString("0");
            cooldownText.text = "Leaving the prison in " + guiTimer;
            if (durationTimerPrison < 0){
                durationTimerPrison = prisonTimeDuration;
                isInPrison = false;
                //transform.position = Utils.GetRandomSpawnPoint();
            }
        }
    }

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }

    public bool isOnStealCooldown()
    {
        return ( durationTimerStealCooldown > 0);
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

}
