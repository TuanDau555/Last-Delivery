using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private TextMeshProUGUI lable;
    [SerializeField] private TMP_FontAsset regularFont;
    [SerializeField] private TMP_FontAsset boldFont;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        lable.font = boldFont;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        lable.font = regularFont;
    }
}
