using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck", menuName = "ScriptableObjects/Deck")]
public class DeckSO : ScriptableObject
{
    public int deckID;
    public string deckName;
    public List<DeckCards> deckCards;
    
    [Serializable] public class DeckCards
    {
        public CardSO card;
        public int amount;
        
        public DeckCards(CardSO card, int amount)
        {
            this.card = card;
            this.amount = amount;
        }
    }
}


