using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class CardVisual : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Card Data")]
    public Card cardData;

    [Header("UI References")]
    public Image cardBackground;
    public Image cardArt;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI effectText;

    [Header("Drag Settings")]
    public bool isDraggable = true;
    public float dragScale = 1.1f;

    //runtime(hopefully)
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Canvas parentCanvas;
    private bool isBeingDragged = false;

    void Start()
    {
        parentCanvas = GetComponentInParent<Canvas>();
        SetupCard();
    }
    public void SetupCard(Card newCardData = null)
    {
        if (newCardData != null)
            cardData = newCardData;
        if (cardData == null)
        {
            Debug.LogWarning("No card assigned to CardVisual");
            return;
        }
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        // Set basic info
        if (cardNameText != null)
            cardNameText.text = cardData.cardName;

        if (damageText != null)
            damageText.text = cardData.GetFinalDamage().ToString();

        if (effectText != null)
            effectText.text = cardData.effectDescription;

        // Set colors and art
        if (cardBackground != null)
            cardBackground.color = cardData.cardColor;

        if (cardArt != null && cardData.cardArt != null)
        {
            cardArt.sprite = cardData.cardArt;
        }
        else if (cardArt != null)
        {
            // Create placeholder colored square
            CreatePlaceholderArt();
        }
    }
    // Create a simple placeholder graphic
    void CreatePlaceholderArt()
    {
        // For now, just tint the art based on card type
        Color placeholderColor = Color.gray;
        switch (cardData.cardType)
        {
            case CardType.Damage:
                placeholderColor = Color.red;
                break;
            case CardType.Modifier:
                placeholderColor = Color.blue;
                break;
            case CardType.Utility:
                placeholderColor = Color.green;
                break;
        }
        cardArt.color = placeholderColor;
    }

    #region Drag and Drop
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        isBeingDragged = true;
        originalPosition = transform.position;
        originalScale = transform.localScale;

        // Scale up and bring to front
        transform.localScale = originalScale * dragScale;
        transform.SetAsLastSibling();

        Debug.Log($"Started dragging {cardData.cardName}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        // Follow mouse/touch
        Vector2 screenPosition = eventData.position;
        Vector3 worldPosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            parentCanvas.transform as RectTransform,
            screenPosition,
            parentCanvas.worldCamera,
            out worldPosition
        );

        transform.position = worldPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        isBeingDragged = false;
        transform.localScale = originalScale;

        // Check if dropped on valid slot (to be implemented)
        if (!CheckForValidDrop(eventData))
        {
            // Return to original position
            transform.position = originalPosition;
            Debug.Log($"Returned {cardData.cardName} to hand");
        }
    }

    // Placeholder for slot detection
    bool CheckForValidDrop(PointerEventData eventData)
    {
        // This will be connected to the clock slot system later
        Debug.Log("Checking for valid drop location...");
        return false; // For now, always return to hand
    }
    #endregion

    // Public method to reset card position
    public void ReturnToHand()
    {
        if (originalPosition != Vector3.zero)
        {
            transform.position = originalPosition;
        }
        transform.localScale = originalScale;
        isBeingDragged = false;
    }
}
