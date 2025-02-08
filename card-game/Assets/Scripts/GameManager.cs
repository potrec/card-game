using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int playerTurn = 1;
    public int currentMana { get; private set; }
    public int maxMana { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    
    public void Start()
    {
        maxMana = 1;
        currentMana = maxMana;
    }
    
    public bool CanSpendMana(int manaCost)
    {
        if (currentMana >= manaCost)
        {
            currentMana -= manaCost;
            return true;
        }
        return false;
    }
    
    public void EndTurn()
    {
        playerTurn += 1;
        maxMana = maxMana < 10 ? maxMana + 1 : 10;
        currentMana = maxMana;
    }
}
