using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : Player
{
    public float additionalMoveSpeed = 3f;
    public GameObject attackPoint;
    public float attackRadius;
    public override void Run(float dirX)
    {
        rb.velocity = new Vector2(dirX * (moveSpeed + additionalMoveSpeed), rb.velocity.y);
    }

    // Used for melee attacks
    public void MeleeAttack()
    {
        if (attackPoint != null)
        {
            Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius, enemies);

            foreach (Collider2D enemyGameObject in enemy)
            {
                Debug.Log("Hit Enemy");
                enemyGameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
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
