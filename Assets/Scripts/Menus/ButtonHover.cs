using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image image;
    private Color originalImageColor;
    private Color hoveredImageColor;
    private TextMeshProUGUI text;
    private float originalTextSize = 24f;
    private float hoveredTextSize = 30f;

    [SerializeField] private bool enlargeButton;
    private Vector3 cachedScale;

    void Start()
    {
        image = GetComponent<Image>();
        originalImageColor = hoveredImageColor = image.color;
        hoveredImageColor.a = 1f;
        text = GetComponentInChildren<TextMeshProUGUI>();
        cachedScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!enlargeButton)
        {
            text.fontSize = hoveredTextSize;
            image.color = hoveredImageColor;
        }
        else
        {
            transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!enlargeButton)
        {
            text.fontSize = originalTextSize;
            image.color = originalImageColor;
        }
        else
        {
            transform.localScale = cachedScale;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!enlargeButton)
        {
            text.fontSize = originalTextSize;
            image.color = originalImageColor;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(enlargeButton)
            transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (enlargeButton)
            transform.localScale = cachedScale;
    }
}