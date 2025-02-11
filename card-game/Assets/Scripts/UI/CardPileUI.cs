using System.Collections.Generic;
using UnityEngine;

public abstract class CardPileUI : MonoBehaviour
{
    [SerializeField] protected Transform cardsGrid;
    
    public void UpdateVisuals(List<CardSO> cardList)
    {
        foreach (Transform child in cardsGrid)
        {
            Destroy(child.gameObject);
        }
        
        if(cardList == null) return;

        foreach (var cardSO in cardList)
        {
            var card = Instantiate(DeckManager.Instance.basicCardPrefab, cardsGrid);
            var cardScript = card.GetComponent<BasicCard>();

            cardScript.cardData = cardSO;
            cardScript.cardState = BasicCard.CardState.Discarded;
            cardScript.Initialize();
        }
    }
}
