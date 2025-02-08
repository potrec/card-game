using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasicCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CardSO cardData;
    private Transform parentAfterDrag;
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
        parentAfterDrag = transform.parent;
        initialPosition = transform.localPosition;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        if (transform.parent == parentAfterDrag.root)
        {
            Debug.Log("Dropped outside of parent");
            ReturnToHand();
        }
        Debug.Log("Dropped on parent");
        DeckManager.Instance.UpdateHandVisuals();
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
        transform.SetParent(parentAfterDrag);
        transform.localPosition = initialPosition;
    }
}
