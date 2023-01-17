using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUIHandler : MonoBehaviour, IMenu
{
    public TMP_InputField inputField;
    private Canvas myCanvas;


    void Start()
    {
        myCanvas = GetComponent<Canvas>();

        if (PlayerPrefs.HasKey("PlayerNickname"))
            inputField.text = PlayerPrefs.GetString("PlayerNickname");
    }

    public void OnJoinGameClicked()
    {
        PlayerPrefs.SetString("PlayerNickname", inputField.text);
        PlayerPrefs.Save();

        GameManager.instance.playerNickName = inputField.text;

        SceneManager.LoadScene("Multiplayer");
    }

    public void OnSinglePlayerClicked()
    {
        SceneManager.LoadScene("Main_Scene");
    }

    public void OnSampleSceneClicked()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public bool toggleStatus()
    {
        myCanvas.enabled = !myCanvas.enabled;
        return myCanvas.enabled;

    }


}
