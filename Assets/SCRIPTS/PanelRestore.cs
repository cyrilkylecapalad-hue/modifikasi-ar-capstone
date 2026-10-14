using UnityEngine;
using System.Collections;

public class PanelRestorer : MonoBehaviour
{
    public UIManager uiManager;

    private void Start()
    {
        StartCoroutine(RestorePanel());
    }

    private IEnumerator RestorePanel()
    {
        yield return null;

        string lastPanel = GameManager.Instance?.GetLastPanel() ?? UIManager.targetPanel ?? "MAIN_PAGE";
        Debug.Log($"[PanelRestorer] Restoring panel: {lastPanel}");

        if (uiManager != null)
            uiManager.ShowPanel(lastPanel);
        else
            Debug.LogError("[PanelRestorer] UIManager not assigned!");
    }
}
