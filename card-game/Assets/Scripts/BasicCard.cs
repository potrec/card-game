using UnityEngine;
using UnityEngine.UI;

public class BasicCard : MonoBehaviour
{
    public CardSO cardData;
    private Image cardImage;
    
    public void Initialize() {
        GetComponent<Image>().sprite = cardData.cardImage;
    }
    
    
}
