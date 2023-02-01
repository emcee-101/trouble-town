using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Camera cam;
    [SerializeField]
    private float rayDistance = 15f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private ThiefActions thiefActions;

    void Start() 
    {
        thiefActions = GetComponent<ThiefActions>();
        playerUI = GetComponent<PlayerUI>();
    }

    void Update()
    {
        playerUI.UpdateText(string.Empty);
        // create invisible ray from the center of the camera, shooting outwards.
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, rayDistance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.promptMessage);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.BaseInteract();
                    onInteractPlayerAction(interactable.interactableType);
                    
                }
            }
        }
    }

    void onInteractPlayerAction(string interactableType){
        switch (interactableType)
        {
            case "bank":
                bool rubbed_succesfully = thiefActions.rubBank();
                if (rubbed_succesfully){
                }
                break;
            case "hideout":
                thiefActions.hideMoney();
                break;
            case "thief":
                thiefActions.getInvestigated();
                break;

        }
    }
}
