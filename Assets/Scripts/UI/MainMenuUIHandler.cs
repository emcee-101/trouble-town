using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuUIHandler : MonoBehaviour, IMenu
{
    [Header("Panels")]
    public GameObject playerDetailsPanel;
    public GameObject sessionListPanel;
    public GameObject createSessionPanel;
    public GameObject statusPanel;

    [Header("Player Details")]
    public TMP_InputField playerNameInputField;

    [Header("Session List ")]
    public Button backFromSessionListPanelButton;
    public Button createNewSessionButton;
    
    [Header("Create Session")]
    public TMP_InputField sessionNameInputField;
    public Button backFromCreateSessionPanelButton;

    private Canvas myCanvas;
    private const string mapSceneName = "Multiplayer";


    void Start()
    {
        myCanvas = this.GetComponent<Canvas>();

        if (PlayerPrefs.HasKey("PlayerNickname"))
            playerNameInputField.text = PlayerPrefs.GetString("PlayerNickname");


        myCanvas.enabled = true;
    }

    private void HideAllPanels()
    {
        playerDetailsPanel.SetActive(false);
        sessionListPanel.SetActive(false);
        statusPanel.SetActive(false);
        createSessionPanel.SetActive(false);
    }

    public void OnFindGameClicked()
    {
        PlayerPrefs.SetString("PlayerNickname", playerNameInputField.text);
        PlayerPrefs.Save();

        GameManager.instance.playerNickName = playerNameInputField.text;

        NetworkRunnerHandler networkRunnerHandler = FindObjectOfType<NetworkRunnerHandler>();

        networkRunnerHandler.OnJoinLobby();

        HideAllPanels();

        sessionListPanel.gameObject.SetActive(true);
        backFromSessionListPanelButton.gameObject.SetActive(false);
        createNewSessionButton.gameObject.SetActive(false);

        FindObjectOfType<SessionListUIHandler>(true).OnLookingForGameSessions();
    }

    public void OnCreateNewGameClicked()
    {
        HideAllPanels();

        createSessionPanel.SetActive(true);
    }

    public void OnStartNewSessionClicked()
    {
        NetworkRunnerHandler networkRunnerHandler = FindObjectOfType<NetworkRunnerHandler>();

        networkRunnerHandler.CreateGame(sessionNameInputField.text, mapSceneName);

        HideAllPanels();

        statusPanel.gameObject.SetActive(true);
    }

    public void OnBackFromSessionListPanelClicked()
    {
        HideAllPanels();

        playerDetailsPanel.gameObject.SetActive(true);
    }

    public void OnBackFromCreateSessionPanelClicked()
    {
        HideAllPanels();

        sessionListPanel.gameObject.SetActive(true);
    }

    public void OnJoiningServer()
    {
        HideAllPanels();

        statusPanel.gameObject.SetActive(true);
    }

    public bool toggleStatus(bool setTo)
    {
        myCanvas.enabled = setTo;
        return myCanvas.enabled;

    }

    public bool getStatus()
    {
        return myCanvas.enabled;
    }
}
