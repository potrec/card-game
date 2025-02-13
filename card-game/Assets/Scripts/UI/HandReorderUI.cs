using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandReorderUI : MonoBehaviour, IDropHandler
{
    public GridLayoutGroup gridLayoutGroup;

    public void OnDrop(PointerEventData eventData)
    {
        if(!GameManager.Instance.isActionsEnabled) return;
        UpdateHandCardOrderInDeckManager(eventData);
    }
    
    public List<CardPosition> GetCardPositionsAroundDrop(Vector3 position)
    {
        List<CardPosition> cardPositions = new List<CardPosition>();
        
        foreach (Transform card in gridLayoutGroup.transform)
        {
            BasicCard basicCard = card.GetComponent<BasicCard>();
            
            float distance = Vector3.Distance(basicCard.initialPosition, position);
            bool direction = basicCard.initialPosition.x > position.x;
            
            CardPosition cardPosition = new CardPosition(basicCard.initialPosition, card, direction, distance);
            cardPositions.Add(cardPosition);
        }
        
        return cardPositions;
    }
    
    private void UpdateHandCardOrderInDeckManager(PointerEventData eventData)
    {
        Vector3 positionInThisTransform = transform.InverseTransformPoint(eventData.pointerDrag.transform.position);
        List<CardPosition> cardPositions = GetCardPositionsAroundDrop(positionInThisTransform);

        if (cardPositions.Count <= 1) return;

        bool isAllCardsOnTheLeft = cardPositions.All(c => c.isOnLeftOfDropPosition);
        bool isAllCardsOnTheRight = cardPositions.All(c => !c.isOnLeftOfDropPosition);

        Transform cardInHandList = DeckManager.Instance.handCards.Find(c => c == eventData.pointerDrag.transform);
        if (!cardInHandList)
        {
            Debug.LogError("Card not found in hand list");
            return;
        }

        int index = DeckManager.Instance.handCards.IndexOf(cardInHandList);
        CardPosition closestCard = cardPositions.OrderBy(c => c.distanceFromDropPosition).First();
        if (closestCard.card == cardInHandList)
        {
            return;
        }
        DeckManager.Instance.handCards.RemoveAt(index);
        if (isAllCardsOnTheLeft)
        {
            DeckManager.Instance.handCards.Insert(0, cardInHandList);
        }
        else if (isAllCardsOnTheRight)
        {
            DeckManager.Instance.handCards.Add(cardInHandList);
        }
        else
        {
            int closestCardIndex = DeckManager.Instance.handCards.IndexOf(closestCard.card);
            int insertIndex = closestCard.isOnLeftOfDropPosition ? closestCardIndex : closestCardIndex + 1;
            DeckManager.Instance.handCards.Insert(insertIndex, cardInHandList);
        }
        gridLayoutGroup.transform.DetachChildren();
        foreach (var card in DeckManager.Instance.handCards)
        {
            card.SetParent(gridLayoutGroup.transform);
        }

        DeckManager.Instance.UpdateHandCards();
    }
}

public class CardPosition
{
    public Vector3 position;
    public Transform card;
    public bool isOnLeftOfDropPosition;
    public float distanceFromDropPosition;
        
    public CardPosition(Vector3 position, Transform card, bool isOnLeftOfDropPosition, float distanceFromDropPosition)
    {
        this.position = position;
        this.card = card;
        this.isOnLeftOfDropPosition = isOnLeftOfDropPosition;
        this.distanceFromDropPosition = distanceFromDropPosition;
    }
        
    public override string ToString()
    {
        return $"Card {card.GetComponent<BasicCard>().cardData.name} is at position {position} and is on the {(isOnLeftOfDropPosition ? "left" : "right")} side of the drop position";
    }
}