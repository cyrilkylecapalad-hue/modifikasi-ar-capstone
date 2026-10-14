using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.Text;

public class SupabaseInventory : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text PriceTxt;

    [Header("Model to Query (Must Match Supabase 'Model' Column)")]
    public string ModelName;

    // Supabase credentials
    private string supabaseUrl = "https://sqjklzzaywpqqstdzbey.supabase.co/rest/v1/inventory_parts";
    private string apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InNxamtsenpheXdwcXFzdGR6YmV5Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTE2NDgwMzAsImV4cCI6MjA2NzIyNDAzMH0.7Mhs8scwP5QZSTti4C-QxvGfDVx9tRHFwTkOnCXPptU";

    // To keep original TMP settings
    private TMP_FontAsset originalFont;
    private float originalFontSize;
    private Color originalColor;
    private FontStyles originalStyle;

    void Start()
    {
        if (PriceTxt == null)
        {
            Debug.LogError("❌ PriceTxt is not assigned in the Inspector!");
            return;
        }

        // Store the TMP font style settings
        originalFont = PriceTxt.font;
        originalFontSize = PriceTxt.fontSize;
        originalColor = PriceTxt.color;
        originalStyle = PriceTxt.fontStyle;

        if (!string.IsNullOrEmpty(ModelName))
            StartCoroutine(GetInventory(ModelName));
        else
            Debug.LogWarning("⚠️ No ModelName set for SupabaseInventory!");
    }

    IEnumerator GetInventory(string modelName)
    {
        string encodedModel = UnityWebRequest.EscapeURL(modelName);
        string url = $"{supabaseUrl}?select=*&model=eq.{encodedModel}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("apikey", apiKey);
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("❌ Error fetching data: " + request.error + "\nResponse: " + request.downloadHandler.text);
                PriceTxt.text = "Error fetching data.";
            }
            else
            {
                string json = request.downloadHandler.text;
                Debug.Log("✅ Received Data: " + json);

                JArray data = JArray.Parse(json);

                if (data.Count > 0)
                {
                    JObject partData = (JObject)data[0];

                    string model = partData["Model"]?.ToString() ?? "Unknown";
                    int availability = partData["availability"]?.ToObject<int>() ?? 0;
                    float price = partData["price"]?.ToObject<float>() ?? 0f;

                    // Update text but preserve font settings
                    PriceTxt.text = $"Price: {price:0.00}\nQuantity: {availability}";
                    PriceTxt.font = originalFont;
                    PriceTxt.fontSize = originalFontSize;
                    PriceTxt.color = originalColor;
                    PriceTxt.fontStyle = availability <= 0 ? FontStyles.Strikethrough : originalStyle;
                }
                else
                {
                    PriceTxt.text = $"No data found for {modelName}";
                    PriceTxt.font = originalFont;
                    PriceTxt.fontSize = originalFontSize;
                    PriceTxt.color = originalColor;
                }
            }
        }
    }
}
