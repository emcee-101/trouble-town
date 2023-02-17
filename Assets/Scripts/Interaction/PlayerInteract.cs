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
    private RaycastHit hitInfo;
    
    void Start() 
    {
        thiefActions = gameObject.GetComponent<ThiefActions>();
        playerUI = gameObject.GetComponent<PlayerUI>();
    }

    void Update()
    {
        playerUI.promptText.text = string.Empty;
        // create invisible ray from the center of the camera, shooting outwards.
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance);
        if (Physics.Raycast(ray, out hitInfo, rayDistance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.promptText.text = interactable.promptMessage;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.BaseInteract();
                    onInteractPlayerAction(hitInfo.collider);
                    
                }
            }
        }
    }

    void onInteractPlayerAction(Collider collider){

        Interactable interactable = collider.GetComponent<Interactable>();
        switch (interactable.interactableType)
        {
            case "bank":
                thiefActions.rubBank();
                break;
            case "hideout":
                thiefActions.hideMoney();
                break;
            case "thief":
                NetworkPlayer netPlayer = collider.GetComponent<NetworkPlayer>();
                netPlayer.getInvestigated();
                break;

        }
    }
}
