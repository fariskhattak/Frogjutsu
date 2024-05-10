using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[System.Serializable] // This makes it visible in the inspector
public class Stats
{
    public int maxHealth;
    public int currentHealth;
    public int maxMana;
    public int currentMana;
    public float moveSpeed;
    public float damage;
    public float defense;
    public float jumpForce;
    public int unlockedLevels = 1;

    public int score;

    public Stats()
    {
        maxHealth = 100;
        maxMana = 50;
        moveSpeed = 7f;
        damage = 10f;
        defense = 10f;
        jumpForce = 14f;
        unlockedLevels = 1;
    }

    public Stats(int maxHealth, int maxMana, float moveSpeed, float damage, float defense, float jumpForce)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth; // Initialize current health to max by default
        this.maxMana = maxMana;
        this.currentMana = maxMana; // Initialize current mana to max by default
        this.moveSpeed = moveSpeed;
        this.damage = damage;
        this.defense = defense;
        this.jumpForce = jumpForce;
        unlockedLevels = 1;
        score = 0;
    }

    public Stats(int maxHealth, int maxMana, float moveSpeed, float damage, float defense, float jumpForce, int _unlockedLevels, int _score)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth; // Initialize current health to max by default
        this.maxMana = maxMana;
        this.currentMana = maxMana; // Initialize current mana to max by default
        this.moveSpeed = moveSpeed;
        this.damage = damage;
        this.defense = defense;
        this.jumpForce = jumpForce;
        unlockedLevels = _unlockedLevels;
        score = _score;
    }

    public Stats BaseWarriorStats()
    {
        return new Stats(120, 20, 7, 20, 10, 17, unlockedLevels, score);
    }

        public Stats BaseMageStats()
    {
        return new Stats(70, 100, 5, 40, 7, 14, unlockedLevels, score);
    }

        public Stats BaseRangerStats()
    {
        return new Stats(100, 30, 7, 20, 7, 14, unlockedLevels, score);
    }

        public Stats BaseAssassinStats()
    {
        return new Stats(80, 40, 10, 15, 5, 14, unlockedLevels, score);
    }

    public void DeathReset()
    {
        Debug.Log("Death Reset on Player");
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    // You might want to add methods here for modifying stats, like taking damage or restoring health
    public void TakeDamage(int damageAmount)
    {
        int damageAfterDefense = Mathf.Clamp(damageAmount - (int)defense, 1, int.MaxValue);
        Debug.Log("Damage after defense:" + damageAfterDefense);
        currentHealth -= damageAfterDefense;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log("Player current health: " + currentHealth);
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    public void RestoreMana(int restoreAmount)
    {
        currentMana += restoreAmount;
        currentMana = Mathf.Min(currentMana, maxMana);
    }

    public void PrintStats()
    {
        // Log each variable to the console
        Debug.Log($"Max Health: {maxHealth}");
        Debug.Log($"Current Health: {currentHealth}");
        Debug.Log($"Max Mana: {maxMana}");
        Debug.Log($"Current Mana: {currentMana}");
        Debug.Log($"Move Speed: {moveSpeed}");
        Debug.Log($"Damage: {damage}");
        Debug.Log($"Defense: {defense}");
        Debug.Log($"Jump Force: {jumpForce}");
        Debug.Log($"Unlocked Levels: {unlockedLevels}");
    }

    public void IncreaseScore()
    {
        int scoreIncrease = Random.Range(100, 301); // Upper limit is exclusive in Random.Range
        score += scoreIncrease;
        Debug.Log("Score increased by " + scoreIncrease + ". Total score: " + score);
    }
}