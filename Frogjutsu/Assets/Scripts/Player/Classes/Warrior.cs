using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Player
{
    public GameObject attackPoint;
    public float attackRadius;
    [SerializeField] private AudioClip attackSound;
    void Start()
    {
        playerStats = playerStats.BaseWarriorStats();
        PlayerManager.Instance.playerStats = playerStats;
    }

    // Used for melee attacks
    public void MeleeAttack()
    {
        SoundManager.instance.PlaySound(attackSound);
        if (attackPoint != null)
        {
            Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius, enemies);

            foreach (Collider2D enemyGameObject in enemy)
            {
                Debug.Log("Hit Enemy");
                enemyGameObject.GetComponent<EnemyHealth>().TakeDamage(playerStats.damage);
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
}
