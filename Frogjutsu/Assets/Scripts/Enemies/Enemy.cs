using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected int damage = 10; // Damage dealt by the enemy

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            collision.GetComponent<Player>().TakeDamage(damage);
        }
    }
}