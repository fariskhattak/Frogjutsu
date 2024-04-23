using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private float fallDelay = 0.01f;
    private float destroyDelay = 2f;
    private float fallSpeed = 1.0f; // Adjust this value to control fall speed
    private Vector3 initialPosition;
    private bool isFalling = false;

    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        isFalling = true;
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = fallSpeed; // Set the gravity scale to control fall speed
        yield return new WaitForSeconds(destroyDelay);
        ResetPlatform();
    }

    private void ResetPlatform()
    {
        rb.bodyType = RigidbodyType2D.Static;
        transform.position = initialPosition;
        rb.gravityScale = 0f;
        isFalling = false;
    }
}
