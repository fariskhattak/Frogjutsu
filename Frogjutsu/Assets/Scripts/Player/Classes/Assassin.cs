using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : Player
{
    public float additionalMoveSpeed = 3f;
    public override void Run(float dirX)
    {
        float originalMoveSpeed = moveSpeed;
        moveSpeed += additionalMoveSpeed;

        base.Run(dirX);

        moveSpeed = originalMoveSpeed;
    }
}
