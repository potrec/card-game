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
    public DeckSO deckSO;
    public List<Transform> handCards = new List<Transform>();
    public List<Transform> tableCards = new List<Transform>();
    public List<CardSO> discardPile = new List<CardSO>();
    
    public CardSO currentCard { get; set; } = null;
    
    public Transform basicCardPrefab;
    public Transform tableUI;
    public Transform handUI;

    public List<DeckSO.DeckCards> deckCards;

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
        deckCards = new List<DeckSO.DeckCards>();
        
        foreach (var deckCard in deckSO.deckCards)
        {
            deckCards.Add(new DeckSO.DeckCards(deckCard.card, deckCard.amount));
        }
        
        DrawInitialHand();
        UpdateHandCards();
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
        if (deckCards.Count == 0) return;
        
        int randomCardIndex = UnityEngine.Random.Range(0, deckCards.Count);
        var cardData = deckCards[randomCardIndex].card;
        deckCards[randomCardIndex].amount--;
        if (deckCards[randomCardIndex].amount == 0)
        {
            deckCards.RemoveAt(randomCardIndex);
        }
        
        var card = InstantiateCard(cardData, handUI);
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
        discardPile.AddRange(tableCards.ConvertAll(c => c.GetComponent<BasicCard>().cardData));
        tableCards.Clear();

        foreach (Transform child in tableUI)
        {
            Destroy(child.gameObject);
        }
        
        DrawCards(startingHandDeckSize-handCards.Count);
        UpdateHandCards();
    }
}
