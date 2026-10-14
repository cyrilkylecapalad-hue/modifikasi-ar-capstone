using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController instance;

    private void Awake()
    {
        // ✅ Make sure only one SceneController exists across scenes
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 🔹 Load your main UI scene
    public void LoadModifikasi()
    {
        SceneManager.LoadScene("Modifikasi");
    }

    // 🔹 Generic function to load any AR scene (e.g. "AROhlins", "ARCaliper", etc.)
    public void LoadARScene(string arSceneName)
    {
        SceneManager.LoadScene(arSceneName);
    }
}
