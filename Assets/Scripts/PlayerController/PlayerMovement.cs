using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Player Controller

public class NewBehaviourScript : MonoBehaviour
{   

    // Needs adjustment
    public float speed = 30f;

    CharacterController cc;


    // Start is called before the first frame update
    void Start()
    {
       print("Script for Player-Movement started...");

        // Component "CharacterController" needs to be added in the Object-Inspector in Unity
        cc = gameObject.GetComponent<CharacterController>(); 
    }

    // Update is called once per frame
    void Update()
    {     
              
                                    
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // test output
        // print(transform.position);

        Vector3 dir = transform.forward * z + transform.right * x;
        cc.Move(dir * speed * Time.deltaTime);



    }
}
