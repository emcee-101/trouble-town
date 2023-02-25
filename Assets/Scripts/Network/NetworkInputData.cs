using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public struct NetworkInputData : INetworkInput
{
    public Vector2 movementInput;
    public Vector3 aimForwardVector;
    public NetworkBool isJumpPressed;

    // custom data
    public NetworkString<_16> playerName;
    public float scoreChange;
    public int globalMoneyChange;
    public int globalPocketMoneyChange;
    public NetworkBool criminalStatus;


}
