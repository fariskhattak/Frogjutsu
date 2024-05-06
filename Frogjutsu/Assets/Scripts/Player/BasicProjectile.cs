using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;
    private float damage;

    [SerializeField] private LayerMask enemies;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Ignore collision with players or other projectiles
        if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Arrow" || collider.gameObject.tag == "Checkpoint")
        {
            return;
        }
        hit = true;
        // Check if the object has an animator component
        boxCollider.enabled = false;
        // Check if the collider's layer name matches the name of the "enemies" layer
        if (((1 << collider.gameObject.layer) & enemies) != 0)
        {
            // Get the EnemyHealth component from the collided GameObject
            EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();

            // If EnemyHealth component exists, deal damage
            if (enemyHealth != null)
            {
                // Call a method in EnemyHealth to deal damage
                enemyHealth.TakeDamage(damage);
            }
        }
        Deactivate();
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
        Debug.Log("Arrow direction: " + transform.localScale.x);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
