using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Definición de la clase Spell, que representa un hechizo con nombre, daño y dificultad para acertar.
[System.Serializable]
public class Spell
{
    public string name;
    public int damage;
    public int difficultyToHit;
}

// Definición de la clase MonsterZone, que representa una zona de monstruos con nombre, salud máxima, hechizos, imagen y si es un jefe o no.
[System.Serializable]
public class MonsterZone
{
    public string name;
    public int maxHealth;
    public Spell[] spells;
    public Sprite monsterImage;
    [HideInInspector]
    public int actualHealth;

    public bool isBoss;
    
    public string zone = "Tainela"; // Tainela es la única zona disponible en esta versión.

}

// Clase principal Monster que controla el comportamiento de los monstruos en el juego.
public class Monster : MonoBehaviour
{
    public MonsterZone[] monsterZones; // Array de zonas de monstruos disponibles.
    public MonsterZone currentMonsterZone; // Zona de monstruo actual en la que se encuentra el monstruo.
    public Slider estadoVidaMonstruo; // Referencia al componente Slider que muestra la vida del monstruo en la UI.

    public Image monsterImageUI; // Referencia al componente Image en la UI para mostrar la imagen del monstruo.
    public Player player; // Referencia al jugador.
    private UIManager uiManager; // Referencia al UIManager para controlar elementos de la UI.
    public bool hasAttacked = false; // Indica si el monstruo ya ha atacado.
    
    // Start se llama antes de la primera actualización del frame.
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>(); // Busca el UIManager en la escena.
        estadoVidaMonstruo = FindObjectOfType<Slider>(); // Busca el componente Slider en la escena.
        SelectRandomMonsterZone(); // Selecciona una zona de monstruo al azar al iniciar.
        monsterImageUI.sprite = currentMonsterZone.monsterImage; // Establece la imagen del monstruo en la UI.
    }

    // Selecciona una zona de monstruo al azar y establece la salud actual igual a la salud máxima.
    void SelectRandomMonsterZone()
    {
        int index = Random.Range(0, monsterZones.Length); // Selecciona un índice al azar.
        currentMonsterZone = monsterZones[index]; // Establece la zona de monstruo actual.
        currentMonsterZone.actualHealth = currentMonsterZone.maxHealth; // Establece la salud actual a la máxima.
        estadoVidaMonstruo.minValue = 0; // Establece el valor mínimo del Slider de vida.
        estadoVidaMonstruo.maxValue = currentMonsterZone.maxHealth; // Establece el valor máximo del Slider de vida.
        estadoVidaMonstruo.value = currentMonsterZone.maxHealth; // Establece el valor actual del Slider de vida.
    }

    // Intenta realizar un ataque.
    public void TryAttack()
    {
        if (hasAttacked) return; // Si ya ha atacado, no hace nada.
        
        int resultadoDado = GameManager.Instance.rollDieMonster(); // Lanza un dado para el monstruo.
        Spell hechizoSeleccionado = SelectRandomSpell(); // Selecciona un hechizo al azar.

        StartCoroutine(ManejarAtaque(resultadoDado, hechizoSeleccionado)); // Inicia la corutina para manejar el ataque.
    }

    // Corutina que maneja el ataque.
    IEnumerator ManejarAtaque(int resultadoDado, Spell hechizoSeleccionado)
    {
        yield return new WaitForSeconds(5); // Espera 5 segundos.
        
        // Si el resultado del dado es mayor o igual a la dificultad para acertar, no es el turno del jugador y el estado de la partida es 1 (combate),
        // entonces el jugador pierde vida y se muestra un mensaje.
        if (resultadoDado >= hechizoSeleccionado.difficultyToHit 
            && GameManager.Instance.turnoPlayer == false 
            && GameManager.Instance.estadoPartida == 1)
        {
            player.vida -= hechizoSeleccionado.damage; // El jugador pierde vida.
            uiManager.ShowMessage(currentMonsterZone.name + " te ataca con " + 
                                  hechizoSeleccionado.name + " pierdes " +
                                  hechizoSeleccionado.damage + " de vida"); // Muestra mensaje de ataque.
            uiManager.txtVida.text = "Vida  " + player.vida; // Actualiza la vida del jugador en la UI.
            StartCoroutine(uiManager.Wait(5)); // Espera 5 segundos.
            GameManager.Instance.turnosCombate += 1; // Incrementa el contador de turnos de combate.
            GameManager.Instance.turnoPlayer = true; // Cambia el turno al jugador.
        }
        else
        {
            uiManager.ShowMessage(currentMonsterZone.name + " ha fallado su ataque."); // Muestra mensaje de ataque fallido.
            StartCoroutine(uiManager.Wait(5)); // Espera 5 segundos.
            GameManager.Instance.turnosCombate += 1; // Incrementa el contador de turnos de combate.
            GameManager.Instance.turnoPlayer = true; // Cambia el turno al jugador.
        }
        hasAttacked = true; // Indica que el monstruo ya ha atacado.
    }

    // Selecciona un hechizo al azar de la zona de monstruo actual.
    Spell SelectRandomSpell()
    {
        int index = Random.Range(0, currentMonsterZone.spells.Length); // Selecciona un índice al azar.
        return currentMonsterZone.spells[index]; // Devuelve el hechizo seleccionado.
    }
    
    // Update se llama una vez por frame.
    void Update()
    {
        Debug.Log("VIDA MONSTRUO: " + currentMonsterZone.actualHealth); // Muestra la vida actual del monstruo.
        Debug.Log(hasAttacked ? "YA ATACO EL MONSTRUO": "NO HA ATACADO EL MONSTRUO"); // Indica si el monstruo ha atacado o no.
    }
}