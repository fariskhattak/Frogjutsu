using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : Player
{
    public float assassinMoveSpeed = 10f;
    public GameObject attackPoint;
    public float attackRadius;
    void Start()
    {
        playerStats.moveSpeed = assassinMoveSpeed;
        PlayerManager.Instance.playerStats = playerStats;
    }
    // public override void Run(float dirX)
    // {
    //     rb.velocity = new Vector2(dirX * playerStats.moveSpeed, rb.velocity.y);
    // }

    // Used for melee attacks
    public void MeleeAttack()
    {
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
