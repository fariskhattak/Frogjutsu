using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : Player
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private string arrowTag;
    [SerializeField] private AudioClip attackSound;
    private GameObject[] arrows;
    private float cooldownTimer;

    void Start()
    {
        InitArrows();
        playerStats.damage = 20;
    }

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
        SoundManager.instance.PlaySound(attackSound);
        int arrowNum = FindArrow();
        Debug.Log(arrowNum);
        arrows[arrowNum].transform.position = firePoint.position;
        arrows[arrowNum].GetComponent<BasicProjectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private void InitArrows()
    {
        GameObject[] arrowObjects = GameObject.FindGameObjectsWithTag(arrowTag);
        arrows = new GameObject[arrowObjects.Length];
        for (int i = 0; i < arrowObjects.Length; i++)
        {
            arrows[i] = arrowObjects[i];
            arrows[i].GetComponent<BasicProjectile>().SetDamage(playerStats.damage);
            arrows[i].SetActive(false);
        }
    }

    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
