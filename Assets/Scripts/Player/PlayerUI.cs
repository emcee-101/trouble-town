using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Author: Mohammad Zidane
public class PlayerUI : MonoBehaviour
{
    [Header("Text Elements")]
    public TextMeshProUGUI promptText;
    [SerializeField] private TextMeshProUGUI warnMessage;
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private TextMeshProUGUI introSubText;
    [SerializeField] private TextMeshProUGUI thiefPocketMoney;
    [SerializeField] private TextMeshProUGUI thiefSecuredMoney;
    [SerializeField] private MissionWaypoint waypoints;
    
    [Header("Animator")]
    [SerializeField] private  Animator fadeAnimator;

    [Header("Intense Overlay")]
    [SerializeField] private Image intenseOverlay;

    [Header("Money Bar")]
    [SerializeField] private Image frontBarMoney;
    [SerializeField] private Image backBarMoney;
    [SerializeField] private TextMeshProUGUI moneyText;

    [Header("LobbyUI")]
    [SerializeField] public TextMeshProUGUI playerCountText;

    private bool animatorRunning;
    private float fadeSceneSwitchDuration;
    private float animationDuration = 2.0f;
    private float moneyTotal;
    private float moneyLeft;
    private float totalPocketMoney;

    private NetworkPlayer netPlayer;
    private ThiefActions thiefActions;
    private PlayerAudio playerAudio;

    private GameObject state;
    private global_money globalMoney;

    private Vector3 ownHideoutLocation;
    private Vector3 bankLocation;


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
        bankLocation = new Vector3(-30.1200f,3.81f,89.03f);
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

        // Some UI element are overritten multple times here, as higher priority events are 
        // intentionally placed at the bottom after lower priority once

        // Workaround : Get the hideout location for once and store it in a variable
        if (ownHideoutLocation == Vector3.zero && netPlayer.myHideout != null){
            ownHideoutLocation = netPlayer.myHideout.transform.position;
        }
        thiefPocketMoney.text = string.Format("Pocket Money: {0}$.", thiefActions.pocketMoney);
        thiefSecuredMoney.text = string.Format("Secured Money: {0}$.", thiefActions.currentMoney);

        waypoints.setWaypointType("bank");
        waypoints.setWayPointPosition(bankLocation);

        cc.enabled = true;

        // If the player has stolen (is Criminal) & not in prison.
        if (netPlayer.isCriminal && !netPlayer.isInPrison) {

            Debug.Log(netPlayer.myHideout.transform.position);
            Debug.Log(ownHideoutLocation);

            // Workaround : Get the hideout location for once and store it in a variable
            if (ownHideoutLocation == null && netPlayer.myHideout.transform.position != null)
            {
                ownHideoutLocation = new Vector3(netPlayer.myHideout.transform.position.x, netPlayer.myHideout.transform.position.y, netPlayer.myHideout.transform.position.z);

            }

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
            waypoints.setWayPointPosition(bankLocation);
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
            playerAudio.ownAudio.clip = thiefActions.catchedByPolice;
            // Play the sound
            playerAudio.ownAudio.Play();
            fadeAnimator.SetTrigger("FadeToBlack");
            yield return new WaitForSeconds(9);
            playerAudio.ownAudio.Stop();
            fadeAnimator.SetTrigger("FadeBack");
            currentlyPlayingCriminalCatched = false;
         }
         
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
