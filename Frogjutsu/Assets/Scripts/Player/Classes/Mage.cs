using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Player
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject magicAmmoPrefab; // Prefab of the arrow object
    private List<GameObject> magicAmmo = new List<GameObject>(); // Use a list instead of an array
    [SerializeField] private int totalAmmo = 10;
    private float cooldownTimer;

    void Start()
    {
        InitMagicAmmo();
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
        int magicNum = FindMagicAmmo();
        Debug.Log(magicNum);
        magicAmmo[magicNum].transform.position = firePoint.position;
        magicAmmo[magicNum].GetComponent<BasicMagic>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private void InitMagicAmmo()
    {
        // Instantiate new ammo and add them to the list
        for (int i = 0; i < totalAmmo; i++) // Example: instantiate 10 ammo
        {
            GameObject newMagic = Instantiate(magicAmmoPrefab, transform.position, Quaternion.identity);
            newMagic.GetComponent<BasicMagic>().SetDamage(damage);
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
