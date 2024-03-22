using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Player
{
    public float additionalJumpForce = 5f;
    public override void Jump()
    {
        float originalJumpForce = jumpForce;
        jumpForce += additionalJumpForce;

        base.Jump();

        jumpForce = originalJumpForce;
    }
}
