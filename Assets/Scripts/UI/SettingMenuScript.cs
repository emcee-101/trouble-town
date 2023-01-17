using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenuScript : MonoBehaviour, IMenu
{

    private Canvas thisCanvas;
    

    void Start()
    {
        thisCanvas=this.GetComponent<Canvas>();
        

    }
    public bool toggleStatus(bool setTo)
    {

        thisCanvas.enabled = setTo;
        return thisCanvas.enabled;

    }

    public bool getStatus() {
        
        return thisCanvas.enabled;
    }


}
