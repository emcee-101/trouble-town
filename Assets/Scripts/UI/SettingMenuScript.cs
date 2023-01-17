using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenuScript : MonoBehaviour, IMenu
{

    private Canvas myCanvas;

    void Start()
    {
        myCanvas = GetComponent<Canvas>();
    }
    public bool toggleStatus()
    {

        myCanvas.enabled = !myCanvas.enabled;
        return myCanvas.enabled;

    }


}
