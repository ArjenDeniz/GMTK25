using UnityEngine;
using System.Collections.Generic;

public class HandManager : MonoBehaviour
{
    [Header("Hand Settings")]
    public Transform handContainer; // Parent object for hand cards
    public GameObject cardVisualPrefab; // Prefab with CardVisual component
    public List<Card> startingCards = new List<Card>(); // Cards to spawn at start

    [Header("Layout Settings")]
    public float cardSpacing = 120f;
    public Vector3 handOffset = Vector3.zero;

    [Header("Debug")]
    public bool autoSpawnCards = true;

    // Runtime data
    private List<CardVisual> currentHandCards = new List<CardVisual>();

    void Start()
    {
        if (autoSpawnCards)
        {
            SpawnStartingHand();
        }
    }

    // Create the initial hand of cards
    void SpawnStartingHand()
    {
        if (startingCards.Count == 0)
        {
            Debug.LogWarning("No starting cards assigned to HandManager!");
            return;
        }

        for (int i = 0; i < startingCards.Count; i++)
        {
            SpawnCard(startingCards[i], i);
        }

        Debug.Log($"Spawned {startingCards.Count} cards in hand");
    }

    // Spawn a single card in the hand
    public CardVisual SpawnCard(Card cardData, int handIndex)
    {
        if (cardVisualPrefab == null)
        {
            Debug.LogError("No card visual prefab assigned!");
            return null;
        }

        // Create the card GameObject
        GameObject cardObject = Instantiate(cardVisualPrefab, handContainer);
        CardVisual cardVisual = cardObject.GetComponent<CardVisual>();

        if (cardVisual == null)
        {
            Debug.LogError("Card prefab doesn't have CardVisual component!");
            Destroy(cardObject);
            return null;
        }

        // Set up the card
        cardVisual.SetupCard(cardData);

        // Position in hand
        PositionCardInHand(cardVisual, handIndex);

        // Add to our tracking list
        currentHandCards.Add(cardVisual);

        return cardVisual;
    }

    // Position a card in the hand layout
    void PositionCardInHand(CardVisual cardVisual, int index)
    {
        Vector3 position = handContainer.position + handOffset;

        // Simple horizontal layout
        float totalWidth = (currentHandCards.Count - 1) * cardSpacing;
        float startX = -totalWidth * 0.5f;
        position.x += startX + (index * cardSpacing);

        cardVisual.transform.position = position;
    }

    // Remove a card from hand (when played)
    public void RemoveCardFromHand(CardVisual cardVisual)
    {
        if (currentHandCards.Contains(cardVisual))
        {
            currentHandCards.Remove(cardVisual);
            Destroy(cardVisual.gameObject);

            // Reposition remaining cards
            ReorganizeHand();
        }
    }

    // Reorganize hand after card removal
    void ReorganizeHand()
    {
        for (int i = 0; i < currentHandCards.Count; i++)
        {
            PositionCardInHand(currentHandCards[i], i);
        }
    }

    // Add a new card to hand
    public void AddCardToHand(Card cardData)
    {
        SpawnCard(cardData, currentHandCards.Count);
    }

    // Get all cards currently in hand
    public List<Card> GetHandCards()
    {
        List<Card> handCards = new List<Card>();
        foreach (CardVisual cardVisual in currentHandCards)
        {
            handCards.Add(cardVisual.cardData);
        }
        return handCards;
    }

    // Debug method to test card spawning
    [ContextMenu("Test Spawn Random Card")]
    void TestSpawnCard()
    {
        if (startingCards.Count > 0)
        {
            Card randomCard = startingCards[Random.Range(0, startingCards.Count)];
            AddCardToHand(randomCard);
        }
    }
}