using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMouse : MonoBehaviour
{
    // In future iterations: can be set by menu
    public float mouseSensitivity = 200f;

    public float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
          print("Script for mouse-input started...");
    }

    // Update is called once per frame
    void Update()
    {
        float mX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        xRotation -= mY;
        xRotation = Mathf.Clamp(xRotation, - 90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.parent.Rotate(Vector3.up * mX);
    }
}
