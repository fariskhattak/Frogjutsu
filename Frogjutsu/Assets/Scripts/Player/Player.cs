using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public int deathCounter = 0;

    protected bool isAlive;

    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer sprite;
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

    public virtual void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Player used attack button");
            StartAttack();
        }
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
            deathCounter++;
            anim.SetTrigger("death");
            isAlive = false;
            rb.velocity = Vector2.zero;
            playerMovement.enabled = false;
        }

    }

    public void ResetPlayer()
    {
        if (deathCounter > 2){
            SceneManager.LoadScene("CharacterSelect");
        } else {
            Debug.Log("Resetting Player");
            anim.SetTrigger("respawn"); // Assuming you have a reset animation or logic
            transform.position = startPosition;
            transform.rotation = startRotation;
            currentHealth = maxHealth;
            healthBar.SetHealth(maxHealth);
            isAlive = true;
            playerMovement.enabled = true;
        }
        
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

                // Calculate knockback direction (opposite of collision normal)
                Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;

                // Apply knockback force to the player
                rb.velocity = knockbackDirection * knockbackForce;

                Debug.Log("Knockback direction: " + knockbackDirection);
                Debug.Log("Knockback force: " + (knockbackDirection * knockbackForce));
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Death Platform") {
            Die();
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
