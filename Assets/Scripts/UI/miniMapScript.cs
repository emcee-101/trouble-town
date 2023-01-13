using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class miniMapScript : MonoBehaviour
{
    public Camera miniMapCam; 
    public bool enable { get { return isCurEnabled; } set { miniMapCam.enabled = value; isCurEnabled = value; } }

    private bool isCurEnabled;

}
