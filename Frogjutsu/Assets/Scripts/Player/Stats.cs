using UnityEngine;

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

    public Stats()
    {
        maxHealth = 100;
        maxMana = 50;
        moveSpeed = 7f;
        damage = 10f;
        defense = 10f;
        jumpForce = 14f;
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
    }

    public void DeathReset()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    // You might want to add methods here for modifying stats, like taking damage or restoring health
    public void TakeDamage(int damageAmount)
    {
        int damageAfterDefense = Mathf.Clamp(damageAmount - (int)defense, 1, int.MaxValue);
        currentHealth -= damageAfterDefense;
        currentHealth = Mathf.Max(currentHealth, 0);
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
}