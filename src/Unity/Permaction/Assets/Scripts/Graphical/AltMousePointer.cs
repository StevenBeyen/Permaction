using UnityEngine;
using UnityEngine.EventSystems;

public class AltMousePointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Texture2D cursorTexture;
    public Texture2D defaultCursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(defaultCursorTexture, hotSpot, cursorMode);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Cursor.SetCursor(defaultCursorTexture, hotSpot, cursorMode);
    }
}
