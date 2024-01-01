using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Spell
{
    public string name;
    public int damage;
    public int difficultyToHit;
}
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
    
    public string zone = "Tainela"; // Tainela en este caso en esta primer version solo tendrá esta zona

}

public class Monster : MonoBehaviour
{
    public MonsterZone[] monsterZones;
    public MonsterZone currentMonsterZone;

    public Image monsterImageUI; // Referencia al componente Image en la UI
    public Player player;
    private UIManager uiManager;
    
    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        SelectRandomMonsterZone();
        monsterImageUI.sprite = currentMonsterZone.monsterImage;
    }

    void SelectRandomMonsterZone()
    {
        int index = Random.Range(0, monsterZones.Length);
        currentMonsterZone = monsterZones[index];
        currentMonsterZone.actualHealth = currentMonsterZone.maxHealth;
    }

    public void TryAttack()
    {
        int resultadoDado = GameManager.Instance.rollDieMonster();
        Spell hechizoSeleccionado = SelectRandomSpell();

        if (resultadoDado >= hechizoSeleccionado.difficultyToHit 
            && GameManager.Instance.turnoPlayer == false 
            && GameManager.Instance.estadoPartida == 2)
        {
            player.vida -= hechizoSeleccionado.damage;
            uiManager.ShowMessage(currentMonsterZone.name + " te ataca con " + 
                                  hechizoSeleccionado.name + " pierdes " +
                                  hechizoSeleccionado.damage + " de vida");
            uiManager.txtVida.text = "Vida  " + player.vida;
            StartCoroutine(uiManager.WaitAndContinueMonster(5f));;
            GameManager.Instance.turnosCombate += 1;
        }
        else
        {
            uiManager.ShowMessage(currentMonsterZone.name + " ha fallado su ataque.");
            GameManager.Instance.turnosCombate += 1;
            uiManager.WaitAndContinueMonster(5f);
        }
    }

    Spell SelectRandomSpell()
    {
        int index = Random.Range(0, currentMonsterZone.spells.Length);
        return currentMonsterZone.spells[index];
    }

    // Update is called once per frame
    void Update()
    {
        // Aquí iría cualquier lógica que necesites ejecutar cada frame.
    }
}
