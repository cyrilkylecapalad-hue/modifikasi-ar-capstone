using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.IO;

public class ARCapture : MonoBehaviour
{
    [Header("UI Elements")]
    public Button captureButton;
    public GameObject captureMessagePanel;
    public TMP_Text captureMessageText;

    [Header("UI To Hide During Capture")]
    public GameObject[] uiElementsToHide;

    private bool isCapturing = false;

    private void Start()
    {
        if (captureButton != null)
            captureButton.onClick.AddListener(OnCaptureButtonClicked);
        else
            Debug.LogWarning("⚠️ Capture Button not assigned in Inspector!");

        if (captureMessagePanel != null)
            captureMessagePanel.SetActive(false);
    }

    private void OnCaptureButtonClicked()
    {
        if (!isCapturing)
            StartCoroutine(CaptureRoutine());
    }

    private IEnumerator CaptureRoutine()
    {
        isCapturing = true;

        // 🔹 Hide UI
        foreach (var ui in uiElementsToHide)
            if (ui != null) ui.SetActive(false);

        yield return new WaitForEndOfFrame();

        // 🔹 Capture full screen into Texture2D
        Texture2D screenTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenTex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenTex.Apply();

        // 🔹 Folder setup
        string folderPath = Path.Combine(Application.persistentDataPath, "ARCaptures");
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        string fileName = "AR_Capture_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string fullPath = Path.Combine(folderPath, fileName);

        // 🔹 Save to internal path
        File.WriteAllBytes(fullPath, screenTex.EncodeToPNG());
        Debug.Log($"📸 Screenshot saved at: {fullPath}");

        // Free texture memory
        Destroy(screenTex);

#if UNITY_ANDROID && !UNITY_EDITOR
        // 🔹 Copy to DCIM/Pictures for visibility in gallery
        try
        {
            string galleryDir = "/storage/emulated/0/Pictures/ARCaptures";
            if (!Directory.Exists(galleryDir)) Directory.CreateDirectory(galleryDir);

            string galleryPath = Path.Combine(galleryDir, fileName);
            File.Copy(fullPath, galleryPath, true);

            using (AndroidJavaClass mediaScanner = new AndroidJavaClass("android.media.MediaScannerConnection"))
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                mediaScanner.CallStatic("scanFile", activity, new string[] { galleryPath }, null, null);
            }

            Debug.Log($"✅ Copied to gallery: {galleryPath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError("❌ Copy to gallery failed: " + e.Message);
        }
#endif

        // 🔹 Show popup message
        if (captureMessagePanel != null && captureMessageText != null)
        {
            captureMessageText.text = "Captured Successfully!";
            captureMessagePanel.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            captureMessagePanel.SetActive(false);
        }

        // 🔹 Show UI again
        foreach (var ui in uiElementsToHide)
            if (ui != null) ui.SetActive(true);

        isCapturing = false;
    }
}
