using System;
using System.Collections;
using UnityEngine;

// Se define una clase serializable para representar los hechizos del jugador, con nombre, daño y dificultad para acertar.
[System.Serializable]
public class PlayerSpell
{
    public string name;
    public int damage;
    public int difficultyToHit;
}

// Clase principal del jugador que hereda de MonoBehaviour para poder interactuar con Unity.
public class Player : MonoBehaviour
{
    // Variables para la vida, kamas (moneda del juego) y nivel del jugador, inicializadas.
    public int vida = 10;
    public int kamas = 0;
    public int nivel = 1;
    // Array de ataques disponibles para el jugador.
    public PlayerSpell[] ataques;
    // Referencia privada al UIManager para interactuar con la UI.
    private UIManager _uiManager;

    // Start se llama antes de la primera actualización del frame, inicializa el UIManager.
    void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    // Método para realizar un ataque, recibe el índice del ataque a utilizar.
    public void Ataque(int index)
    {
        // Verifica si el índice es válido, si no, muestra un error y sale del método.
        if (index < 0 || index >= ataques.Length)
        {
            Debug.LogError("Índice de ataque no válido.");
            return;
        }

        // Lanza un dado para determinar el resultado del ataque y selecciona el hechizo basado en el índice.
        int resultadoDado = GameManager.Instance.rollDiePlayer();
        PlayerSpell hechizoSeleccionado = ataques[index];
        // Encuentra al monstruo en la escena para interactuar con él.
        Monster monster = GameObject.Find("Monster").GetComponent<Monster>();
        // Calcula si el ataque es crítico basado en el resultado del dado.
        int danioCritico = resultadoDado == 20 ? 1 : 0;
        // Determina si el ataque acierta basado en la dificultad del hechizo y el resultado del dado.
        bool isHit = resultadoDado >= hechizoSeleccionado.difficultyToHit;

        // Si el ataque acierta, es el turno del jugador, el estado de la partida es de combate y quedan turnos de combate...
        if (isHit && GameManager.Instance.turnoPlayer && 
            GameManager.Instance.estadoPartida == 2 && // COMBATE
            GameManager.Instance.turnosCombate > 0)
        {
            // Se reduce la salud del monstruo y se verifica si ha sido derrotado.
            monster.currentMonsterZone.actualHealth -= (hechizoSeleccionado.damage + danioCritico);
            if (monster.currentMonsterZone.actualHealth <= 0)
            {
                // Si el monstruo es derrotado, se cambia el estado de la partida, se desactiva el monstruo y se muestra un mensaje.
                GameManager.Instance.estadoPartida = 1;
                monster.gameObject.SetActive(false);
                _uiManager.ShowMessage($"Has derrotado al {monster.currentMonsterZone.name}\n has ganado 1 KAMA");
                this.kamas += 1;
                _uiManager.ShowMessage("" + this.kamas);
                StartCoroutine(WaitAndChangeState(5, monster));
            }
            else
            {
                // Si el monstruo no es derrotado, se muestra un mensaje con el daño realizado.
                string message = danioCritico != 0 ?
                    $"(CRÍTICO) Atacas al {monster.name} con {hechizoSeleccionado.name} " +
                    $"y pierde {hechizoSeleccionado.damage + danioCritico} de vida" :
                    $"Atacas al {monster.currentMonsterZone.name} con {hechizoSeleccionado.name} " +
                    $"y pierde {hechizoSeleccionado.damage} de vida";

                _uiManager.ShowMessage(message);
                StartCoroutine(WaitAndChangeTurn(5, false));
            }
        }
        else
        {
            // Si el ataque falla, se muestra un mensaje y se cambia el turno.
            _uiManager.ShowMessage("Haz fallado tu ataque.");
            StartCoroutine(WaitAndChangeTurn(5, false));
        }
        // Espera antes de continuar con el siguiente turno.
        StartCoroutine(_uiManager.Wait(5));
        monster.hasAttacked = false;
        GameManager.Instance.turnoPlayer = false;
        GameManager.Instance.turnosCombate += 1;
    }
    
    // Corutinas para manejar el cambio de estado y turno después de un delay.
    private IEnumerator WaitAndChangeState(float delay, Monster monster)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.turnosCombate = 0;
        monster.hasAttacked = true;
        GameManager.Instance.turnoPlayer = true;
    }

    private IEnumerator WaitAndChangeTurn(float delay, bool isPlayerTurn)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.turnoPlayer = isPlayerTurn;
    }
    

    // Métodos de ataque individuales que llaman al método general Ataque con diferentes índices.
    public void Ataque1() { Ataque(0); }
    public void Ataque2() { Ataque(1); }
    public void Ataque3() { Ataque(2); }

    // Update se llama una vez por frame, muestra la vida del jugador en consola.
    private void Update()
    {
        Debug.Log("VIDA JUGADOR: " + vida);
    }
}
