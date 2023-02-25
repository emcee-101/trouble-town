using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed = false;
    private NetworkPlayer networkPlayer;

    //Other components
    LocalCameraHandler localCameraHandler;

    //Values to be transmitted to the Server
    public float scoreChange = 0;
    public int globalMoneyChange = 0;
    public int globalPocketMoneyChange = 0;
    public bool criminalStatus;

    scoring scores;


    private void Awake()
    {
        localCameraHandler = GetComponentInChildren<LocalCameraHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        networkPlayer = GetComponentInParent<NetworkPlayer>();


    }

    // Update is called once per frame
    void Update()
    {
        if (!networkPlayer.isGamePaused)
        {
            //View input
            viewInputVector.x = Input.GetAxis("Mouse X");
            viewInputVector.y = Input.GetAxis("Mouse Y") * -1; //Invert the mouse look

            //Move input
            moveInputVector.x = Input.GetAxis("Horizontal");
            moveInputVector.y = Input.GetAxis("Vertical");


            //Jump
            if (Input.GetButtonDown("Jump"))
                isJumpButtonPressed = true;

            //Set view
            localCameraHandler.SetViewInputVector(viewInputVector);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            networkPlayer.toggleGamePausedState();

        }
    }
    private void getScoresScript()
    {
        scores = GameObject.FindGameObjectWithTag("State").GetComponent<scoring>();

    }

    public void addRobbingPoints()
    {
        if (scores == null)
            getScoresScript();

        scoreChange += scores.pointsForRobbing;

    }

    public void addGettingCaughtPoints()
    {
        if (scores == null)
            getScoresScript();

        scoreChange += scores.pointsForGettingCaught;
    }

    public void addStoringMoneyPoints()
    {
        if (scores == null)
            getScoresScript();


        scoreChange += scores.pointsForStoringMoney;
    }

    public void addCatchingRobberPoints()
    {
        if (scores == null)
            getScoresScript();


        scoreChange += scores.pointsForCatchingRobber;
    }



    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();
        
        //Aim data
        networkInputData.aimForwardVector = localCameraHandler.transform.forward;

        //Move data
        networkInputData.movementInput = moveInputVector;

        //Jump data
        networkInputData.isJumpPressed = isJumpButtonPressed;

        //Reset variables now that we have read their states
        isJumpButtonPressed = false;
        
        // become criminal data
        networkInputData.criminalStatus = criminalStatus;
        
        // transmit name - important for scores
        networkInputData.playerName = new Fusion.NetworkString<Fusion._16>();




        // reflects data from Interactions with players
        networkInputData.globalMoneyChange = globalMoneyChange;
        networkInputData.globalPocketMoneyChange = globalPocketMoneyChange;

        if (networkPlayer != null)
        {

            networkInputData.playerName.Set(networkPlayer.nickName.ToString());
            networkInputData.scoreChange = scoreChange;
        }
        else

            networkInputData.playerName = "FAILURE";

        // set variables back to reflect consumption of change (so that no change is doubled)
        globalMoneyChange = 0;
        scoreChange = 0;
        globalPocketMoneyChange = 0;



        return networkInputData;
    }
}
