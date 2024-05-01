using UnityEngine;
using System.Collections;

public class Firetrap : MonoBehaviour
{
    protected int damage = 10;

    [Header("Firetrap Timers")]
    [SerializeField] private float activeTime;
    [SerializeField] private float cooldownTime;
    [SerializeField] private float damageInterval = 0.2f;
    private Animator anim;
    private bool active;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        // Start the coroutine to activate the firetrap periodically
        StartCoroutine(ActivateFiretrap());
    }

    private IEnumerator ActivateFiretrap()
    {
        while (true)
        {
            // Activate the firetrap
            Activate();

            // Wait for the active time
            yield return new WaitForSeconds(activeTime);

            // Deactivate the firetrap
            Deactivate();

            // Wait for the cooldown time
            yield return new WaitForSeconds(cooldownTime);
        }
    }

    private void Activate()
    {
        active = true;
        anim.SetBool("activated", true);
        // Start coroutine to continuously apply damage while the firetrap is active
        StartCoroutine(ApplyDamage());
    }

    private void Deactivate()
    {
        active = false;
        anim.SetBool("activated", false);
        // Stop coroutine for applying damage when firetrap is deactivated
        StopCoroutine(ApplyDamage());
    }

    private IEnumerator ApplyDamage()
    {
        while (active)
        {
            yield return new WaitForSeconds(damageInterval);

            // Apply damage to the player if they are within the firetrap's area
            Player player = FindObjectOfType<Player>(); // You may need a more sophisticated way to find the player
            if (player != null && IsPlayerInFiretrap(player.transform.position))
            {
                player.TakeDamage(damage);

            }
        }
    }

    private bool IsPlayerInFiretrap(Vector2 playerPosition)
    {
        // Calculate the distance between the player's position and the firetrap's center
        float distance = Vector2.Distance(playerPosition, transform.position);

        // If the distance is less than half the width of the firetrap's collider, the player is within the firetrap
        // Adjust the threshold as needed based on the size of your firetrap
        return distance < (GetComponent<Collider2D>().bounds.size.x / 2);
    }

}
