using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : Player
{
    public float additionalMoveSpeed = 3f;
    public override void Run(float dirX)
    {
        rb.velocity = new Vector2(dirX * (moveSpeed + additionalMoveSpeed), rb.velocity.y);
    }
}
