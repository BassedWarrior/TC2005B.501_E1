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

    private void CalculateLineDamage(List<CardPropertiesDrag> line,
                                     int totalDamage)
    {
        endTurnButton.interactable = false;
        // Skip if line is empty
        if (line.Count == 0)
        {
            return;
        }

        // Add damage dealt to each player card
        int damagePerCard = (int) Mathf.Floor(
                (float) totalDamage / line.Count);
        if (damagePerCard == 0)
        {
            for (int i = 0; i < totalDamage; i++)
            {
                line[i].card.AddDamage(1);
            }
            return;
        }

        // Add damage per card to each card
        foreach (CardPropertiesDrag card in line)
        {
            card.card.ResetDamage();
            card.card.AddDamage(damagePerCard);
            card.AssignInfo();
        }

        // Add extra damage to first card in case the damage per
        // doesn't round nicely
        int extraDamage = (int) Mathf.Ceil(
                (float) totalDamage / line.Count) - damagePerCard;
        line[0].card.AddDamage(extraDamage);
    }

    private void CalculateLineClash(List<CardPropertiesDrag> playerLine,
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
            gameManager.AddPlayerDamage(enemyAttack);
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
            // Deal excess damage to the player directly
            gameManager.AddPlayerDamage(enemyAttack - playerDefence);
        }
        // The enemy is outnumbered in that line
        // and dealt more damage than its cards can defend
        else if (playerLine.Count > enemyLine.Count && playerAttack > enemyDefence)
        {
            // Reduce damage dealt by enemies
            enemyAttack = Mathf.Max(
                    0, enemyAttack - (playerAttack - enemyDefence));
        }

        CalculateLineDamage(playerLine, enemyAttack);
        CalculateLineDamage(enemyLine, playerAttack);
    }

    public void UpdateLists(List<CardPropertiesDrag> previousCards,
                            List<CardPropertiesDrag> currentCards,
                            string listName)
    {
        List<CardPropertiesDrag> targetList = GetListByName(listName);

        if (targetList == null)
        {
            Debug.Log("List not found");
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

        CalculateLineClash(timelineA, enemylineA);
        CalculateLineClash(timelineB, enemylineB);
        CalculateLineClash(timelineC, enemylineC);
    }

    private void DealLineDamage(List<CardPropertiesDrag> playerLine,
                                List<CardPropertiesDrag> enemyLine)
    {
        // Check if there are enemy cards.
        // Skip if there are none.
        if (enemyLine.Count == 0)
        {
            return;
        }

        // Deal damage to each enemy card
        foreach (CardPropertiesDrag card in enemyLine)
        {
            card.card.ApplyDamage();
        }

        // Deal damage to each player card
        foreach (CardPropertiesDrag card in playerLine)
        {
            card.card.ApplyDamage();
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
            if (!card.card.IsAlive())
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
    
    public void Clash()
    {
        DealLineDamage(timelineA, enemylineA);
        DealLineDamage(timelineB, enemylineB);
        DealLineDamage(timelineC, enemylineC);

        gameManager.ApplyPlayerDamage();

        foreach (CardPropertiesDrag card in timelineA)
        {
            card.AssignInfo();
        }

        foreach (CardPropertiesDrag card in timelineB)
        {
            card.AssignInfo();
        }

        foreach (CardPropertiesDrag card in timelineC)
        {
            card.AssignInfo();
        }

        AfterClash();
        this.GetComponent<WaveManager>().NextWave();
    }
}
