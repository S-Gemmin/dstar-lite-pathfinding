using UnityEngine;
using TMPro;
using System.Collections;

public class VertexVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro gCostText;
    [SerializeField] private TextMeshPro rhsCostText;
    [SerializeField] private TextMeshPro hCostText;
    [SerializeField] private TextMeshPro k1CostText;
    [SerializeField] private SpriteRenderer backgroundSpriteRenderer;

    private Color lastColor;
    private bool flashing = false;

    private void Start()
    {
        if (gCostText == null || rhsCostText == null || hCostText == null || k1CostText == null || backgroundSpriteRenderer == null)
            Debug.LogWarning("Ensure variable in inspector are initialized!");

        lastColor = backgroundSpriteRenderer.color;
    }

    public void UpdateVertex(int hCost, int k1Cost)
    {
        hCostText.SetText(hCost == int.MaxValue ? "∞" : hCost.ToString());
        k1CostText.SetText(k1Cost == int.MaxValue ? "∞" : k1Cost.ToString());
    }

    public void SetGCost(int gCost)
    {
        gCostText.SetText(gCost == int.MaxValue ? "∞" : gCost.ToString());
    }

    public void SetRhsCost(int rhsCost)
    {
        rhsCostText.SetText(rhsCost == int.MaxValue ? "∞" : rhsCost.ToString());
    }

    public void SetBackgroundColor(Color color)
    {
        backgroundSpriteRenderer.color = color;
    }

    public IEnumerator FlashWhite(float duration = 0.1f)
    {
        if (flashing) yield break;

        flashing = true;
        lastColor = backgroundSpriteRenderer.color;
        backgroundSpriteRenderer.color = Color.white;
        yield return new WaitForSeconds(duration);
        backgroundSpriteRenderer.color = lastColor;
        flashing = false;
    }
}
