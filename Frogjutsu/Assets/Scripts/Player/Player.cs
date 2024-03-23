using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected int maxHealth = 100;
    protected int currentHealth;
    protected HealthBar healthBar;
    protected float moveSpeed = 7f;
    protected int mana = 50;
    protected float jumpForce = 14f;

    protected Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        InitHealthBar();

    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
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

    void InitHealthBar()
    {
        GameObject healthBarObject = GameObject.FindGameObjectWithTag("HealthBar");
        if (healthBarObject != null)
        {
            // Get the HealthBar component from the found GameObject
            healthBar = healthBarObject.GetComponent<HealthBar>();
            if (healthBar != null)
            {
                // Initialize health bar (as an example)
                healthBar.SetMaxHealth(maxHealth);
            }
            else
            {
                Debug.LogError("The HealthBar component was not found on the object with tag 'HealthBar'.");
            }
        }
        else
        {
            Debug.LogError("An object with the tag 'HealthBar' was not found in the scene.");
        }
    }
}
