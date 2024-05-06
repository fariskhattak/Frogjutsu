using UnityEngine;
using System.Collections.Generic;

public class Mage : Player
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject magicAmmoPrefab;
    [SerializeField] private int manaCostPerShot = 10; // Add the mana cost per shot
    [SerializeField] private int manaRegenAmount = 1; // Mana to regenerate per tick
    private List<GameObject> magicAmmo = new List<GameObject>();
    [SerializeField] private int totalAmmo = 10;
    [SerializeField] private AudioClip attackSound;
    private float cooldownTimer;
    [SerializeField] private float manaRegenCooldown = 0.5f; // Time interval between mana regen
    private float manaRegenTimer;

    void Start()
    {
        playerStats.damage = 40;
        InitMagicAmmo();
    }

    public override void Update()
    {
        if (cooldownTimer > attackCooldown)
        {
            base.Update();
        }

        cooldownTimer += Time.deltaTime;

        // Regenerate mana over time
        manaRegenTimer += Time.deltaTime;
        if (manaRegenTimer >= manaRegenCooldown)
        {
            playerStats.RestoreMana(manaRegenAmount);
            manaBar.SetMana(playerStats.currentMana);
            Debug.Log("Mage current mana: " + playerStats.currentMana);
            manaRegenTimer = 0f;
        }
    }

    public override void StartAttack()
    {
        if (playerStats.currentMana >= manaCostPerShot)
        {
            base.StartAttack();
            playerStats.currentMana -= manaCostPerShot;
            manaBar.SetMana(playerStats.currentMana);
            cooldownTimer = 0;
        }
        else
        {
            Debug.Log("Not enough mana to shoot.");
        }
    }

    public void Shoot()
    {
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
