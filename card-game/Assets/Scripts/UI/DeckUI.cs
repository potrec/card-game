using System;
using UnityEngine;
using UnityEngine.Serialization;

public class DeckUI : MonoBehaviour
{
    [SerializeField] private Transform cardsGrid;

    public void UpdateVisuals()
    {
        foreach (Transform child in cardsGrid)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < DeckManager.Instance.deckCards.Count; i++)
        {
            var card = Instantiate(DeckManager.Instance.basicCardPrefab, cardsGrid);
            var cardScript = card.GetComponent<BasicCard>();

            cardScript.cardData = DeckManager.Instance.deckCards[i];
            cardScript.Initialize();
        }
    }

    public void Start()
    {
        UpdateVisuals();
    }
}
