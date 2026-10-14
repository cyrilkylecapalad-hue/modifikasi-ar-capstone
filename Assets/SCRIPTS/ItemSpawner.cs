using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

#if UNITY_ANDROID
[DefaultExecutionOrder(100)]
public class ItemSpawner : MonoBehaviour
{
    [Header("AR References (optional)")]
    [Tooltip("Optional - if you want plane placement. Can be left unassigned.")]
    public ARRaycastManager raycastManager;

    [Header("Prefab")]
    public GameObject objectPrefab;

    [Header("Fallback spawn")]
    public float spawnDistance = 0.8f; // meters in front of camera when no plane

    private GameObject spawnedObject;
    private readonly List<ARRaycastHit> hits = new();

    void Start()
    {
        // If raycastManager was not assigned in inspector, try to find one (XR Origin usually holds it)
        if (raycastManager == null)
            raycastManager = FindAnyObjectByType<ARRaycastManager>();
        SpawnAtCameraForward();
    }

    // Spawn and replace existing
    public void SpawnAtCameraForward()
    {
        if (objectPrefab == null) return;

        // Destroy previous
        if (spawnedObject != null)
            Destroy(spawnedObject);

        Vector3 spawnPos;
        Quaternion spawnRot = Quaternion.identity;

        // try center-screen plane first (if raycastManager available)
        if (raycastManager != null)
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            if (raycastManager.Raycast(screenCenter, hits, TrackableType.Planes))
            {
                // use the best hit
                Pose p = hits[0].pose;
                spawnPos = p.position;
                spawnRot = p.rotation;
            }
            else
            {
                // fallback to camera forward
                spawnPos = Camera.main.transform.position + Camera.main.transform.forward * spawnDistance;
                spawnRot = Quaternion.LookRotation(-Camera.main.transform.forward);
            }
        }
        else
        {
            spawnPos = Camera.main.transform.position + Camera.main.transform.forward * spawnDistance;
            spawnRot = Quaternion.LookRotation(-Camera.main.transform.forward);
        }

        spawnedObject = Instantiate(objectPrefab, spawnPos, spawnRot);
        spawnedObject.name = objectPrefab.name; // remove (Clone) naming
        // Ensure spawned object is root (not parented under XR Origin)
        spawnedObject.transform.SetParent(null, worldPositionStays: true);
    }

    // Optional method to call from UI to respawn/replace manually
    public void Respawn()
    {
        SpawnAtCameraForward();
    }
}
#endif
