using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using DG.Tweening;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; }

    [Header("Deck Settings")]
    public int startingHandDeckSize = 3;
    public List<CardSO> deckCards;
    public List<Transform> handCards = new List<Transform>();
    public List<Transform> tableCards = new List<Transform>();
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
        UpdateHandCards();
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
        DrawCards(startingHandDeckSize);
    }
    
    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            DrawCard();
        }
    }

    public void DrawCard()
    {
        if(deckCards.Count == 0) return;
        
        CardSO deckCardSO = deckCards[0];
        deckCards.RemoveAt(0);
        Transform card = InstantiateCard(deckCardSO, handUI);
        handCards.Add(card);
    }
    
    private Transform InstantiateCard(CardSO cardData, Transform parent)
    {
        var card = Instantiate(basicCardPrefab, parent);
        var cardScript = card.GetComponent<BasicCard>();
        
        cardScript.cardData = cardData;
        cardScript.Initialize();
        return card;
    }

    public void UpdateHandCards()
    {
        foreach (Transform child in handUI)
        {
            var card = child.GetComponent<BasicCard>();
            if (card == null)
            {
                Destroy(child);
                continue;
            }
            card.UpdateInitialTransform();
        }
    }

    public void PlayCard(Transform card)
    {
        handCards.Remove(card);
        tableCards.Add(card);
        card.transform.SetParent(tableUI);
    }
    
    public void EndTurn()
    {
        // discardPile.AddRange(tableCards);
        tableCards.Clear();

        foreach (Transform child in tableUI)
        {
            Destroy(child.gameObject);
        }
        
        DrawCards(startingHandDeckSize-handCards.Count);
        UpdateHandCards();
    }
}
