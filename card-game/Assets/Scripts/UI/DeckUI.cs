using System;
using UnityEngine;
using UnityEngine.Serialization;

public class DeckUI : MonoBehaviour
{
    [SerializeField] private Transform cardsGrid;

    public void UpdateVisuals()
    {
        // TODO: Create a visual representation of the card and the amount of that card in the deck
        foreach (Transform child in cardsGrid)
        {
            Destroy(child.gameObject);
        }

        // for (int i = 0; i < DeckManager.Instance.deckCards.Count; i++)
        // {
        //     var card = Instantiate(DeckManager.Instance.basicCardPrefab, cardsGrid);
        //     var cardScript = card.GetComponent<BasicCard>();
        //
        //     cardScript.cardData = DeckManager.Instance.deckCards[i];
        //     cardScript.Initialize();
        // }
    }

    public void Start()
    {
        UpdateVisuals();
    }
}
