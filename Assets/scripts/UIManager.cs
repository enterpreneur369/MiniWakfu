using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button btnArriba;
    public Button btnDerecha;
    public Button btnIzquierda;
    public GameObject player;
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
                    newPosition.y += 2; // Cambia 1 por la distancia que quieres que se mueva
                } 
                else if (GameManager.Instance.posicionMapa == 3)
                {
                    GameManager.Instance.posicionMapa = 5;
                    newPosition.y += 2; // Cambia 1 por la distancia que quieres que se mueva
                }
                else if (GameManager.Instance.posicionMapa == 5)
                {
                    GameManager.Instance.posicionMapa = 6;
                    newPosition.y += 2; // Cambia 1 por la distancia que quieres que se mueva
                }
                break;
            case "Derecha":
                if (GameManager.Instance.posicionMapa == 1)
                {
                    GameManager.Instance.posicionMapa = 2;
                    newPosition.y += 1;
                    newPosition.x += 3;
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
                    GameManager.Instance.posicionMapa = 4;
                    newPosition.x -= 3;
                    newPosition.y += 1;
                }
                break;
            // Agrega más casos según sea necesario
        }

        player.transform.position = newPosition;
        GameManager.Instance.UpdateGameStateAfterMove();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        btnArriba.gameObject.SetActive(GameManager.Instance.ShouldShowUpButton());
        btnDerecha.gameObject.SetActive(GameManager.Instance.ShouldShowRightButton());
        btnIzquierda.gameObject.SetActive(GameManager.Instance.ShouldShowLeftButton());
    }
}