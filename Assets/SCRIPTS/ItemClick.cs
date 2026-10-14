using UnityEngine;
using UnityEngine.UI;

public class ItemClick : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject targetPage;            // The part page to open (e.g. CALIPER)
    [SerializeField] private GameObject externalPartsRoot;     // The EXTERNAL_PARTS main GameObject

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(OpenTargetPage);
    }

    private void OpenTargetPage()
    {
        // 1️⃣ Hide the External Parts panel completely
        if (externalPartsRoot != null && externalPartsRoot.activeSelf)
        {
            externalPartsRoot.SetActive(false);
        }

        // 2️⃣ Show the selected part page
        if (targetPage != null)
        {
            targetPage.SetActive(true);
        }

   
    }
}
