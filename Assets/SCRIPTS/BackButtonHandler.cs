using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonHandler : MonoBehaviour
{
    public void BackToModifikasi()
    {
        string targetPanel = GameManager.Instance != null
            ? GameManager.Instance.GetLastPanel()
            : "MAIN_PAGE";

        UIManager.targetPanel = targetPanel;
        Debug.Log($"[BackButtonHandler] Returning to Modifikasi, targetPanel = {targetPanel}");
        SceneManager.LoadScene("Modifikasi");
    }
}
