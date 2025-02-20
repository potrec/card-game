using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "ScriptableObjects/Card")]
public class CardSO : ScriptableObject
{
    public int cardID;
    public string cardName;
    public string description;
    public Sprite cardImage;
    public int manaCost;
}
