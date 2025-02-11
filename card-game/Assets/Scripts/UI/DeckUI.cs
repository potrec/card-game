using System;
using UnityEngine;
using UnityEngine.Serialization;

public class DeckUI : CardPileUI
{
    [SerializeField] private Transform deckCardsTemplate;
    public void OnEnable()
    {
        UpdateVisuals();
    }
    
    public void UpdateVisuals()
    {
        foreach (Transform child in cardsGrid)
        {
            Destroy(child.gameObject);
        }
        
        foreach (var deckCard in DeckManager.Instance.deckCards)
        {
            var card = Instantiate(deckCardsTemplate, cardsGrid);
            var cardScript = card.GetComponent<DeckCard>();
            cardScript.Initialize(deckCard.amount, deckCard.card);
        }
    }
}
