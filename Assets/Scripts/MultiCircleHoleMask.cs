using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MultiCircleHoleMask : MonoBehaviour
{
    public RectTransform[] holes;   // 여기에 Hole 동그라미들 드래그
    public float radiusScale = 0.5f; // Rect 크기에서 반지름 비율

    Material mat;
    RectTransform rt;
    Canvas canvas;

    void Awake()
    {
        var img = GetComponent<Image>();
        // Image가 복사해서 쓰는 인스턴스 머티리얼 확보
        mat = img.material;
        rt = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void LateUpdate()
    {
        if (mat == null || rt == null || holes == null) return;

        int count = Mathf.Min(holes.Length, 8);
        mat.SetInt("_HoleCount", count);

        for (int i = 0; i < count; i++)
        {
            var h = holes[i];
            if (h == null) continue;

            // Hole의 중심을 Overlay Rect 기준 0~1 좌표로 변환
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, h.position);

            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, screenPos, canvas.worldCamera, out localPos);

            // localPos (좌측하단 -width/2~ +width/2, -height/2~+height/2)
            float u = (localPos.x / rt.rect.width) + 0.5f;
            float v = (localPos.y / rt.rect.height) + 0.5f;

            // Hole 반지름 (Rect 가로/세로 중 작은 쪽 기준)
            float baseSize = Mathf.Min(h.rect.width, h.rect.height);
            float r = (baseSize / rt.rect.width) * radiusScale;

            mat.SetVector("_Holes" + i, new Vector4(u, v, r, 0));
        }
    }
}
