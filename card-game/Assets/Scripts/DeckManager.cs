using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; }

    [Header("Deck Settings")]
    public int startingHandDeckSize = 3;
    public List<CardSO> deckCards;
    public List<CardSO> handCards = new List<CardSO>();
    public List<CardSO> tableCards = new List<CardSO>();
    public List<CardSO> discardPile = new List<CardSO>();
    
    public CardSO currentCard { get; set; } = null;
    
    public Transform basicCardPrefab;
    public Transform tableUI;
    public Transform handUI;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializeDeck();
    }

    public void InitializeDeck()
    {
        ShuffleDeck();
        DrawInitialHand();
        UpdateHandVisuals();
    }

    private void ShuffleDeck()
    {
        for (int i = 0; i < deckCards.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, deckCards.Count);
            (deckCards[randomIndex], deckCards[i]) = (deckCards[i], deckCards[randomIndex]);
        }
    }

    public void DrawInitialHand()
    {
        for (int i = 0; i < startingHandDeckSize; i++)
        {
            DrawCard();
        }
    }

    public void DrawCard()
    {
        if(deckCards.Count == 0) return;
        
        var card = deckCards[0];
        deckCards.RemoveAt(0);
        handCards.Add(card);
        InstantiateCard(card, handUI);
    }
    
    private void InstantiateCard(CardSO cardData, Transform parent)
    {
        var card = Instantiate(basicCardPrefab, parent);
        var cardScript = card.GetComponent<BasicCard>();
        
        cardScript.cardData = cardData;
        cardScript.Initialize();
    }

    public void UpdateHandVisuals()
    {
        foreach (Transform child in handUI)
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < handCards.Count; i++)
        {
            InstantiateCard(handCards[i], handUI);
        }
    }

    public void PlayCard(BasicCard card)
    {
        handCards.Remove(card.cardData);
        tableCards.Add(card.cardData);
        card.transform.SetParent(tableUI);
    }
}
