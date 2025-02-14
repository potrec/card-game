using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BasicCardVisual : MonoBehaviour
{
    private RectTransform rectTransform;
    public Vector3 initialScale;
    private BasicCard card;
    [HideInInspector] public GameObject clone;

    private const float HoverScaleFactor = 1.1f;
    private const float HoverAnimationDuration = 0.1f;
    private const float HoverMoveUpDistance = 100f;
    private const float ExitHoverAnimationDuration = 0.25f;
    private const float MoveToTableAnimationDuration = 0.5f;

    private Image cardImage;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialScale = rectTransform.localScale;
        card = GetComponentInParent<BasicCard>();
        cardImage = GetComponent<Image>();
    }

    public void InitializeVisual(CardSO cardData)
    {
        GetComponent<Image>().sprite = cardData.cardImage;
    }

    public void SetHoverEffect()
    {
        Vector3 move = new Vector3(0, HoverMoveUpDistance, 0);
        transform.parent.DOScale(initialScale * HoverScaleFactor, HoverAnimationDuration);
        transform.parent.DOLocalMove(card.initialPosition + move, HoverAnimationDuration);
    }

    public void ResetHoverEffect()
    {
        transform.parent.DOScale(initialScale, ExitHoverAnimationDuration);
        transform.parent.DOLocalMove(card.initialPosition, ExitHoverAnimationDuration);
    }

    public void CloneVisual()
    {
        clone = new GameObject("CardClone");
        clone.transform.position = transform.position;
        
        RectTransform cloneRectTransform = clone.AddComponent<RectTransform>();
        cloneRectTransform.sizeDelta = card.GetComponent<RectTransform>().sizeDelta;
        cloneRectTransform.localScale = transform.parent.localScale;
        
        Image cloneImage = clone.AddComponent<Image>();
        cloneImage.sprite = cardImage.sprite;
        cloneImage.raycastTarget = false;
        
        clone.transform.SetParent(DeckManager.Instance.deckCardsPoint);
        clone.transform.SetAsLastSibling();
    }

    public void DeleteCloneVisual()
    {
        if(clone != null) Destroy(clone);
        clone = null;
    }
    
    public void PlayCardAnimation()
    {
        card.cardState = BasicCard.CardState.OnTable;
        transform.parent.DOScale(initialScale, MoveToTableAnimationDuration);
        transform.parent.DOMove(DeckManager.Instance.tableUI.position, MoveToTableAnimationDuration).OnComplete(() =>
        {
            DeckManager.Instance.PlayCard(transform.parent);
            DeckManager.Instance.UpdateHandCards();
        });
    }
}