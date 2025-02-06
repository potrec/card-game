using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private CardSO[] deckCards;
    [SerializeField] private CardSO[] handCards;
    [SerializeField] private CardSO[] discardPile;
    [SerializeField] private Transform basicCardPrefab;
    [SerializeField] private Transform deckUI;

    public int cardRotationInDeck = 5;
    
    private void Start()
    {
        float cardRotation = cardRotationInDeck * deckCards.Length/2;
        for (int i = 0; i < deckCards.Length; i++)
        {
            var card = Instantiate(basicCardPrefab, deckUI);
            var cardScript = card.GetComponent<BasicCard>();
            
            cardScript.cardData = deckCards[i];
            cardScript.Initialize();
            card.localRotation = Quaternion.Euler(0, 0, cardRotation-i*cardRotationInDeck);
        }
    }
    
}
