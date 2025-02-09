using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class BasicCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CardSO cardData;
    private Vector3 initialPosition;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Initialize() {
        GetComponent<Image>().sprite = cardData.cardImage;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        initialPosition = transform.localPosition;
    }

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
            PlayCard();
            DeckManager.Instance.UpdateHandVisuals();
        }
        else
        {
            ReturnToHand();
        }
    }

    public void PlayCard()
    {
        if (GameManager.Instance.CanSpendMana(cardData.manaCost))
        {
            GameManager.Instance.SpendMana(cardData.manaCost);
            DeckManager.Instance.PlayCard(this);
        }
        else
        {
            ReturnToHand();
        }
    }

    public void ReturnToHand()
    {
        transform.DOLocalMove(initialPosition, 0.5f);
    }
}
