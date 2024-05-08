using UnityEngine;

public class IceBlock : MonoBehaviour
{
    // Speed multiplier to increase player's horizontal speed
    public float horizontalSpeedMultiplier = 2f;

    private Player player; // Reference to the player

    // Method to set the player's reference
    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && player != null)
        {
            Rigidbody2D playerRigidbody = collider.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                // Multiply the player's current horizontal velocity by the horizontalSpeedMultiplier
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x * horizontalSpeedMultiplier, playerRigidbody.velocity.y);

                // Indicate that the player's speed has been boosted by the ice block
                player.SetIceBlockSpeedBoosted(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && player != null)
        {
            // Reset the player's speed boost when they leave the ice block
            player.SetIceBlockSpeedBoosted(false);
        }
    }
}
