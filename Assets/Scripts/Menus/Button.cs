using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Text text;
    public Color initialColor;
    public Color hoverColor = new Color(13f, 41f, 0f, 1f);
    public Color pressedColor = new Color(65f, 24f, 0f, 1f);

    private void Start()
    {
        initialColor = text.color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = hoverColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = initialColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        text.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        text.color = initialColor;
    }
}
