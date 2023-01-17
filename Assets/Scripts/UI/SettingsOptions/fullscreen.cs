using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fullscreen : MonoBehaviour
{

    private Toggle toggler;
    // Start is called before the first frame update
    void Start()
    {
        toggler = GetComponent<Toggle>();
        toggler.isOn = (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen || Screen.fullScreenMode == FullScreenMode.FullScreenWindow);
    }

    public void setFull(bool setToTrue)
    {

        switch (setToTrue)
        {
            case (true):
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case (false):
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;

        }
        
    }
}
