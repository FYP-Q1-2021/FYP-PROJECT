using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    private Color originalImageColor;
    private Color hoveredImageColor;
    private TextMeshProUGUI text;
    private float originalTextSize = 24f;
    private float hoveredTextSize = 30f;

    void Start()
    {
        image = GetComponent<Image>();
        originalImageColor = hoveredImageColor = image.color;
        hoveredImageColor.a = 1f;
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = Color.yellow;
        text.fontSize = hoveredTextSize;
        image.color = hoveredImageColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = Color.white;
        text.fontSize = originalTextSize;
        image.color = originalImageColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        text.color = Color.white;
        text.fontSize = originalTextSize;
        image.color = originalImageColor;
    }
}