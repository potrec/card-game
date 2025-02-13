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
        Drawn,
        Disabled
    }
    
    public static bool IsDraggingCard { get; private set; }
    
    public CardSO cardData;
    public CardState cardState = CardState.Drawn;
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

    public void Initialize() {
        cardVisual.InitializeVisual(cardData);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (cardState == CardState.OnTable || cardState == CardState.Discarded || cardState == CardState.Disabled) return;
        cardState = CardState.Dragging;
        canvasGroup.blocksRaycasts = false;
        IsDraggingCard = true;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (cardState == CardState.OnTable || cardState == CardState.Discarded || cardState == CardState.Drawn || cardState == CardState.Disabled) return;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 1f;
        transform.position = mousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (cardState == CardState.OnTable || cardState == CardState.Discarded || cardState == CardState.Drawn || cardState == CardState.Disabled) return;
        canvasGroup.blocksRaycasts = true;
        IsDraggingCard = false;
        if(eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<DropZone>() != null)
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
        
        DeckManager.Instance.UpdateHandCards();
        DeckManager.Instance.PlayCard(transform);
        
        cardState = CardState.OnTable;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cardState == CardState.OnTable || IsDraggingCard || cardState == CardState.Discarded || cardState == CardState.Drawn || cardState == CardState.Disabled) return;
        cardVisual.SetHoverEffect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cardState == CardState.OnTable || IsDraggingCard || cardState == CardState.Discarded || cardState == CardState.Drawn || cardState == CardState.Disabled) return;
        cardVisual.ResetHoverEffect();
    }
}
