using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Player
{
    public float additionalJumpForce = 5f;
    public GameObject attackPoint;
    public float attackRadius;
    void Start()
    {
        jumpForce += additionalJumpForce;
        damage = 20;
    }
    public override void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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
                enemyGameObject.GetComponent<EnemyHealth>().health -= damage;
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
