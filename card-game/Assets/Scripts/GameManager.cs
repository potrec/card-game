using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int playerTurn = 1;
    public int currentMana { get; private set; }
    public int maxMana { get; private set; }
    public bool isActionsEnabled = true;
    
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI turnText;
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void Start()
    {
        InitializeMana();
        turnText.text = $"{playerTurn}";
        SetManaText();
    }
    
    public bool CanSpendMana(int manaCost)
    {
        if (currentMana >= manaCost)
        {
            return true;
        }
        return false;
    }
    
    private void InitializeMana()
    {
        maxMana = 1;
        currentMana = maxMana;
    }
    
    public void SpendMana(int manaCost)
    {
        currentMana -= manaCost;
        SetManaText();
    }
    
    public void EndTurn()
    {
        playerTurn += 1;
        turnText.text = $"{playerTurn}";
        maxMana = maxMana < 10 ? maxMana + 1 : 10;
        currentMana = maxMana;
        DeckManager.Instance.EndTurn();
        manaText.text = currentMana + "/" + maxMana;
        SetManaText();
    }
    
    public void SetManaText()
    {
        manaText.text = currentMana + "/" + maxMana;
    }
}
