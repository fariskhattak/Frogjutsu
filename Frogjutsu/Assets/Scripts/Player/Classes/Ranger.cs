using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Ranger : Player
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrows;
    private float cooldownTimer;

    public override void Update()
    {
        if (cooldownTimer > attackCooldown)
        {
            base.Update();
        }

        cooldownTimer += Time.deltaTime;
    }

    public override void StartAttack()
    {
        base.StartAttack();
        cooldownTimer = 0;

    }

    public void Shoot()
    {
        arrows[0].transform.position = firePoint.position;
        Debug.Log(transform.localScale.x);
        arrows[0].GetComponent<BasicProjectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
}
