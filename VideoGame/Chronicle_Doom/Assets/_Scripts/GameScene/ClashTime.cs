using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClashTime : MonoBehaviour
{
    public List<CardPropertiesDrag> timelineA = new List<CardPropertiesDrag>();
    public List<CardPropertiesDrag> timelineB = new List<CardPropertiesDrag>();
    public List<CardPropertiesDrag> timelineC = new List<CardPropertiesDrag>();
    public List<CardPropertiesDrag> enemylineA = new List<CardPropertiesDrag>();
    public List<CardPropertiesDrag> enemylineB = new List<CardPropertiesDrag>();
    public List<CardPropertiesDrag> enemylineC = new List<CardPropertiesDrag>();
    public List<CardPropertiesDrag> quantumTunnel = new List<CardPropertiesDrag>();
    [SerializeField] private Transform enemySpawner;
    [SerializeField] private Transform enemyAreaA;
    [SerializeField] private Transform enemyAreaB;
    [SerializeField] private Transform enemyAreaC;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Button endTurnButton;

    public void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        endTurnButton.onClick.AddListener(CheckClash);
    }

    public void UpdateLists(List<CardPropertiesDrag> previousCards,
                            List<CardPropertiesDrag> currentCards,
                            string listName)
    {
        List<CardPropertiesDrag> targetList = GetListByName(listName);

        if (targetList == null)
        {
            return;
        }
        foreach (CardPropertiesDrag card in previousCards)
        {
            if (!currentCards.Contains(card))
            {
                targetList.Remove(card);
            }
        }

        foreach (CardPropertiesDrag card in currentCards)
        {
            if (!targetList.Contains(card))
            {
                targetList.Add(card);
            }
        }
    }

    private List<CardPropertiesDrag> GetListByName(string listName)
    {
        switch (listName)
        {
            case "TimeLineA":
                return timelineA;
            case "TimeLineB":
                return timelineB;
            case "TimeLineC":
                return timelineC;
            case "QuantumTunnel":
                return quantumTunnel;
            case "EnemyLineA":
                return enemylineA;
            case "EnemyLineB":
                return enemylineB;
            case "EnemyLineC":
                return enemylineC;
            default:
                return null;
        }
    }

    public void RelocateEnemies()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in enemySpawner)
        {
            if (child != enemySpawner) 
            {
                children.Add(child);
            }
        }

        Debug.Log($"Relocating {children.Count} enemies");
        foreach (Transform child in children)
        {
            if (child.CompareTag("Enemy"))
            {
                CardPropertiesDrag card = child.GetComponent<CardPropertiesDrag>();
                if (card != null)
                {
                    int randomLine = Random.Range(0, 3);
                    switch (randomLine)
                    {
                        case 0:
                            card.transform.SetParent(enemyAreaA);
                            break;
                        case 1:
                            card.transform.SetParent(enemyAreaB);
                            break;
                        case 2:
                            card.transform.SetParent(enemyAreaC);
                            break;
                    }
                }
            }
        }
    }

    public void CheckClash()
    {
        endTurnButton.interactable = false;
        if (enemylineA.Count > 0)
        {
            Debug.Log("Clashing Line A");
            Clash(timelineA, enemylineA);
        }
        if (enemylineB.Count > 0)
        {
            Debug.Log("Clashing Line B");
            Clash(timelineB, enemylineB);
        }
        if (enemylineC.Count > 0)
        {
            Debug.Log("Clashing Line C");
            Clash(timelineC, enemylineC);
        }
        WaitForPlayerTurn();
    }

    private void DealLineDamage(List<CardPropertiesDrag> line, int totalDamage)
    {
        if (!(totalDamage > 0))
        {
            Debug.Log($"Skipping dealing {totalDamage} damage");
            return;
        }

        Debug.Log($"Dealing {totalDamage} damage to {line.Count} cards.");

        // Calculate damage dealt to each player card
        int damagePerCard = (int) Mathf.Floor(
                (float) totalDamage / line.Count);
        if (damagePerCard == 0)
        {
            Debug.Log("Dealing 1 damage to each card until "
                      + $"totalDamage ({totalDamage}) runs out");
            for (int i = 0; i < totalDamage; i++)
            {
                line[i].card.health -= 1;
            }
            return;
        }

        Debug.Log($"Raw damage per card: {(float) totalDamage / line.Count}");
        Debug.Log("Ceil damage per card: "
                  + $"{Mathf.Ceil((float) totalDamage / line.Count)}");
        Debug.Log("Floor damage per card: "
                  + $"{Mathf.Floor((float) totalDamage / line.Count)}");
        // Deal extra damage to first card in case the damage per
        // doesn't round nicely
        int extraDamage = (int) Mathf.Ceil(
                (float) totalDamage / line.Count) - damagePerCard;
        Debug.Log($"Dealing extra {extraDamage} damage to "
                  + "first card.");
        line[0].card.health -= extraDamage;

        Debug.Log($"Dealing {damagePerCard} damage to "
                  + "each card.");
        // Deal damage to each card, ensuring health is never negative
        foreach (CardPropertiesDrag card in line)
        {
            Debug.Log($"Card health before damage: {card.card.health}");
            Debug.Log($"Card health minus damage: "
                      + $"{card.card.health - damagePerCard}");
            card.card.health = Mathf.Max(
                    0, card.card.health - damagePerCard);
            Debug.Log($"Card health after damage: {card.card.health}");
            ShowFloatingText(card.transform.position, damagePerCard, true);
        }
    }

    private void Clash(List<CardPropertiesDrag> playerLine,
                       List<CardPropertiesDrag> enemyLine)
    {
        // Calculate total enemy attack
        int enemyAttack = 0;
        foreach (CardPropertiesDrag card in enemyLine)
        {
            enemyAttack += card.card.attack;
        }

        // Check if there are player cards.
        // Deal damage to player if there are none.
        if (playerLine.Count == 0)
        {
            Debug.Log("No player cards in line. Damaging player directly for "
                      + $"{enemyAttack} damage");
            gameManager.playerHealth = Mathf.Max(
                    0, gameManager.playerHealth - enemyAttack);
            return;
        }

        // Calculate total player attack
        int playerAttack = 0;
        foreach (CardPropertiesDrag card in playerLine)
        {
            playerAttack += card.card.attack;
        }

        // Calculate total player defence
        int playerDefence = 0;
        foreach (CardPropertiesDrag card in playerLine)
        {
            playerDefence += card.card.health;
        }

        // Calculate total enemy defence
        int enemyDefence = 0;
        foreach (CardPropertiesDrag card in enemyLine)
        {
            enemyDefence += card.card.health;
        }

        // The player is outnumbered in that line
        // and dealt more damage than their units can defend
        if (playerLine.Count < enemyLine.Count && playerDefence < enemyAttack)
        {
            Debug.Log("Player is outnumbered and overwhelmed."
                      + "Damaging player directly for "
                      + $"{enemyAttack - playerDefence} damage");
            // Deal excess damage to the player directly
            gameManager.playerHealth = Mathf.Max(
                    0,
                    gameManager.playerHealth - (enemyAttack - playerDefence));
        }
        // The enemy is outnumbered in that line
        // and dealt more damage than its cards can defend
        else if (playerLine.Count > enemyLine.Count && playerAttack > enemyDefence)
        {
            Debug.Log("Enemy is outnumbered and overwhelmed."
                      + "Reducing enemy damage by "
                      + $"{playerAttack - enemyDefence} damage");
            // Reduce damage dealt by enemies
            enemyAttack = Mathf.Max(
                    0, enemyAttack - (playerAttack - enemyDefence));
        }

        Debug.Log($"Dealing {enemyAttack} damage to player line");
        DealLineDamage(playerLine, enemyAttack);
        Debug.Log($"Dealing {playerAttack} damage to enemy line");
        DealLineDamage(enemyLine, playerAttack);

        foreach (CardPropertiesDrag card in playerLine)
        {
            card.AssignInfo();
        }

        foreach (CardPropertiesDrag card in enemyLine)
        {
            card.AssignInfo();
        }
    }

    private void AfterClash()
    {
        DestroyCardsWithZeroHealth(timelineA);
        DestroyCardsWithZeroHealth(timelineB);
        DestroyCardsWithZeroHealth(timelineC);
        DestroyCardsWithZeroHealth(enemylineA);
        DestroyCardsWithZeroHealth(enemylineB);
        DestroyCardsWithZeroHealth(enemylineC);
    }

    private void DestroyCardsWithZeroHealth(List<CardPropertiesDrag> cardList)
    {
        foreach (CardPropertiesDrag card in cardList)
        {
            if (card.card.health <= 0)
            {
                Destroy(card.gameObject);
            }
        }
    }


    private void ShowFloatingText(Vector3 worldPosition, int text, bool isDamage)
    {
        GameObject floatingTextInstance = Instantiate(floatingTextPrefab, mainCanvas.transform);
        CreateFloatingText floatingText = floatingTextInstance.GetComponent<CreateFloatingText>();

        if (floatingText != null)
        {
            floatingText.Initialize(text, worldPosition, isDamage);
        }
    }

    private void WaitForPlayerTurn()
    {
        StartCoroutine(WaitAndProceed());
    }

    private IEnumerator<object> WaitAndProceed()
    {
        yield return new WaitForSeconds(2.5f);
        AfterClash();
        this.GetComponent<WaveManager>().NextWave();
        endTurnButton.interactable = true;
    }
}
