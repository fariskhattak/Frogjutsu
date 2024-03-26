using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Timeline;

public class Player : MonoBehaviour
{
    protected int maxHealth = 100;
    protected int currentHealth;
    protected HealthBar healthBar;
    protected float moveSpeed = 7f;
    protected int mana = 50;
    protected float jumpForce = 14f;
    protected float knockbackForce = 15f;
    public float damage = 10;

    protected bool isAlive;

    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer sprite;
    public GameObject attackPoint;
    public float attackRadius;
    public LayerMask enemies;
    private Vector3 startPosition;
    private Quaternion startRotation;
    protected PlayerMovement playerMovement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();

        startPosition = transform.position;
        startRotation = transform.rotation;
        isAlive = true;

        currentHealth = maxHealth;
        InitHealthBar();

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isAlive)
        {

            Debug.Log("Player has died");
            anim.SetTrigger("death");
            isAlive = false;
            rb.velocity = Vector2.zero;
            playerMovement.enabled = false;
        }

    }

    public void ResetPlayer()
    {
        Debug.Log("Resetting Player");
        anim.SetTrigger("respawn"); // Assuming you have a reset animation or logic
        transform.position = startPosition;
        transform.rotation = startRotation;
        currentHealth = maxHealth;
        healthBar.SetHealth(maxHealth);
        isAlive = true;
        playerMovement.enabled = true;
    }

    public virtual void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        // Debug.Log(jumpForce);
    }

    public virtual void Run(float dirX)
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        // Debug.Log(moveSpeed);
    }

    public virtual void EndAttack()
    {
        anim.SetBool("isAttacking", false);
    }

    public virtual void StartAttack()
    {
        if (playerMovement.IsGrounded())
        {
            anim.SetBool("isAttacking", true);
        }

    }

    // Used for melee attacks
    public virtual void Attack()
    {
        if (attackPoint != null)
        {
            Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius, enemies);

            foreach (Collider2D enemyGameObject in enemy)
            {
                Debug.Log("Hit Enemy");
                enemyGameObject.GetComponent<EnemyHealth>().health -= damage;
            }
        }

    }

    private void OnDrawGizmos()
    {
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius);
        }

    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            anim.SetTrigger("hit");
            if (rb != null)
            {
                // Calculate horizontal knockback direction but keep it at ground level
                Vector2 horizontalKnockbackDirection = (transform.position - collision.transform.position).normalized;
                horizontalKnockbackDirection.y = 0; // Keep the force horizontal

                // Apply an initial upward force
                Vector2 upwardForce = Vector2.up * knockbackForce; // Adjust the multiplier as needed
                rb.AddForce(upwardForce, ForceMode2D.Impulse);

                // Apply horizontal force
                rb.AddForce(-horizontalKnockbackDirection * knockbackForce, ForceMode2D.Impulse);
                Debug.Log("Knockback Strength:" + knockbackForce);
                Debug.Log("Horizontal knockback Direction:" + horizontalKnockbackDirection);
                Debug.Log("Horizontal Knockback Strength * Direction:" + horizontalKnockbackDirection * knockbackForce);
            }
        }
    }

    private void InitHealthBar()
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
