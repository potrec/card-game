using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasicCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CardSO cardData;
    private Image cardImage;
    private Transform parentAfterDrag;
    
    public void Initialize() {
        GetComponent<Image>().sprite = cardData.cardImage;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        transform.rotation = Quaternion.identity;
        DeckManager.Instance.handCards.Remove(cardData);
        DeckManager.Instance.currentCard = cardData;
        Debug.Log($"Dragged {cardData.cardName}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DeckManager.Instance.tableCards.Add(cardData);
        transform.SetParent(DeckManager.Instance.handUI);
        DeckManager.Instance.currentCard = null;
        DeckManager.Instance.UpdateVisuals();
    }
}
