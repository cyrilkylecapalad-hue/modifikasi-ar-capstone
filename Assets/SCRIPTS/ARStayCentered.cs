using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARStayCentered : MonoBehaviour
{
    public Camera arCamera;
    public float distanceFromCamera = 0.5f;

    void Start()
    {
        if (arCamera == null)
            arCamera = Camera.main;
    }

    void LateUpdate()
    {
        // Keep object always in front of the camera
        transform.position = arCamera.transform.position + arCamera.transform.forward * distanceFromCamera;
        transform.rotation = Quaternion.LookRotation(arCamera.transform.forward);
    }
}
