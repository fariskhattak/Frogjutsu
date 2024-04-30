using UnityEngine;
using System.Collections;

public class Firetrap : MonoBehaviour
{
    protected int damage = 10;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered;
    private bool active;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    // OnTriggerEnter2D is called when a collider enters the trigger zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Use CompareTag for better performance
        {
            if (!triggered)
            {
                StartCoroutine(ActivateFiretrap());
            }
            if (triggered && active)
            {
                // Ensure that the TakeDamage method is called on the Player component
                collision.GetComponent<Player>().TakeDamage(damage);
            }
        }
    }

    private IEnumerator ActivateFiretrap()
    {
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
