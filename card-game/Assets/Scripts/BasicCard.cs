using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasicCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum CardState
    {
        InHand,
        Dragging,
        OnTable,
        Discarded,
        Disabled
    }

    public static bool IsDraggingCard { get; private set; }

    public CardSO cardData;
    public CardState cardState = CardState.Disabled;
    public BasicCardVisual cardVisual;
    [HideInInspector] public Vector3 initialPosition;

    private CanvasGroup canvasGroup;
    private Vector3 initialScale;
    private RectTransform rectTransform;
    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        UpdateInitialTransform();
    }

    IEnumerator CoWaitForEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        initialPosition = GetComponent<RectTransform>().localPosition;
    }

    public void UpdateInitialTransform()
    {
        StartCoroutine(CoWaitForEndOfFrame());
    }

    public void Initialize()
    {
        cardVisual.InitializeVisual(cardData);
    }

    private bool IsCardInteractive()
    {
        return cardState == CardState.InHand;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsCardInteractive()) return;
        cardState = CardState.Dragging;
        canvasGroup.blocksRaycasts = false;
        IsDraggingCard = true;

        cardVisual.CloneVisual();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsCardInteractive() && cardState != CardState.Dragging) return;
        transform.position = Input.mousePosition;
        if(cardVisual.clone != null) cardVisual.clone.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsCardInteractive() && cardState != CardState.Dragging) return;
        canvasGroup.blocksRaycasts = true;
        IsDraggingCard = false;

        cardVisual.DeleteCloneVisual();

        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<DropZone>() != null)
        {
            if (GameManager.Instance.CanSpendMana(cardData.manaCost))
            {
                PlayCard();
            }
            else
            {
                cardState = CardState.InHand;
            }
        }
        else
        {
            cardState = CardState.InHand;
        }
    }

    public void PlayCard()
    {
        GameManager.Instance.SpendMana(cardData.manaCost);
        cardVisual.PlayCardAnimation();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsCardInteractive() || IsDraggingCard) return;
        cardVisual.SetHoverEffect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsCardInteractive() || IsDraggingCard) return;
        cardVisual.ResetHoverEffect();
    }
}