
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
    private Collider2D collider;
    private Animator animator; // Animator component

    void Start()
    {
        nextSlamTime = Time.time + slamInterval; // Set initial slam time
        originalPosition = transform.position; // Store the original position
        collider = GetComponent<Collider2D>(); // Get the collider
        animator = GetComponent<Animator>(); // Get the animator component
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


        yield return new WaitForSeconds(warningDuration);

        // Calculate the target position taking collider size into account
        float colliderBottom = collider.bounds.min.y;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, colliderBottom), Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));
        float offset = collider.bounds.extents.y; // Half of the height of the collider
        float targetY = hit.point.y + offset + 0.38f; // Adjust the target Y position to be above the ground by the Rock Head's height, plus a slight offset
        Vector3 targetPosition = new Vector3(hit.point.x, targetY, transform.position.z); // Maintain the same X and Z coordinates


        if (hit.collider != null)
        {

            float slamDuration = 0.5f; // Duration of the slam movement

            float startTime = Time.time;

            // Move towards the slam position
            while (Time.time - startTime < slamDuration)
            {
                float t = (Time.time - startTime) / slamDuration;
                t = t * t; // Square the time value for faster movement
                transform.position = Vector3.Lerp(originalPosition, targetPosition, t);
                yield return null;
            }

            // Wait for 2 seconds before returning to original position
            yield return new WaitForSeconds(2f);

            // Return to the original position
            startTime = Time.time;
            while (Time.time - startTime < slamDuration)
            {
                float t = (Time.time - startTime) / slamDuration;
                t = Mathf.Sqrt(t); // Use square root for slower movement
                transform.position = Vector3.Lerp(targetPosition, originalPosition, t);
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