using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class hideoout_dispatcher : MonoBehaviour
{
    public GameObject[] hideouts;
    public int numOfHideouts;
    public List<Transform> hideoutPositions;
    public GameObject prefabForHideout;
    public GameObject parentObj;

    void Start()
    {
        hideouts = GameObject.FindGameObjectsWithTag("Hideout");

        numOfHideouts = hideouts.Count();
        hideoutPositions = new List<Transform>();

        foreach(GameObject hideout in hideouts)
        {
            hideoutPositions.Add(hideout.transform);
            hideout.SetActive(false);

        }


    }

    public GameObject dispatchHideout(int number) {

        if (number-1 <= numOfHideouts)
        {
            if(hideoutPositions[number - 1 - 1] == null)
            {
                Debug.Log("Values were read incorrect");
                return null;

            }
                                                    // minus 1 for the fact that player 1 is the police guy and minus 1 for the array/list beginning at 0
            return Instantiate(prefabForHideout, hideoutPositions[number - 1 - 1].position, hideoutPositions[number  - 1 - 1].rotation, parentObj.transform);
           

        }
        else return null;
    }

}
