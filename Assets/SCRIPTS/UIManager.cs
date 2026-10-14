using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public static string targetPanel;

    [System.Serializable]
    public class PanelItem
    {
        public string panelName;
        public GameObject panelObject;
    }

    [Header("UI Panels")]
    public List<PanelItem> Panels = new List<PanelItem>();

    private void Awake()
    {
        // Ensure singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(targetPanel))
            targetPanel = "MAIN_PAGE";

        Debug.Log("[UIManager] Initialized. Target Panel: " + targetPanel);
        ShowPanel(targetPanel);
    }

    public void ShowPanel(string panelName)
    {
        foreach (var p in Panels)
        {
            if (p.panelObject != null)
                p.panelObject.SetActive(p.panelName == panelName);
        }

        targetPanel = panelName;
        Debug.Log($"[UIManager] Showing panel: {panelName}");
    }
}
