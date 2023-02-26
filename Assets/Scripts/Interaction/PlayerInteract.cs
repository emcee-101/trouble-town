using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Author: Mohammad Zidane
public class PlayerInteract : MonoBehaviour
{
    public Camera cam;
    [SerializeField] private float rayDistance = 15f;
    [SerializeField] private LayerMask mask;
    private PlayerUI playerUI;
    private ThiefActions thiefActions;
    private PoliceActions policeActions;
    private RaycastHit hitInfo;
    
    void Start() 
    {
        thiefActions = gameObject.GetComponent<ThiefActions>();
        policeActions = gameObject.GetComponent<PoliceActions>();
        playerUI = gameObject.GetComponent<PlayerUI>();
    }

    void Update()
    {
        playerUI.promptText.text = string.Empty;
        // create invisible ray from the center of the camera, shooting outwards.
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance);
        if (!Physics.Raycast(ray, out hitInfo, rayDistance, mask)){
            return;
        }
        if (hitInfo.collider.GetComponent<Interactable>() == null){
            return;
        }
        Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
    
        // Do not show message or allow interaction with hideout if the
        // player has no pocket money
        if (interactable.interactableType == "hideout")
        {
            if (thiefActions.pocketMoney == 0) return;
        }
        playerUI.promptText.text = interactable.promptMessage;
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            interactable.BaseInteract();
            onInteractPlayerAction(hitInfo.collider);
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
                NetworkPlayer remoteNetPlayer = collider.GetComponent<NetworkPlayer>();
                policeActions.investigatePlayer(remoteNetPlayer);
                break;

        }
    }
}
