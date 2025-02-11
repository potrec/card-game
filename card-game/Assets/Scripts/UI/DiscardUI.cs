using UnityEngine;

public class DiscardUI : CardPileUI
{
    public void OnEnable()
    {
        UpdateVisuals();
    }
    
    public void UpdateVisuals()
    {
        base.UpdateVisuals(DeckManager.Instance.discardPile);
    }
}
