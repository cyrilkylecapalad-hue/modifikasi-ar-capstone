using System.Linq;
using UnityEngine;
using TMPro;

public class PriceSorter : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public TMP_Dropdown priceDropdown;
    public Transform contentParent;
    public GameObject cancelButton;
    public GameObject defaultPartsPanel;  // The 4 external parts panel
    public GameObject partScrollView;     // The ScrollView showing parts
    public TMP_InputField searchInput;

    private TMP_Text captionText;
    private string defaultLabel = "Price";

    private void OnEnable()
    {
        captionText = priceDropdown.captionText;
        ResetDropdown();
        priceDropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    private void OnDisable()
    {
        priceDropdown.onValueChanged.RemoveListener(OnDropdownChanged);
    }

    private void OnDropdownChanged(int index)
    {
        switch (index)
        {
            case 0: // Low to High
                captionText.text = "Low to High";
                ToggleViews(showParts: true);
                SortItems(true);
                ShowCancelButton(true);
                break;

            case 1: // High to Low
                captionText.text = "High to Low";
                ToggleViews(showParts: true);
                SortItems(false);
                ShowCancelButton(true);
                break;

            default:
                ResetDropdown();
                break;
        }
    }

    private void SortItems(bool ascending)
    {
        if (contentParent == null) return;

        var items = contentParent.GetComponentsInChildren<PartItem>(true);

        // Make sure each item updates its latest price before sorting
        foreach (var item in items)
            item.UpdatePriceValue();

        var sorted = ascending
            ? items.OrderBy(i => i.GetPrice()).ToList()
            : items.OrderByDescending(i => i.GetPrice()).ToList();

        for (int i = 0; i < sorted.Count; i++)
            sorted[i].transform.SetSiblingIndex(i);
    }

    public void OnCancelPressed()
    {
        // Reset dropdown and text
        ResetDropdown();

        // Clear search field
        if (searchInput != null)
            searchInput.text = "";

        // Show external parts again, hide part list
        ToggleViews(showParts: false);
        ShowCancelButton(false);
    }

    private void ResetDropdown()
    {
        // Reset the dropdown to default "Price" label
        priceDropdown.value = -1;
        captionText.text = defaultLabel;
    }

    private void ShowCancelButton(bool show)
    {
        if (cancelButton != null)
            cancelButton.SetActive(show);
    }

    private void ToggleViews(bool showParts)
    {
        if (defaultPartsPanel != null)
            defaultPartsPanel.SetActive(!showParts);

        if (partScrollView != null)
            partScrollView.SetActive(showParts);
    }
}
