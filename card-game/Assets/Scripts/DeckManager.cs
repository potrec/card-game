using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public Transform deckCardsPoint;
    public Transform discardPilePoint;

    public List<DeckSO.DeckCards> deckCards;
    
    public float cardDrawTime = 0.5f;
    public float cardAnimationTimeDraw = 0.5f;

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
        StartCoroutine(DrawCards(startingHandDeckSize));
    }
    
    IEnumerator DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            DrawCard();
            yield return new WaitForSeconds(cardDrawTime);
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
        
        var card = InstantiateCard(cardData, deckCardsPoint);
        handCards.Add(card);
        StartCoroutine(CardDrawAnimationCoroutine(card));
    }
    
    private Transform InstantiateCard(CardSO cardData, Transform parent)
    {
        var card = Instantiate(basicCardPrefab, parent);
        card.gameObject.name = cardData.name;
        var cardScript = card.GetComponent<BasicCard>();
        
        cardScript.cardData = cardData;
        cardScript.Initialize();
        return card;
    }

    public void UpdateHandCards()
    {
        Debug.Log("Updating hand cards");
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
        
        StartCoroutine(DrawCards(startingHandDeckSize-handCards.Count));
        UpdateHandCards();
    }

    IEnumerator CardDrawAnimationCoroutine(Transform card)
    {
        BasicCard basicCard = card.GetComponent<BasicCard>();
        basicCard.cardState = BasicCard.CardState.Drawn;
        
        Vector3 targetScale = Vector3.one;
        
        card.localScale = Vector3.zero;
        Tween scaleTween = card.DOScale(targetScale, cardAnimationTimeDraw).SetEase(Ease.InSine);
        
        yield return scaleTween.WaitForCompletion();
        Tween moveTween = card.DOMove(handUI.position, cardAnimationTimeDraw).SetEase(Ease.InSine);
        yield return moveTween.WaitForCompletion();
        card.SetParent(handUI);
        basicCard.cardState = BasicCard.CardState.InHand;

        UpdateHandCards();
    }
}
