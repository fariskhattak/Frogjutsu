using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using TMPro;

public class Player : MonoBehaviour
{
    public Stats playerStats;
    protected HealthBar healthBar;
    protected ManaBar manaBar;
    protected float knockbackForce = 15f;
    public int lifeCounter = 3;
    public TMP_Text lifeText;
    protected bool isAlive;

    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer sprite;
    public LayerMask enemies;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 respawnPosition;
    protected PlayerMovement playerMovement;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip jumpSound;

    void Awake()
    {
        Debug.Log("Player is now awake!");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();

        startPosition = transform.position;
        startRotation = transform.rotation;
        respawnPosition = transform.position;
        isAlive = true;

        playerStats = PlayerManager.Instance.playerStats;

        InitHealthBar();
        InitManaBar();
        InitLifeText();
    }

    public virtual void Update()
    {
        bool attacking = anim.GetBool("isAttacking");
        if (Input.GetButtonDown("Fire1") && !attacking)
        {
            Debug.Log("Player used attack button");
            StartAttack();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isAlive)
        {
            SoundManager.instance.PlaySound(deathSound);
            anim.SetTrigger("hit");
            playerStats.TakeDamage(damage);
            healthBar.SetHealth(playerStats.currentHealth);
            if (playerStats.currentHealth <= 0)
            {
                Die();
            }
        }
    }
    public void Die()
    {
        if (isAlive)
        {
            Debug.Log("Player has died");
            lifeCounter--;
            UpdateLifeText();
            anim.SetTrigger("death");
            isAlive = false;
            rb.velocity = Vector2.zero;
            playerMovement.enabled = false;
        }

    }

    public void ResetPlayer()
    {
        playerStats.DeathReset();

        if (lifeCounter <= 0)
        {
            PlayerManager.Instance.playerStats = new Stats();
            SceneManager.LoadSceneAsync("Game Over");
        }
        else
        {
            Debug.Log("Resetting Player");
            anim.SetTrigger("respawn"); // Assuming you have a reset animation or logic
            transform.position = respawnPosition;
            transform.rotation = startRotation;
            healthBar.SetHealth(playerStats.maxHealth);
            manaBar.SetMana(playerStats.maxMana);
            isAlive = true;
            playerMovement.enabled = true;
        }

    }

    public virtual void Jump()
    {
        SoundManager.instance.PlaySound(jumpSound);
        rb.velocity = new Vector2(rb.velocity.x, playerStats.jumpForce);
        // Debug.Log(jumpForce);
    }

    public virtual void Run(float dirX)
    {
        rb.velocity = new Vector2(dirX * playerStats.moveSpeed, rb.velocity.y);
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
        return playerStats.moveSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // anim.SetTrigger("hit");
            if (rb != null)
            {

                // Calculate knockback direction (opposite of collision normal)
                Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;

                // Apply knockback force to the player
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

                Debug.Log("Knockback direction: " + knockbackDirection);
                Debug.Log("Knockback force: " + (knockbackDirection * knockbackForce));
            }
        }
        if (collision.gameObject.tag == "Level Sewer")
        {
            // Get the active scene's name
            string activeSceneName = SceneManager.GetActiveScene().name;
            List<string> levelList = new List<string>(PlayerManager.Instance.levelScenes);
            // Find the index of the active scene in the list
            int levelNumber = levelList.IndexOf(activeSceneName) + 1;
            Debug.Log("Active Scene is: " + activeSceneName);
            Debug.Log("Level Number is: " + levelNumber);
            Debug.Log("Unlocked Levels Count is: " + playerStats.unlockedLevels);
            if (levelNumber == playerStats.unlockedLevels)
            {
                playerStats.unlockedLevels++;
                Debug.Log("Unlocked Levels Count is updated to: " + playerStats.unlockedLevels);
                Debug.Log("PlayerManager Instance Unlocked Levels Count is updated to: " + PlayerManager.Instance.playerStats.unlockedLevels);
            }
            SceneManager.LoadScene("Level Selection");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Death Platform")
        {
            SoundManager.instance.PlaySound(deathSound);
            Die();
        }
        if (collider.gameObject.tag == "Checkpoint")
        {
            Debug.Log("Passed checkpoint");
            respawnPosition = collider.gameObject.transform.position;
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
                healthBar.SetMaxHealth(playerStats.maxHealth);
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
    private void InitManaBar()
    {
        GameObject manaBarObject = GameObject.FindGameObjectWithTag("ManaBar");
        if (manaBarObject != null)
        {
            manaBar = manaBarObject.GetComponent<ManaBar>();
            if (manaBar != null)
            {
                manaBar.SetMaxMana(playerStats.maxMana);
            }
            else
            {
                Debug.LogError("The ManaBar component was not found on the object with tag 'ManaBar'.");
            }
        }
        else
        {
            Debug.LogError("An object with the tag 'ManaBar' was not found in the scene.");
        }
    }

    private void InitLifeText()
    {
        GameObject lifeTextObject = GameObject.FindGameObjectWithTag("Life Counter");
        if (lifeTextObject != null)
        {
            lifeText = lifeTextObject.GetComponent<TMP_Text>();
            if (lifeText != null)
            {
                UpdateLifeText();
            }
            else
            {
                Debug.LogError("TMP_Text component not found on the object with tag 'LifeText'.");
            }
        }
        else
        {
            Debug.LogError("An object with the tag 'LifeText' was not found in the scene.");
        }
    }

    private void UpdateLifeText()
    {
        if (lifeText != null)
            lifeText.text = "x" + lifeCounter;
    }
}
