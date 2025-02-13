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
        Vector3 scale = initialScale;
        Vector3 move = new Vector3(0, HoverMoveUpDistance, 0);
        transform.DOScale(scale * HoverScaleFactor, HoverAnimationDuration);
        transform.parent.DOLocalMove(card.initialPosition + move, HoverAnimationDuration);
    }

    public void ResetHoverEffect()
    {
        transform.DOScale(initialScale, ExitHoverAnimationDuration);
        transform.parent.DOLocalMove(card.initialPosition, ExitHoverAnimationDuration);
    }

    public void CloneVisual()
    {
        clone = new GameObject("CardClone");
        clone.transform.position = transform.position;
        
        RectTransform cloneRectTransform = clone.AddComponent<RectTransform>();
        cloneRectTransform.sizeDelta = card.GetComponent<RectTransform>().sizeDelta;
        cloneRectTransform.localScale = transform.localScale;
        
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
}