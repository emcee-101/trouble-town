using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : Interactable
{
    private int totalMoney = 8000;
    private int rubAmount = 1000;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
        
        if (totalMoney>= rubAmount)
        {
            totalMoney -= rubAmount;
            
        }
        else if (totalMoney > 0){
            totalMoney = 0
        }
        else {
            
        }
    }
}
