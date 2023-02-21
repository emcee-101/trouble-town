using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Mohammad Zidane
public class Interactable : MonoBehaviour
{
    // Add or remove an InteractionEvent component to this gameObject.
    public bool useEvents;
    // type of interactable object (hints the interactor to do the intended action)
    public string interactableType;
    // Message displayed when looking at an interactable.
    public string promptMessage;
    public void BaseInteract()
    {
        if (useEvents)
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        Interact();
    }
    protected virtual void Interact()
    {
        //Override this function in your subclass!
    }
}
