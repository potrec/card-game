using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardAmountText;
    [SerializeField] private Image cardImage;
    
    public void Initialize(int amount, CardSO cardData)
    {
        cardImage.sprite = cardData.cardImage;
        cardAmountText.text = $"Card amount: {amount}";
    }
}
