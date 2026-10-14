using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager Instance;
    private string lastOpenedPanel = "";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OpenARScene(string arSceneName, string currentPanel)
    {
        lastOpenedPanel = currentPanel;
        if (GameManager.Instance != null)
            GameManager.Instance.lastPanel = currentPanel;

        Debug.Log($"[NavigationManager] Opening AR Scene: {arSceneName} from panel: {currentPanel}");
        SceneManager.LoadScene(arSceneName);
    }

    public string GetLastOpenedPanel() => lastOpenedPanel;

    public void BackToModifikasi()
    {
        string targetPanel = GameManager.Instance?.lastPanel ?? "MAIN_PAGE";
        UIManager.targetPanel = targetPanel;
        Debug.Log($"[NavigationManager] Going back to Modifikasi, targetPanel = {targetPanel}");
        SceneManager.LoadScene("Modifikasi");
    }
}
