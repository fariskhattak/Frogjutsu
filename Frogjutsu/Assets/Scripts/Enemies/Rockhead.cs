using UnityEngine;
using System.Collections;

public class RockHeadEnemy : MonoBehaviour
{
    public float slamInterval = 3f; // Time interval between each slam
    public float warningDuration = 1; // Duration of the warning indicator
    protected int damage = 300; // Damage dealt by the enemy

    private float nextSlamTime; // Time when the next slam will occur
    private bool isSlamming; // Flag to track if the rock head is currently slamming
    private Vector3 originalPosition; // Original position of the rock head

    void Start()
    {
        nextSlamTime = Time.time + slamInterval; // Set initial slam time
        originalPosition = transform.position; // Store the original position
    }

    void Update()
    {
        if (Time.time >= nextSlamTime && !isSlamming)
        {
            StartCoroutine(StartSlamSequence());
        }
    }

    IEnumerator StartSlamSequence()
    {
        isSlamming = true;

        Debug.Log("Warning: Rock Head is about to slam!");

        yield return new WaitForSeconds(warningDuration);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            float slamDuration = 0.1f;

            Vector3 targetPosition = hit.point;

            float startTime = Time.time;
            float elapsedTime = 0f;

            while (elapsedTime < slamDuration)
            {
                // Faster interpolation on the way down
                float t = elapsedTime / slamDuration;
                t = t * t; // Square the time value for faster movement
                transform.position = Vector3.Lerp(originalPosition, targetPosition, t);
                elapsedTime = Time.time - startTime;
                yield return null;
            }

            // Wait for 2 seconds before returning to original position
            yield return new WaitForSeconds(2f);

            startTime = Time.time;
            elapsedTime = 0f;

            while (elapsedTime < slamDuration)
            {
                // Slower interpolation on the way up
                float t = elapsedTime / slamDuration;
                t = Mathf.Sqrt(t); // Use square root for slower movement
                transform.position = Vector3.Lerp(targetPosition, originalPosition, t);
                elapsedTime = Time.time - startTime;
                yield return null;
            }
        }

        isSlamming = false;
        nextSlamTime = Time.time + slamInterval; // Update next slam time
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
