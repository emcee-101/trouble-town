using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoSimMoveForward : MonoBehaviour
{
    public GameObject gameObject;
    public Vector3 direction = Vector3.one;
    public float speed = 5.0f;
    public Vector3 target;
    public Vector3 target2;



    // Start is called before the first frame update
    void Start()
    {
        target = transform.position + direction;
       
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (target == transform.position)
        {

            gameObject.transform.Rotate(180, 0, 180);
            target = transform.position - direction;
            transform.Translate(target* Time.deltaTime);


        }
       
    }







    /* void Update()
     {

         transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

         if (transform.position == target)
         {

             gameObject.transform.Rotate(180.0f, 0.0f, 180.0f);
             target = transform.position - direction;

         }


     Autos fahren und rotieren, fahren abwer nicht in rotierte richtung weiter


--------------------------------------------------------------------------------------------------------------------------------
    void Update()
{

    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

    if (target == transform.position)
    {

        gameObject.transform.Rotate(180, 0, 180);
        target = transform.position - direction;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        target2 = target;


    }
    if (target2 == transform.position)
    {

        gameObject.transform.Rotate(180, 0, 180);
        target2 = transform.position - direction;
        transform.position = Vector3.MoveTowards(transform.position, target2, speed * Time.deltaTime);
        target = target2;
    }
}



    ________________________________________________________________________________________________________________________________
    if (target == transform.position)
        {

            gameObject.transform.Rotate(180, 0, 180);
            
            transform.Translate(transform.position-direction * Time.deltaTime);


        }kreuz und quer

     }*/

}
