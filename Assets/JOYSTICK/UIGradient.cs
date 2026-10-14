using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/UI Gradient")]
public class UIGradient : BaseMeshEffect
{
    public Color topColor = Color.white;
    public Color bottomColor = Color.black;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() || vh.currentVertCount == 0) return;

        UIVertex vertex = new UIVertex();
        int count = vh.currentVertCount;

        float height = GetComponent<RectTransform>().rect.height;

        for (int i = 0; i < count; i++)
        {
            vh.PopulateUIVertex(ref vertex, i);
            float ratio = (vertex.position.y - GetComponent<RectTransform>().rect.y) / height;
            vertex.color = Color.Lerp(bottomColor, topColor, ratio);
            vh.SetUIVertex(vertex, i);
        }
    }
}
