using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public int damagePerTick = 200; // Damage per tick
    public float damageInterval = 0.12f; // Time interval between damage ticks

    private float nextDamageTime;
    private bool isPlayerInside;

    // Called continuously while the player stays within the trigger collider
    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // If the player stays inside the trigger, continuously apply damage
            if (Time.time >= nextDamageTime)
            {
                // Get the player's health component
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    // Apply damage to the player
                    player.TakeDamage(damagePerTick);
                }

                // Update the next damage time
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }
}
