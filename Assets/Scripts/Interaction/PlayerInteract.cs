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
        RaycastHit hitInfo;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance);
        if (Physics.Raycast(ray, out hitInfo, rayDistance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.promptMessage);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.BaseInteract();
                    onInteractPlayerAction(hitInfo);
                    
                }
            }
        }
    }

    void onInteractPlayerAction(RaycastHit hitInfo){
        Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
        switch (interactable.interactableType)
        {
            case "bank":
                bool rubbed_succesfully = thiefActions.rubBank();
                break;
            case "hideout":
                thiefActions.hideMoney();
                break;
            case "thief":
                NetworkPlayer netPlayer = hitInfo.collider.GetComponent<NetworkPlayer>();
                netPlayer.getInvestigated = true;
                break;

        }
    }
}
