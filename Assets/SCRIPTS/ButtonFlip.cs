using UnityEngine;

public class HoldFlipBoth : MonoBehaviour
{
    public GameObject targetObject;  // The object to flip
    public float flipSpeed = 100f;   // Speed of flipping
    private float flipDirection = 0f; // -1 for left, 1 for right, 0 for stop

    void Update()
    {
        if (flipDirection != 0 && targetObject != null)
        {
            // Rotate around X axis (upside down vertically)
            targetObject.transform.Rotate(Vector3.right * flipSpeed * flipDirection * Time.deltaTime);
        }
    }

    // When Flip Left button is held
    public void OnHoldLeft()
    {
        flipDirection = -1f; // rotate one way
    }

    // When Flip Right button is held
    public void OnHoldRight()
    {
        flipDirection = 1f; // rotate opposite way
    }

    // When button is released
    public void OnRelease()
    {
        flipDirection = 0f; // stop rotation
    }
}
