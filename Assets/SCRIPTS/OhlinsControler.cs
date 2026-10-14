using UnityEngine;

public class OhlinsController : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 50f;

    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;

    private void Update()
    {
        if (isRotatingLeft)
        {
            transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime, Space.World);
        }
        else if (isRotatingRight)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    // Called by Event Trigger (Pointer Down on Left button)
    public void StartRotateLeft()
    {
        isRotatingLeft = true;
    }

    // Called by Event Trigger (Pointer Up on Left button)
    public void StopRotateLeft()
    {
        isRotatingLeft = false;
    }

    // Called by Event Trigger (Pointer Down on Right button)
    public void StartRotateRight()
    {
        isRotatingRight = true;
    }

    // Called by Event Trigger (Pointer Up on Right button)
    public void StopRotateRight()
    {
        isRotatingRight = false;
    }
}
