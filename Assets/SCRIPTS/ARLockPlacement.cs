using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARLockPlacement : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public ARRaycastManager raycastManager;
    private bool isLocked = false;

    public void LockObject()
    {
        isLocked = true;
        planeManager.enabled = false;
        foreach (var plane in planeManager.trackables)
            plane.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isLocked)
        {
            // (Optional) allow placement logic here
        }
    }
}
