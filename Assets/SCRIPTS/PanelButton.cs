using UnityEngine;

public class PanelButton : MonoBehaviour
{
    public string arSceneName; // AR scene to open
    public string panelName;   // exact panel name (e.g., "shock_YA_786")

    public void OpenAR()
    {
        if (NavigationManager.Instance == null)
        {
            Debug.LogError("NavigationManager.Instance is null!");
            return;
        }

        NavigationManager.Instance.OpenARScene(arSceneName, panelName);
    }
}
