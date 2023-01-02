using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCaught: MonoBehaviour, IPlayerCatch
{

    // Tutorial followed: https://youtu.be/THmW4YolDok

    [SerializeField] private string _prompt;
    public string InteractPrompt => _prompt;


    public bool Interact(PlayerCatcher interactor) {
        Log.Info("Interaction between Players");
        return true;
    }
}
