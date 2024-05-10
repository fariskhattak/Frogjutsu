using UnityEngine;
using System.Collections.Generic;

public class Mage : Player
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject magicAmmoPrefab;
    [SerializeField] private int manaCostPerShot = 10; // Add the mana cost per shot
    private List<GameObject> magicAmmo = new List<GameObject>();
    [SerializeField] private int totalAmmo = 10;
    [SerializeField] private AudioClip attackSound;
    private float cooldownTimer;

    void Start()
    {
        playerStats = playerStats.BaseMageStats();
        PlayerManager.Instance.playerStats = playerStats;
        InitMagicAmmo();
        InitHealthBar();
        InitManaBar();
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
        if (playerStats.currentMana >= manaCostPerShot)
        {
            base.StartAttack();
        }
        else
        {
            Debug.Log("Not enough mana to shoot.");
        }
    }

    public void Shoot()
    {
        playerStats.currentMana -= manaCostPerShot;
        manaBar.SetMana(playerStats.currentMana);
        cooldownTimer = 0;
        SoundManager.instance.PlaySound(attackSound);
        int magicNum = FindMagicAmmo();
        magicAmmo[magicNum].transform.position = firePoint.position;
        magicAmmo[magicNum].GetComponent<BasicMagic>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private void InitMagicAmmo()
    {
        for (int i = 0; i < totalAmmo; i++)
        {
            GameObject newMagic = Instantiate(magicAmmoPrefab, transform.position, Quaternion.identity);
            newMagic.GetComponent<BasicMagic>().SetDamage(playerStats.damage);
            newMagic.SetActive(false);
            magicAmmo.Add(newMagic);
        }
    }

    private int FindMagicAmmo()
    {
        for (int i = 0; i < magicAmmo.Count; i++)
        {
            if (!magicAmmo[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
