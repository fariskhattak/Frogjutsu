using UnityEngine;
using UnityEngine.SceneManagement;

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
        // Deactivate the current game object
        gameObject.SetActive(false);
        PlayerManager.Instance.playerStats.IncreaseScore();

        if (SceneManager.GetActiveScene().name == "Level 4")
        {
            // Check if all BossEnemies are dead
            BossEnemy[] bosses = FindObjectsOfType<BossEnemy>();
            bool allBossesDead = true;

            foreach (BossEnemy boss in bosses)
            {
                // If any boss is still active, set allBossesDead to false
                if (boss.gameObject.activeSelf)
                {
                    allBossesDead = false;
                    break;
                }
            }

            // If all bosses are dead, load the victory screen
            if (allBossesDead)
            {
                LoadVictoryScreen();
            }
        }
    }

    void LoadVictoryScreen()
    {
        SceneManager.LoadScene("Victory Scene");
    }
}