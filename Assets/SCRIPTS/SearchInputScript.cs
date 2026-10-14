using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class SearchBarFilter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_InputField searchInput;
    [SerializeField] private GameObject scrollView;          // The item list (scroll view)
    [SerializeField] private Transform contentParent;        // Inside scroll view
    [SerializeField] private GameObject externalPartsPanel;  // The 4 main external parts panel

    private void Start()
    {
        searchInput.onValueChanged.AddListener(OnSearchChanged);
        searchInput.onSelect.AddListener(OnSearchSelected);
        ResetSearchUI();
    }

    private void OnEnable()
    {
        // Reset after UI fully loads
        StartCoroutine(DelayedReset());
    }

    private void OnDisable()
    {
        // Clear input text immediately when leaving this page
        ForceClearInputInstant();
    }

    private IEnumerator DelayedReset()
    {
        yield return null; // wait one frame
        ResetSearchUI();
    }

    private void OnSearchSelected(string _)
    {
        if (!string.IsNullOrEmpty(searchInput.text))
        {
            externalPartsPanel.SetActive(false);
            scrollView.SetActive(true);
        }
    }

    private void OnSearchChanged(string searchText)
    {
        searchText = searchText.ToLower();
        bool hasText = !string.IsNullOrEmpty(searchText);

        // Toggle visibility
        externalPartsPanel.SetActive(!hasText);
        scrollView.SetActive(hasText);

        // Filter the list
        foreach (Transform item in contentParent)
        {
            TMP_Text partName = item.GetComponentInChildren<TMP_Text>();
            if (partName != null)
            {
                bool match = partName.text.ToLower().Contains(searchText);
                item.gameObject.SetActive(match || string.IsNullOrEmpty(searchText));
            }
        }
    }

    /// <summary>
    /// Fully resets the search UI, clears text, and restores panels.
    /// </summary>
    public void ResetSearchUI()
    {
        searchInput.text = "";
        searchInput.ForceLabelUpdate(); // ensures it visually clears
        searchInput.DeactivateInputField();
        EventSystem.current?.SetSelectedGameObject(null);

        scrollView.SetActive(false);
        externalPartsPanel.SetActive(true);

        foreach (Transform item in contentParent)
        {
            item.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Clears the text instantly when switching away from this page (even if disabled).
    /// </summary>
    private void ForceClearInputInstant()
    {
        if (searchInput != null)
        {
            searchInput.text = "";
            searchInput.ForceLabelUpdate();
            searchInput.DeactivateInputField();
        }
    }
}
