using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Warrior : Player
{
    public GameObject attackPoint;
    public float attackRadius;
    [SerializeField] private AudioClip attackSound;
    private float baseDamage;
    private float baseMoveSpeed;

    void Start()
    {
        playerStats = playerStats.BaseWarriorStats();
        PlayerManager.Instance.playerStats = playerStats;

        baseDamage = playerStats.damage;
        baseMoveSpeed = playerStats.moveSpeed;

        InitHealthBar();
        InitManaBar();
    }

    public override void Update()
    {
        base.Update();

        if (specialAbilityActivated)
        {
            playerStats.damage = 100;
            playerStats.moveSpeed = 14;
        }
        else
        {
            playerStats.damage = baseDamage;
            playerStats.moveSpeed = baseMoveSpeed;
        }
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
