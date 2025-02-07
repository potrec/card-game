using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; }
    
    public List<CardSO> deckCards;
    public List<CardSO> handCards;
    public List<CardSO> tableCards;
    public List<CardSO> discardPile;
    
    public CardSO currentCard { get; set; } = null;
    
    public Transform basicCardPrefab;
    [SerializeField] private Transform deckUI;
    
    public Transform handUI;

    public float cardRotationInDeck = 2.5f;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateVisuals();
    }
    
    private void ShuffleDeck()
    {
        
    }
    
    public void UpdateVisuals()
    {
        foreach (Transform child in deckUI)
        {
            // if (child == basicCardPrefab) continue;
            Destroy(child.gameObject);
        }
        
        float cardRotation = cardRotationInDeck * (handCards.Count-1)/2;
        for (int i = 0; i < handCards.Count; i++)
        {
            var card = Instantiate(basicCardPrefab, deckUI);
            var cardScript = card.GetComponent<BasicCard>();
            
            cardScript.cardData = handCards[i];
            cardScript.Initialize();
            card.localRotation = Quaternion.Euler(0, 0, cardRotation-i*cardRotationInDeck);
        }
    }

}
