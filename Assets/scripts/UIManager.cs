using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button btnArriba;
    public Button btnDerecha;
    public Button btnIzquierda;
    public GameObject player;
    public TextMeshProUGUI txtNivel;
    public TextMeshProUGUI txtKamas;
    public TextMeshProUGUI txtVida;
    public Image monsterImageUI;
    public Button btnAtaque1;
    public Button btnAtaque2;
    public Button btnAtaque3;
    public int posicionMapa = 1;

    private Monster currentMonster;
    public TextMeshProUGUI txtEstado;
    // Otros botones según necesites
    
    public void MovePlayer(string direction)
    {
        Vector3 newPosition = player.transform.position;

        switch (direction)
        {
            case "Arriba":
                if (posicionMapa == 1)
                {
                    posicionMapa = 3;
                    newPosition.y += 2;
                } 
                else if (posicionMapa == 3)
                {
                    posicionMapa = 5;
                    newPosition.y += 2;
                }
                else if (posicionMapa == 5)
                {
                    posicionMapa = 6;
                    newPosition.y += 2;
                }
                break;
            case "Derecha":
                if (posicionMapa == 1)
                {
                    posicionMapa = 2; // Zona de monstruo
                    newPosition.y += 1;
                    newPosition.x += 3;
                    GameManager.Instance.turnoPlayer = false;
                    GameManager.Instance.estadoPartida = 2;
                }
                else if (posicionMapa == 4)
                {
                    posicionMapa = 5;
                    newPosition.y += 1;
                    newPosition.x += 3;
                }
                break;
            case "Izquierda":
                if (posicionMapa == 2)
                {
                    posicionMapa = 3;
                    newPosition.x -= 3;
                    newPosition.y += 1;
                }
                else if (posicionMapa == 3)
                {
                    posicionMapa = 4; // Zona de monstruo
                    newPosition.x -= 3;
                    newPosition.y += 1;
                }
                break;
        }

        player.transform.position = newPosition;
        GameManager.Instance.UpdateGameStateAfterMove();
    }

    public void ShowMessage(string message)
    {
        txtEstado.text = message;
    }
    public IEnumerator WaitAndAttack(float delay, Monster monster)
    {
        yield return new WaitForSeconds(delay);
        monster.TryAttack();
    }
    
    private void Update()
    {
        UpdateUI();
        Debug.Log(GameManager.Instance.estadoPartida == 1 ? "EXPLORACION" : "COMBATE");
        Debug.Log(GameManager.Instance.turnoPlayer ? "TURNO PLAYER" : "TURNO MONSTER");
        if (IsMonsterEncounterPosition() 
            && GameManager.Instance.estadoPartida == 2)
        {
            // Aquí mostrarías el monstruo
            ShowMonster();
        }
        else
        {
            // Aquí ocultarías el monstruo si no es el lugar o el turno adecuado
            HideMonster();
        }
        if (GameManager.Instance.estadoPartida == 2 
            && GameManager.Instance.turnoPlayer == false)
        {
            HandleMonsterEncounter();
        }
        
    }
    
    public void ShowMonster()
    {
        // Asegúrate de que tienes una referencia a la imagen del monstruo en la UI
        if (monsterImageUI != null)
        {
            monsterImageUI.gameObject.SetActive(true);
        }
    }
    
    public void HideMonster()
    {
        // Oculta la imagen del monstruo en la UI
        if (monsterImageUI != null)
        {
            monsterImageUI.gameObject.SetActive(false);
        }
    }


    private void HandleMonsterEncounter()
    {
        GameObject monsterGameObject = GameObject.Find("Monster");
        if (monsterGameObject != null)
        {
            currentMonster = monsterGameObject.GetComponent<Monster>();
            if (currentMonster != null)
            {
                monsterGameObject.SetActive(true);
                if (GameManager.Instance.turnosCombate == 0)
                {
                    ShowMessage("Un " + currentMonster.currentMonsterZone.name + 
                                " ha aparecido y no te quieren dejar acercar.");
                }
                StartCoroutine(WaitAndAttack(5, currentMonster));
            }
            else
            {
                Debug.LogError("No se encontró el componente Monster en el objeto.");
            }
        }
        else
        {
            Debug.LogError("No se encontró el objeto Monster en la escena.");
        }
    }
    
    public IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    private void UpdateUI()
    {
        if (GameManager.Instance.estadoPartida == 2) // COMBATE
        {
            btnArriba.gameObject.SetActive(false);
            btnDerecha.gameObject.SetActive(false);
            btnIzquierda.gameObject.SetActive(false);

            if (GameManager.Instance.turnoPlayer && GameManager.Instance.turnosCombate > 0 
                                                 && currentMonster.hasAttacked == true)
            {
                btnAtaque1.gameObject.SetActive(true);
                btnAtaque2.gameObject.SetActive(true);
                btnAtaque3.gameObject.SetActive(true);
            }
            else
            {
                btnAtaque1.gameObject.SetActive(false);
                btnAtaque2.gameObject.SetActive(false);
                btnAtaque3.gameObject.SetActive(false);
            }
        }
        else // EXPLORACION
        {
            btnArriba.gameObject.SetActive(this.ShouldShowUpButton());
            btnDerecha.gameObject.SetActive(this.ShouldShowRightButton());
            btnIzquierda.gameObject.SetActive(this.ShouldShowLeftButton());
            
            btnAtaque1.gameObject.SetActive(false);
            btnAtaque2.gameObject.SetActive(false);
            btnAtaque3.gameObject.SetActive(false);
        }
        
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
        return posicionMapa == 2 || posicionMapa == 4 || posicionMapa == 6;
    }
}