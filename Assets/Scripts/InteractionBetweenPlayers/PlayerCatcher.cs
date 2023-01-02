using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCatcher : MonoBehaviour
{   
    // Tutorial followed: https://youtu.be/THmW4YolDok

    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;

    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;


    // Update is called once per frame
    void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

        if (_numFound > 0)
        {

            var interactable = _colliders[0].GetComponent<IPlayerCatch>();

            if (interactable != null && Input.GetKeyDown(KeyCode.E))
            {
                interactable.Interact(this);
            } ;
        }
    }
}
