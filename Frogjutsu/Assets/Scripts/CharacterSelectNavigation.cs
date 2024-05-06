using UnityEngine;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;

    private float horizontalAxis;
    private float axisThreshold = 0.5f;
    private bool canNavigate = true;
    private float cooldownTime = 0.2f; // Adjust this time to set the navigation speed
    private float cooldownTimer = 0f;

    void Update()
    {
        // Check if we're currently in cooldown
        if (!canNavigate)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownTime)
            {
                canNavigate = true;
                cooldownTimer = 0f;
            }
            return; // Skip the rest of the update if we're on cooldown
        }

        // Get the horizontal axis input
        horizontalAxis = Input.GetAxis("Horizontal");

        // Check for left/right input with a threshold to avoid sensitivity issues
        if (horizontalAxis < -axisThreshold)
        {
            NavigateLeft();
            canNavigate = false; // Start the cooldown
        }
        else if (horizontalAxis > axisThreshold)
        {
            NavigateRight();
            canNavigate = false; // Start the cooldown
        }
    }

    void NavigateLeft()
    {
        // Simulate clicking the left button
        leftButton.onClick.Invoke();
    }

    void NavigateRight()
    {
        // Simulate clicking the right button
        rightButton.onClick.Invoke();
    }
}
