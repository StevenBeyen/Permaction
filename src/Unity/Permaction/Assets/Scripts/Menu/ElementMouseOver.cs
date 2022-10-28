using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ElementMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Color originalColor;

    public void OnPointerEnter(PointerEventData eventData)
    {
        var image = GetComponent<Image>();
        originalColor = image.color;
        var tempColor = image.color;
        tempColor.a = 1f;
        image.color = tempColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var image = GetComponent<Image>();
        image.color = originalColor;
    }
}
