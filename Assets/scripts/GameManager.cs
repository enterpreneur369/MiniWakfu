using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // 1 = mover, 2 = combate
    public int estadoPartida = 1;
    public int turnosCombate = 0;
    public int posicionMapa = 1;
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
    
    public bool ShouldShowUpButton()
    {
        // Lógica para determinar si se debe mostrar el botón hacia arriba
        return posicionMapa == 1 || posicionMapa == 3 || posicionMapa == 5;
    }

    public bool ShouldShowRightButton()
    {
        // Lógica para determinar si se debe mostrar el botón hacia la derecha
        return posicionMapa == 1 || posicionMapa == 4;
    }

    public bool ShouldShowLeftButton()
    {
        // Lógica para determinar si se debe mostrar el botón hacia la izquierda
        return posicionMapa == 2 || posicionMapa == 3;
    }
    
    public bool IsMonsterEncounterPosition()
    {
        // Devuelve verdadero si el jugador está en las posiciones 2 o 4
        return posicionMapa == 2 || posicionMapa == 4;
    }

    public void UpdateGameStateAfterMove()
    {
        
    }

}
