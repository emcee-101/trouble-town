using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quality : MonoBehaviour
{

    private void Start()
    {
        // set quality settings to used value
        Debug.Log("current graphics level: "+QualitySettings.GetQualityLevel());
        GetComponent<TMPro.TMP_Dropdown>().SetValueWithoutNotify(QualitySettings.GetQualityLevel());
    }
    public void changeQuality(int newVal)
    {

        Debug.Log("The chosen setting is: " + newVal);
        QualitySettings.SetQualityLevel(newVal);



    }
}
