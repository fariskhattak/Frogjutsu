using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Firetrap : MonoBehaviour
{
    protected int damage = 10;

    [Header ("Firetrap Timers")]
    [SerializeField] public float activationDelay;
    [SerializeField] public float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered;
    private bool active;

    private void Awake() {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public virtual void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player" )
        {
            if (!triggered) {
                StartCoroutine(ActivateFiretrap());
            }
            if (triggered) {
                if (active)
                collision.GetComponent<Player>().TakeDamage(damage);
            }
        }
    }

    private IEnumerator ActivateFiretrap() {
        triggered = true;
        spriteRend.color = Color.red;

        yield return new WaitForSeconds(activationDelay);
        spriteRend.color = Color.white;
        active = true;
        anim.SetBool("activated", true);

        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
}
