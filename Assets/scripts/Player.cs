using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlayerSpell
{
    public string name;
    public int damage;
    public int difficultyToHit;
}

public class Player : MonoBehaviour
{
    public int vida = 10;
    public int kamas = 0;
    public int nivel = 1;
    public PlayerSpell[] ataques;
    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    public void Ataque(int index)
    {
        if (index < 0 || index >= ataques.Length)
        {
            Debug.LogError("Índice de ataque no válido.");
            return;
        }

        int resultadoDado = GameManager.Instance.rollDiePlayer();
        PlayerSpell hechizoSeleccionado = ataques[index];
        Monster monster = GameObject.Find("Monster").GetComponent<Monster>();
        int danioCritico = resultadoDado == 20 ? 1 : 0;
        bool isHit = resultadoDado >= hechizoSeleccionado.difficultyToHit;

        if (isHit && GameManager.Instance.turnoPlayer && 
            GameManager.Instance.estadoPartida == 2 && 
            GameManager.Instance.turnosCombate > 0)
        {
            monster.currentMonsterZone.actualHealth -= (hechizoSeleccionado.damage + danioCritico);
            if (monster.currentMonsterZone.actualHealth <= 0)
            {
                _uiManager.ShowMessage($"Has derrotado al {monster.name}");
                GameManager.Instance.turnosCombate = 0;
                GameManager.Instance.turnoPlayer = true;
                GameManager.Instance.estadoPartida = 2;
            }
            string message = danioCritico != 0 ?
                $"(CRÍTICO) Atacas al {monster.name} con {hechizoSeleccionado.name} " +
                $"y pierde {hechizoSeleccionado.damage + danioCritico} de vida" :
                $"Atacas al {monster.name} con {hechizoSeleccionado.name} y pierde {hechizoSeleccionado.damage} de vida";

            _uiManager.ShowMessage(message);
        }
        else
        {
            _uiManager.ShowMessage("Haz fallado tu ataque.");
        }

        StartCoroutine(_uiManager.WaitAndContinuePlayer(5f));
        GameManager.Instance.turnosCombate += 1;
    }

    // Métodos de ataque individuales que llaman al método general Ataque
    public void Ataque1() { Ataque(0); }
    public void Ataque2() { Ataque(1); }
    public void Ataque3() { Ataque(2); }
}
