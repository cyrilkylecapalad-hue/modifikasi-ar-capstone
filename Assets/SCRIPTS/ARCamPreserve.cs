using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class ARCameraPreserver : MonoBehaviour
{
    private static ARCameraPreserver instance;
    private ARSession arSession;
    private ARCameraBackground cachedBackground;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        cachedBackground = GetComponent<ARCameraBackground>();
        // cache if present; not required
        arSession = FindAnyObjectByType<ARSession>();
    }

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        if (arSession == null)
            arSession = FindAnyObjectByType<ARSession>();

        if (cachedBackground == null)
            cachedBackground = GetComponent<ARCameraBackground>();
    }

    void Update()
    {
        // Defensive: if main camera destroyed briefly, stop trying to access it
        if (Camera.main == null) return;
    }
}
