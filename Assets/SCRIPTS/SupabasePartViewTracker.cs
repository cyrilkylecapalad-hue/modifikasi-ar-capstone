using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json.Linq;

public class SupabasePartViewTracker : MonoBehaviour
{
    [Header("Supabase Settings")]
    private string supabaseUrl = "https://sqjklzzaywpqqstdzbey.supabase.co/rest/v1/inventory_parts";
    private string apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InNxamtsenpheXdwcXFzdGR6YmV5Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTE2NDgwMzAsImV4cCI6MjA2NzIyNDAzMH0.7Mhs8scwP5QZSTti4C-QxvGfDVx9tRHFwTkOnCXPptU";

    [Header("Part Info")]
    public string modelName; // Example: "STYLEMA R RADIAL 4POT"

    public void RecordPartView()
    {
        StartCoroutine(UpdatePartViews());
    }

    IEnumerator UpdatePartViews()
    {
        // Step 1: Get current views
        string getUrl = $"{supabaseUrl}?model=eq.{UnityWebRequest.EscapeURL(modelName)}&select=part_views";

        using (UnityWebRequest getRequest = UnityWebRequest.Get(getUrl))
        {
            getRequest.SetRequestHeader("apikey", apiKey);
            getRequest.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return getRequest.SendWebRequest();

            if (getRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("❌ Error fetching part_views: " + getRequest.error);
                yield break;
            }

            string json = getRequest.downloadHandler.text;
            JArray data = JArray.Parse(json);
            if (data.Count == 0)
            {
                Debug.LogWarning("⚠️ No part found with that product name.");
                yield break;
            }

            int currentViews = (int)data[0]["part_views"];
            int newViews = currentViews + 1;

            // Step 2: PATCH new value
            string patchUrl = $"{supabaseUrl}?model=eq.{UnityWebRequest.EscapeURL(modelName)}";
            string jsonBody = "{\"part_views\": " + newViews + "}";

            using (UnityWebRequest patchRequest = new UnityWebRequest(patchUrl, "PATCH"))
            {
                patchRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonBody));
                patchRequest.downloadHandler = new DownloadHandlerBuffer();

                patchRequest.SetRequestHeader("apikey", apiKey);
                patchRequest.SetRequestHeader("Authorization", "Bearer " + apiKey);
                patchRequest.SetRequestHeader("Content-Type", "application/json");
                patchRequest.SetRequestHeader("Prefer", "return=minimal");

                yield return patchRequest.SendWebRequest();

                if (patchRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("❌ Error updating part_views: " + patchRequest.error);
                }
                else
                {
                    Debug.Log("✅ part_views updated for: " + modelName + " (Now: " + newViews + ")");
                }
            }
        }
    }
}