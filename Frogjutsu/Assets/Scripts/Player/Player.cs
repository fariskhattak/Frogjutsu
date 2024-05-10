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
    private Quaternion respawnRotation;
    private Vector3 respawnPosition;
    protected PlayerMovement playerMovement;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip jumpSound;

    [SerializeField] protected float manaRegenCooldown = 0.5f; // Time interval between mana regen
    protected float manaRegenTimer;
    [SerializeField] protected int manaRegenAmount = 1; // Mana to regenerate per tick

    public bool specialAbilityActivated;
    public float specialAbilityCooldown = 5f; // Cooldown time in seconds
    private float lastSpecialAbilityTime = 0; // Time when special ability was last used

    void Awake()
    {
        Debug.Log("Player is now awake!");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();

        respawnRotation = transform.rotation;
        respawnPosition = transform.position;
        isAlive = true;

        playerStats = PlayerManager.Instance.playerStats;

        specialAbilityActivated = false;

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
        if (Input.GetButtonDown("Fire2") && Time.time > lastSpecialAbilityTime + specialAbilityCooldown && playerStats.currentMana >= 20)
        {
            UseSpecialAbility();
        }

        anim.SetBool("usingSpecialAbility", specialAbilityActivated);

        // Regenerate mana over time
        manaRegenTimer += Time.deltaTime;
        if (manaRegenTimer >= manaRegenCooldown)
        {
            playerStats.RestoreMana(manaRegenAmount);
            manaBar.SetMana(playerStats.currentMana);
            manaRegenTimer = 0f;
        }
    }

    void UseSpecialAbility()
    {
        if (!specialAbilityActivated && playerStats.currentMana >= 20)
        {
            Debug.Log("Started special ability state");
            specialAbilityActivated = true;
            playerStats.currentMana -= 20; // Assuming the special ability costs 20 mana
            manaBar.SetMana(playerStats.currentMana); // Update mana bar UI
            anim.SetTrigger("useSpecial"); // Trigger the special ability animation
            StartCoroutine(SpecialAbilityDuration(5)); // Special ability active for 5 seconds
            lastSpecialAbilityTime = Time.time; // Update last used time
        }
    }

    private IEnumerator SpecialAbilityDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        Debug.Log("Removed special ability state");
        specialAbilityActivated = false; // Reset the special ability state
    }

    public void TakeDamage(float damage)
    {
        if (isAlive)
        {
            Debug.Log("Took damage: " + damage);
            SoundManager.instance.PlaySound(deathSound);
            anim.SetTrigger("hit");
            playerStats.TakeDamage(Mathf.RoundToInt(damage)); // Convert float to int
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
            transform.rotation = respawnRotation;
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

    private bool iceBlockSpeedBoosted = false; // Add this variable

    public virtual void Run(float dirX)
    {
        if (!iceBlockSpeedBoosted) // Check if the player's speed has been boosted by an ice block
        {
            rb.velocity = new Vector2(dirX * playerStats.moveSpeed, rb.velocity.y);
        }
        // Debug.Log(moveSpeed);
    }
    public virtual void Sprint(float dirX)
    {
        rb.velocity = new Vector2(dirX * playerStats.moveSpeed * 2, rb.velocity.y);
    }

    public void SetIceBlockSpeedBoosted(bool boosted)
    {
        iceBlockSpeedBoosted = boosted;
    }
    public void DisableMovementForDuration(float duration)
    {
        StartCoroutine(DisableMovementCoroutine(duration));
    }

    private IEnumerator DisableMovementCoroutine(float duration)
    {
        playerMovement.enabled = false;
        yield return new WaitForSeconds(duration);
        playerMovement.enabled = true;
    }


    public virtual void EndAttack()
    {
        anim.SetBool("isAttacking", false);
        anim.SetBool("isSpecialAttacking", false);
    }

    public virtual void StartAttack()
    {
        if (playerMovement.IsGrounded())
        {
            if (!specialAbilityActivated)
            {
                anim.SetBool("isAttacking", true);
                anim.SetBool("isSpecialAttacking", false);
            }
            else
            {
                anim.SetBool("isSpecialAttacking", true);
                anim.SetBool("isAttacking", false);
            }

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

    protected void InitHealthBar()
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
    protected void InitManaBar()
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
