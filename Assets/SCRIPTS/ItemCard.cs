using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class PartItem : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text priceText;

    private float priceValue;

    private void Awake()
    {
        UpdatePriceValue();
    }

    // Extracts the numeric value from any text format like "P: 3000", "₱25,000", etc.
    public void UpdatePriceValue()
    {
        if (priceText != null && !string.IsNullOrEmpty(priceText.text))
        {
            // Remove everything except digits and decimal points
            string numericText = Regex.Replace(priceText.text, @"[^\d.]", "");

            // If the text had commas, remove them
            numericText = numericText.Replace(",", "");

            // Try to parse as float
            if (!float.TryParse(numericText, out priceValue))
                priceValue = 0; // fallback if text invalid
        }
    }

    // Used by PriceSorter
    public float GetPrice()
    {
        return priceValue;
    }
}
