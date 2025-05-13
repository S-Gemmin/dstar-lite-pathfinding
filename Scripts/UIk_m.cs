using UnityEngine;
using TMPro;

public class UIk_m : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI k_mText;

    private void Start()
    {
        DStarLite.i.Onk_mChanged += (k_m) => { k_mText.SetText($"k_m: {k_m.ToString()}"); };   
    }
}
