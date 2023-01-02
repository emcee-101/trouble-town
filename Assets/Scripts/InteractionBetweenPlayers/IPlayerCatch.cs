using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerCatch
{
    public string InteractPrompt { get; }

    public bool Interact(PlayerCatcher interactor);
}
