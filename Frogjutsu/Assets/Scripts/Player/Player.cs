using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected int health = 5;
    protected float moveSpeed = 7f;
    protected int mana = 50;
    protected float jumpForce = 14f;

    protected Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        Debug.Log(jumpForce);
    }

    public virtual void Run(float dirX)
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        Debug.Log(moveSpeed);
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
}
