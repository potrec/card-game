using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BasicCardVisual : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 initialScale;
    private BasicCard card;
    
    private const float HoverScaleFactor = 1.1f;
    private const float HoverAnimationDuration = 0.1f;
    private const float HoverMoveUpDistance = 100f;
    private const float ExitHoverAnimationDuration = 0.25f;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialScale = rectTransform.localScale;
        card = GetComponentInParent<BasicCard>();
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
}
