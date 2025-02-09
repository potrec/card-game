using UnityEngine;

public class DiscardUI : MonoBehaviour
{
    [SerializeField] private Transform cardsGrid;

    public void UpdateVisuals()
    {
        foreach (Transform child in cardsGrid)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < DeckManager.Instance.discardPile.Count; i++)
        {
            var card = Instantiate(DeckManager.Instance.basicCardPrefab, cardsGrid);
            var cardScript = card.GetComponent<BasicCard>();

            cardScript.cardData = DeckManager.Instance.discardPile[i];
            cardScript.Initialize();
        }
    }

    public void OnEnable()
    {
        UpdateVisuals();
    }
}
