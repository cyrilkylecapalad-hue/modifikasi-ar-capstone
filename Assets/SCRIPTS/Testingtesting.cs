using UnityEngine;
using UnityEngine.UI;

public class ForceLayoutRefresh : MonoBehaviour
{
    public RectTransform content;

    void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
    }
}