using UnityEngine;

public class IceBlock : MonoBehaviour
{
    // Speed multiplier to increase player's horizontal speed
    public float horizontalSpeedMultiplier = 2f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = collider.GetComponent<Rigidbody2D>();
            Player playerScript = collider.GetComponent<Player>(); // Get the Player component

            if (playerRigidbody != null && playerScript != null)
            {
                // Multiply the player's current horizontal velocity by the horizontalSpeedMultiplier
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x * horizontalSpeedMultiplier, playerRigidbody.velocity.y);

                // Indicate that the player's speed has been boosted by the ice block
                playerScript.SetIceBlockSpeedBoosted(true);
            }
        }
    }
}
