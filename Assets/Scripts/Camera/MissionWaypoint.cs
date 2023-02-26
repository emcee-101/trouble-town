using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// Maintained By Mohammad Zidane
// From https://github.com/OBalfaqih/Unity-Tutorials/blob/master/Unity%20Tutorials/WaypointMarker/Scripts/MissionWaypoint.cs
public class MissionWaypoint : MonoBehaviour
{
    public Image img;
    // Indicator icon
    public Sprite bankImg;
    public Sprite hideoutImg;    
    // The target (location, enemy, etc..)
    private GameObject objecty;
    // UI Text to display the distance
    public TextMeshProUGUI meter;
    // To adjust the position of the icon
    public Vector3 offset;
    
    private Vector3 target;

    private void Start()
    {
        img.sprite = bankImg;
        target = new Vector3(-30.1200f,3.81f,89.03f);
    }
    private void Update()
    {
        if (img == null)
        {
            return;
        }
        // Giving limits to the icon so it sticks on the screen
        // Below calculations witht the assumption that the icon anchor point is in the middle
        // Minimum X position: half of the icon width
        float minX = img.GetPixelAdjustedRect().width / 2;
        // Maximum X position: screen width - half of the icon width
        float maxX = Screen.width - minX;

        // Minimum Y position: half of the height
        float minY = img.GetPixelAdjustedRect().height / 2;
        // Maximum Y position: screen height - half of the icon height
        float maxY = Screen.height - minY;

        // Temporary variable to store the converted position from 3D world point to 2D screen point
        Vector2 pos = Camera.main.WorldToScreenPoint(target + offset);

        // Check if the target is behind us, to only show the icon once the target is in front
        if(Vector3.Dot((target - transform.position), transform.forward) < 0)
        {
            // Check if the target is on the left side of the screen
            if(pos.x < Screen.width / 2)
            {
                // Place it on the right (Since it's behind the player, it's the opposite)
                pos.x = maxX;
            }
            else
            {
                // Place it on the left side
                pos.x = minX;
            }
        }

        // Limit the X and Y positions
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        // Update the marker's position
        img.transform.position = pos;
        // Change the meter text to the distance with the meter unit 'm'
        meter.text = ((int)Vector3.Distance(target, transform.position)).ToString() + "m";
    
    }

    public void setWayPointPosition(Vector3 vector)
    {
        target = vector;
    }

    public void setWaypointType(string type)
    {
        switch (type)
        {
            case "bank":
                img.sprite = bankImg;
                break;
            case "hideout":
                img.sprite = hideoutImg;
                break;
        }
    }
}
