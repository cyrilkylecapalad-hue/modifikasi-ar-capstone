using UnityEngine;

public class DiscControl : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private FixedJoystick fixedJoystick;
    private Rigidbody rigidbody;

    private bool moveUp = false;
    private bool moveDown = false;

    private void OnEnable()
    {
        // Use the new Unity method instead of obsolete FindObjectOfType
        fixedJoystick = Object.FindFirstObjectByType<FixedJoystick>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float xVal = fixedJoystick.Horizontal;
        float zVal = fixedJoystick.Vertical;

        Vector3 movement = new Vector3(xVal, 0, zVal);

        if (moveUp) movement.y = 1;
        else if (moveDown) movement.y = -1;

        rigidbody.linearVelocity = movement * speed;

        // Keep X=90, only rotate around Y
        if (new Vector3(xVal, 0, zVal) != Vector3.zero)
        {
            float angle = Mathf.Atan2(xVal, zVal) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(90, angle, 0);
        }
    }

    public void StartMoveUp() { moveUp = true; }
    public void StopMoveUp() { moveUp = false; }
    public void StartMoveDown() { moveDown = true; }
    public void StopMoveDown() { moveDown = false; }
}