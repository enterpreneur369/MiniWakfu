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

    public TextMeshProUGUI txtEstado;
    // Otros botones según necesites
    
    public void MovePlayer(string direction)
    {
        Vector3 newPosition = player.transform.position;

        switch (direction)
        {
            case "Arriba":
                if (GameManager.Instance.posicionMapa == 1)
                {
                    GameManager.Instance.posicionMapa = 3;
                    newPosition.y += 2;
                } 
                else if (GameManager.Instance.posicionMapa == 3)
                {
                    GameManager.Instance.posicionMapa = 5;
                    newPosition.y += 2;
                }
                else if (GameManager.Instance.posicionMapa == 5)
                {
                    GameManager.Instance.posicionMapa = 6;
                    newPosition.y += 2;
                }
                break;
            case "Derecha":
                if (GameManager.Instance.posicionMapa == 1)
                {
                    GameManager.Instance.posicionMapa = 2; // Zona de monstruo
                    newPosition.y += 1;
                    newPosition.x += 3;
                    GameManager.Instance.turnoPlayer = false;
                    GameManager.Instance.estadoPartida = 2;
                }
                else if (GameManager.Instance.posicionMapa == 4)
                {
                    GameManager.Instance.posicionMapa = 5;
                    newPosition.y += 1;
                    newPosition.x += 3;
                }
                break;
            case "Izquierda":
                if (GameManager.Instance.posicionMapa == 2)
                {
                    GameManager.Instance.posicionMapa = 3;
                    newPosition.x -= 3;
                    newPosition.y += 1;
                }
                else if (GameManager.Instance.posicionMapa == 3)
                {
                    GameManager.Instance.posicionMapa = 4; // Zona de monstruo
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

    public IEnumerator WaitAndContinueMonster(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        GameManager.Instance.turnoPlayer = true;
    }
    
    public IEnumerator WaitAndContinuePlayer(float segundos)
    {
        GameManager.Instance.turnoPlayer = false;
        yield return new WaitForSeconds(segundos);
    }
    
    private void Update()
    {
        UpdateUI();
        Debug.Log(GameManager.Instance.estadoPartida == 1 ? "EXPLORACION" : "COMBATE");
        Debug.Log(GameManager.Instance.turnoPlayer ? "TURNO PLAYER" : "TURNO MONSTER");
        if (GameManager.Instance.IsMonsterEncounterPosition() 
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
            Monster currentMonster = monsterGameObject.GetComponent<Monster>();
            if (currentMonster != null)
            {
                monsterGameObject.SetActive(true);
                if (GameManager.Instance.turnosCombate == 0)
                {
                    ShowMessage("Un " + currentMonster.currentMonsterZone.name + 
                                " ha aparecido y no te quieren dejar acercar.");
                    StartCoroutine(WaitAndStartMonsterAttack(currentMonster));
                }
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
    
    private IEnumerator WaitAndStartMonsterAttack(Monster monster)
    {
        yield return new WaitForSeconds(5f);
        monster.TryAttack();
    }

    private void UpdateUI()
    {
        if (GameManager.Instance.estadoPartida == 2)
        {
            btnArriba.gameObject.SetActive(false);
            btnDerecha.gameObject.SetActive(false);
            btnIzquierda.gameObject.SetActive(false);

            if (GameManager.Instance.turnoPlayer && GameManager.Instance.turnosCombate > 0)
            {
                btnAtaque1.gameObject.SetActive(true);
                btnAtaque2.gameObject.SetActive(true);
                btnAtaque3.gameObject.SetActive(true);
            }
        }
        else
        {
            btnArriba.gameObject.SetActive(GameManager.Instance.ShouldShowUpButton());
            btnDerecha.gameObject.SetActive(GameManager.Instance.ShouldShowRightButton());
            btnIzquierda.gameObject.SetActive(GameManager.Instance.ShouldShowLeftButton());
            
            btnAtaque1.gameObject.SetActive(false);
            btnAtaque2.gameObject.SetActive(false);
            btnAtaque3.gameObject.SetActive(false);
        }
        
    }
}