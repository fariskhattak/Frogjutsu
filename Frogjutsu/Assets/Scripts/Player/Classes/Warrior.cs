using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Player
{
    public float additionalJumpForce = 5f;
    void Start()
    {
        jumpForce += additionalJumpForce;
        damage = 20;
    }
    public override void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
