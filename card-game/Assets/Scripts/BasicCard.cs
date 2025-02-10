using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class BasicCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum CardState
    {
        InHand,
        Dragging,
        OnTable
    }   
    public CardSO cardData;
    private Vector3 initialPosition;
    private CanvasGroup canvasGroup;
    private Vector3 initialScale;
    private RectTransform rectTransform;
    public CardState cardState = CardState.InHand;
    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        initialScale = rectTransform.localScale;
        UpdateInitialTransform();
    }
    
    IEnumerator CoWaitForEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        initialPosition = rectTransform.localPosition;
    }

    public void UpdateInitialTransform()
    {
        StartCoroutine(CoWaitForEndOfFrame());
    }

    public void Initialize() {
        GetComponent<Image>().sprite = cardData.cardImage;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        cardState = CardState.Dragging;
        canvasGroup.blocksRaycasts = false;
    }

    //todo: jak trzymasz kartę i najedziesz na inną kartę to w tej karcie na którą najechałeś włącza się animacja w OnPointerEnter i OnPointerExit
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 1f;
        transform.position = mousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
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
        enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 scale = initialScale;
        Vector3 move = new Vector3(0, 100, 0);
        transform.DOScale(scale * 1.1f, 0.1f);
        transform.DOLocalMove(initialPosition + move, 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(initialScale, 0.1f);
        if(cardState == CardState.Dragging) return;
        transform.DOLocalMove(initialPosition, 0.25f);
    }
}
