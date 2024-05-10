using UnityEngine;
using System.Collections.Generic;

public class Mage : Player
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject magicAmmoPrefab;
    [SerializeField] private GameObject specialMagicAmmoPrefab;
    [SerializeField] private int manaCostPerShot = 10; // Add the mana cost per shot
    private List<GameObject> magicAmmo = new List<GameObject>();
    private List<GameObject> specialMagicAmmo = new List<GameObject>();
    [SerializeField] private int totalAmmo = 10;
    [SerializeField] private AudioClip attackSound;
    private float cooldownTimer;

    void Start()
    {
        playerStats = playerStats.BaseMageStats();
        PlayerManager.Instance.playerStats = playerStats;
        InitMagicAmmo();
        InitSpecialMagicAmmo();
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
        if (!specialAbilityActivated)
        {
            playerStats.currentMana -= manaCostPerShot;
            manaBar.SetMana(playerStats.currentMana);
            int magicNum = FindMagicAmmo();
            magicAmmo[magicNum].transform.position = firePoint.position;
            magicAmmo[magicNum].GetComponent<BasicMagic>().SetDirection(Mathf.Sign(transform.localScale.x));
        }
        else
        {
            int magicNum = FindSpecialMagicAmmo();
            specialMagicAmmo[magicNum].transform.position = firePoint.position;
            specialMagicAmmo[magicNum].GetComponent<BasicMagic>().SetDirection(Mathf.Sign(transform.localScale.x));
        }

        cooldownTimer = 0;
        SoundManager.instance.PlaySound(attackSound);

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

    private void InitSpecialMagicAmmo()
    {
        for (int i = 0; i < totalAmmo; i++)
        {
            GameObject newMagic = Instantiate(specialMagicAmmoPrefab, transform.position, Quaternion.identity);
            newMagic.GetComponent<BasicMagic>().SetDamage(playerStats.damage * 2);
            newMagic.SetActive(false);
            specialMagicAmmo.Add(newMagic);
        }
    }

    private int FindSpecialMagicAmmo()
    {
        for (int i = 0; i < specialMagicAmmo.Count; i++)
        {
            if (!specialMagicAmmo[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
