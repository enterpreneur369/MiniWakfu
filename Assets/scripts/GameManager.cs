using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // 1 = mover, 2 = combate
    public int estadoPartida = 1;
    public int turnosCombate = 0;
    public bool turnoPlayer = true;
    
    
    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int rollDieMonster()
    {
        return Random.Range(1,11);
    }
    
    public int rollDiePlayer()
    {
        return Random.Range(1,20);
    }
    
   

    public void UpdateGameStateAfterMove()
    {
        
    }

}
