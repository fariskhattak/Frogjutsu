using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float currentHealth;
    private Animator anim;
    [SerializeField] private AudioClip painSound;
    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = health;
    }
    void Update()
    {
        if (health < currentHealth)
        {
            currentHealth = health;
            anim.SetTrigger("hurt");
        }

        if (currentHealth <= 0)
        {
            anim.SetBool("isDead", true);
            Debug.Log("Enemy is dead");
        }
    }

    public void TakeDamage(float damage)
    {
        SoundManager.instance.PlaySound(painSound);
        health -= damage;
    }

    public void Death()
    {
        gameObject.SetActive(false);
    }
}