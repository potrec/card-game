using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasicCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CardSO cardData;
    private Transform parentAfterDrag;
    private Vector3 initialPosition;
    
    public void Initialize() {
        GetComponent<Image>().sprite = cardData.cardImage;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
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
        if (transform.parent == parentAfterDrag.root)
        {
            ReturnToHand();
        }
        DeckManager.Instance.UpdateHandVisuals();
    }

    public void PlayCard()
    {
        if (GameManager.Instance.CanSpendMana(cardData.manaCost))
        {
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
