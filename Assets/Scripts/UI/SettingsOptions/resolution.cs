using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resolution : MonoBehaviour
{
    TMPro.TMP_Dropdown myDD;
    List<string> options = new List<string>();
    Resolution[] availableRes;
    Resolution resAtStart;
    int indexAtStart = -1;
    
   
    // Start is called before the first frame update
    void Start()
    {
        availableRes = Screen.resolutions;
        resAtStart = Screen.currentResolution;
        
        myDD = GetComponent<TMPro.TMP_Dropdown>();

        int i = 0;

        foreach(Resolution resolution in availableRes) {

            // find out value to highlight at the start
            if (resolution.height == resAtStart.height && resolution.width == resAtStart.width) indexAtStart = i;
            else i++;

            string temp = $"{resolution.width} x {resolution.height}";
            //Debug.Log(temp);
            options.Add(temp);
            
        }

        myDD.AddOptions(options);
        
        myDD.SetValueWithoutNotify(indexAtStart);
    }

    public void setRes(int selectedRes) {

        Debug.Log("Selected Resolution number: "+selectedRes+" which means its height is: " + availableRes[selectedRes].height);
        Screen.SetResolution(availableRes[selectedRes].width, availableRes[selectedRes].height, Screen.fullScreen);
        
    }
    


}
