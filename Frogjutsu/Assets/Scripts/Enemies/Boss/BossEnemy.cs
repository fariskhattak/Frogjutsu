using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    // Start is called before the first frame update
    public HealthBar healthBar;
    
    void Start()
    {
        damage = 100;
    }

}
