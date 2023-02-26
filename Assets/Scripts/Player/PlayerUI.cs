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
    public TextMeshProUGUI introText;
    public TextMeshProUGUI introSubText;
    public Animator fadeAnimator;
    public bool animatorRunning;

    public float fadeSceneSwitchDuration;
    public float animationDuration = 2.0f;
    public float moneyTotal;
    public float moneyLeft;
    public float totalPocketMoney;

    public TextMeshProUGUI thiefPocketMoney;
    public TextMeshProUGUI thiefSecuredMoney;
    

    [Header("Intense Overlay")]
    public Image intenseOverlay;
    public TextMeshProUGUI warnMessage;

    public Image frontBarMoney;
    public Image backBarMoney;
    public TextMeshProUGUI moneyText;

    private NetworkPlayer netPlayer;
    private ThiefActions thiefActions;
    private PlayerAudio playerAudio;

    private GameObject state;
    private global_money globalMoney;

    public MissionWaypoint waypoints;
    private Vector3 ownHideoutLocation;

    [Header("LobbyUI")]
    [SerializeField]
    public TextMeshProUGUI playerCountText;

    public void Init()
    {
        durationTimerSceneTransition = fadeSceneSwitchDuration;
        moneyTotal = globalMoney.GlobalMoney;
        frontBarMoney.fillAmount =  1f;
        backBarMoney.fillAmount  =  1f;
        warnMessage.enabled = true;
        intenseOverlay.color = new Color(intenseOverlay.color.r, intenseOverlay.color.g, intenseOverlay.color.b, 0.6f);
    }
    
    private float fadeSpeed = 0.5f;
    private float durationTimerSceneTransition;
    private float durationTimerAnimation = 0;

    private float currentTimerPrison = 0.0f;
    
    private bool currentlyPlayingCriminalCatched = false;
    private CharacterController cc;

    void Start(){
        state = GameObject.FindWithTag("State");
        globalMoney = state.GetComponent<global_money>();
        // Police text. Will be changed later if player is a thief
        introText.text = "You Are Policeman";
        introSubText.text = "Keep your eyes on thiefs & protect the banks";
    }

    void Update()
    {
        CheckEnterClickFromHost();

        // Work around
        if (waypoints == null && GetComponentInChildren<MissionWaypoint>() != null){
            waypoints = GetComponentInChildren<MissionWaypoint>();
        }

        if ((netPlayer == null) && GetComponent<NetworkPlayer>() != null){
            netPlayer = GetComponent<NetworkPlayer>();
        }
        if ((thiefActions == null) && GetComponent<ThiefActions>() != null){
            thiefActions = GetComponent<ThiefActions>();
            introText.text = "You Are Thief";
            introSubText.text = "Rob banks, hide the cash and be stay away from police";
        }
        if ((playerAudio == null) && GetComponent<PlayerAudio>() != null){
            playerAudio = GetComponent<PlayerAudio>();
        }

        if (cc == null) {
            cc = GetComponentInParent<CharacterController>();
        }
        UpdateMoneyUI();
        cooldownText.text = "";
        warnMessage.text = "";
        intenseOverlay.enabled = false;

        if (!netPlayer.isHostAndPolice)
        {
            updateThiefUI();
        }
    }

    private void updateThiefUI()
    {
        // Workaround : Get the hideout location for once and store it in a variable
        if (ownHideoutLocation == null && netPlayer.myHideout.transform.position != null){
            ownHideoutLocation = netPlayer.myHideout.transform.position;
        }
        thiefPocketMoney.text = string.Format("Pocket Money: {0}$.", thiefActions.pocketMoney);
        thiefSecuredMoney.text = string.Format("Secured Money: {0}$.", thiefActions.currentMoney);

        waypoints.setWaypointType("bank");
        waypoints.setWayPointPosition(new Vector3(-30.1200f,3.81f,89.03f));
        cc.enabled = true;

        // If the player has stolen & not in prison.
        if (netPlayer.isCriminal && !netPlayer.isInPrison) {
            // change waypoint indicator icon and position to player's hideout
            waypoints.setWaypointType("hideout");
            waypoints.setWayPointPosition(ownHideoutLocation);
            // Let the player know that they're wanted
            intenseOverlay.enabled = true;
            warnMessage.text = "Now you are criminal! Stay away from policeman*in";
            // Reseting prison timer
            currentTimerPrison = 0.0f;
            UpdateIntenseOverlay();
            UpdateCriminalStatus();
        }

        // If player has hidden their stolen money, remove Criminal status after
        // certain time has passed
        if (netPlayer.isCriminal && thiefActions.pocketMoneyHidden)
        {
            // change waypoint indicator icon and position back to the bank
            waypoints.setWaypointType("bank");
            waypoints.setWayPointPosition(new Vector3(-30.1200f,3.81f,89.03f));
            cooldownText.text = "No longer criminal in " + thiefActions.currentTimerCriminalState.ToString("0");
        }
    	
        // Show steal cooldown if it exists
        if (thiefActions.currentStealCooldown > 0) {
            cooldownText.text = cooldownText.text = "Steal Cooldown: " + thiefActions.currentStealCooldown.ToString("0");
        }

        if (netPlayer.isBeingInvestigated) {
            // disable character controller so the player cannot move while being investigated
            cc.enabled = false;
            warnMessage.text = "You are being investigated by policeman!";
        }
        // play some fun stuff if the player being investigated is indeed criminal
        if (netPlayer.isBeingInvestigated && netPlayer.isCriminal){
            StartCoroutine(playCriminalCatched());
        }
        if (netPlayer.isInPrison) {
            thiefActions.setCriminal(false);
            warnMessage.text = "You are in prison!";
            currentTimerPrison += Time.deltaTime;
            cooldownText.text = "Leaving the prison in " + (netPlayer.prisonTimeDuration - currentTimerPrison).ToString("0");
        }
    }

    private void CheckEnterClickFromHost(){
        // Christopher
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
        
        backBarMoney.fillAmount = (float)(moneyLeft + totalPocketMoney) / moneyTotal;
        moneyText.text = Mathf.Round(moneyLeft) + "$ Left";

    }

    public void UpdateIntenseOverlay()
    {
        // GUI loop for IntenseOverlay
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

    IEnumerator playCriminalCatched()
     {
         
         if (!currentlyPlayingCriminalCatched)
         {
            currentlyPlayingCriminalCatched = true;
            playerAudio.ownAudio.clip = playerAudio.catchedByPolice;
            // Play the sound
            playerAudio.ownAudio.Play();
            fadeAnimator.SetTrigger("FadeToBlack");
            yield return new WaitForSeconds(9);
            playerAudio.ownAudio.Stop();
            fadeAnimator.SetTrigger("FadeBack");
            currentlyPlayingCriminalCatched = false;
         }
         
     }
}
