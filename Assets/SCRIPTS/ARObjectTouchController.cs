using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if UNITY_ANDROID
public class ARObjectController : MonoBehaviour
{
    [Header("UI Buttons (optional)")]
    public Button lockButton;
    public Button unlockButton;

    [Header("Rotate")]
    public float rotationSpeed = 80f;
    private bool rotateLeftHold = false;
    private bool rotateRightHold = false;

    [Header("Scale (Pinch)")]
    public float minScale = 0.3f;
    public float maxScale = 3f;
    public float pinchSensitivity = 0.01f;
    private bool pinchActive = false;
    private bool pinchStartedOnObject = false;
    private float initialPinchDistance;
    private Vector3 initialScale;

    [Header("Drag")]
    [Tooltip("How fast the object follows finger movement in world units.")]
    public float dragLerp = 8f;
    private bool dragging = false;
    private Vector3 dragOffsetWorld;
    private float dragObjectDistance;

    private Camera arCamera;
    private bool isLocked = false;

    void Start()
    {
        arCamera = Camera.main;

        // Setup lock/unlock buttons if assigned
        if (lockButton != null) lockButton.onClick.AddListener(ToggleLock);
        if (unlockButton != null) unlockButton.onClick.AddListener(ToggleLock);

        // default: unlocked (show lock logo, hide unlock logo)
        if (lockButton != null) lockButton.gameObject.SetActive(true);
        if (unlockButton != null) unlockButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isLocked) return; // when locked do nothing (frozen in place)

        HandleRotation();
        HandleTouches();
    }

    // ---------------- Rotation (called via EventTrigger)
    public void OnRotateLeftDown() => rotateLeftHold = true;
    public void OnRotateLeftUp() => rotateLeftHold = false;
    public void OnRotateRightDown() => rotateRightHold = true;
    public void OnRotateRightUp() => rotateRightHold = false;

    private void HandleRotation()
    {
        if (rotateLeftHold)
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        if (rotateRightHold)
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World);
    }

    // ---------------- Touch handling: accurate drag + pinch
    private void HandleTouches()
    {
        if (Input.touchCount == 0)
        {
            // reset states
            dragging = false;
            pinchActive = false;
            return;
        }

        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            // if pointer is over UI, ignore
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(t.fingerId)) return;

            if (t.phase == TouchPhase.Began)
            {
                // raycast into world to check if touch began on this object
                Ray r = arCamera.ScreenPointToRay(t.position);
                if (Physics.Raycast(r, out RaycastHit hit))
                {
                    // began on this object or on a child of it
                    if (hit.transform.IsChildOf(transform) || hit.transform == transform)
                    {
                        dragging = true;
                        // distance used to place object along ray
                        dragObjectDistance = Vector3.Distance(arCamera.transform.position, transform.position);
                        // compute offset between object position and hit point
                        dragOffsetWorld = transform.position - hit.point;
                    }
                }
            }
            else if (t.phase == TouchPhase.Moved && dragging)
            {
                // move object along camera ray at the stored distance
                Ray r = arCamera.ScreenPointToRay(t.position);
                Vector3 worldPoint = r.GetPoint(dragObjectDistance);
                Vector3 target = worldPoint + dragOffsetWorld;
                transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * dragLerp);
            }
            else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            {
                dragging = false;
            }
        }
        else if (Input.touchCount == 2)
        {
            // PINCH: only when pinch started on object
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            // If pinch just began, test if either touch began on the object
            if (!pinchActive && (t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began))
            {
                pinchActive = true;
                pinchStartedOnObject = false;

                // check both touches for a hit on this object
                Ray r0 = arCamera.ScreenPointToRay(t0.position);
                Ray r1 = arCamera.ScreenPointToRay(t1.position);

                if (Physics.Raycast(r0, out RaycastHit h0) && (h0.transform.IsChildOf(transform) || h0.transform == transform))
                    pinchStartedOnObject = true;
                if (Physics.Raycast(r1, out RaycastHit h1) && (h1.transform.IsChildOf(transform) || h1.transform == transform))
                    pinchStartedOnObject = true;

                if (pinchStartedOnObject)
                {
                    initialPinchDistance = Vector2.Distance(t0.position, t1.position);
                    initialScale = transform.localScale;
                }
            }

            if (pinchActive && pinchStartedOnObject)
            {
                float currentDist = Vector2.Distance(t0.position, t1.position);
                if (Mathf.Approximately(initialPinchDistance, 0f)) initialPinchDistance = currentDist > 0f ? currentDist : 1f;
                float ratio = currentDist / initialPinchDistance;
                Vector3 newScale = initialScale * ratio;
                float clamped = Mathf.Clamp(newScale.x, minScale, maxScale);
                transform.localScale = Vector3.one * clamped;
            }

            // end conditions
            if (t0.phase == TouchPhase.Ended || t1.phase == TouchPhase.Ended ||
                t0.phase == TouchPhase.Canceled || t1.phase == TouchPhase.Canceled)
            {
                pinchActive = false;
                pinchStartedOnObject = false;
            }
        }
    }

    // ---------------- Lock/Unlock
    public void ToggleLock()
    {
        isLocked = !isLocked;
        if (lockButton != null) lockButton.gameObject.SetActive(!isLocked);
        if (unlockButton != null) unlockButton.gameObject.SetActive(isLocked);
    }
}
#endif
