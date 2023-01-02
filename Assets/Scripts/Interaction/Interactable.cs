using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Add or remove an InteractionEvent component to this gameObject.
    public bool useEvents;
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
