using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClashTime : MonoBehaviour
{
    public List<CardPropertiesDrag> playerLineA = new List<CardPropertiesDrag>();
    public List<CardPropertiesDrag> playerLineB = new List<CardPropertiesDrag>();
    public List<CardPropertiesDrag> playerLineC = new List<CardPropertiesDrag>();
    public List<CardPropertiesDrag> enemyLineA = new List<CardPropertiesDrag>();
    public List<CardPropertiesDrag> enemyLineB = new List<CardPropertiesDrag>();
    public List<CardPropertiesDrag> enemyLineC = new List<CardPropertiesDrag>();
    public List<CardPropertiesDrag> quantumTunnel = new List<CardPropertiesDrag>();
    [SerializeField] private Transform enemySpawner;
    [SerializeField] private Transform enemyAreaA;
    [SerializeField] private Transform enemyAreaB;
    [SerializeField] private Transform enemyAreaC;
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Button endTurnButton;
    public bool turnFinished = false;

    public void Start()
    {
        endTurnButton.onClick.AddListener(Clash);
    }

    private List<CardPropertiesDrag> GetListByName(string listName)
    {
        switch (listName)
        {
            case "PlayerLineA":
                return playerLineA;
            case "PlayerLineB":
                return playerLineB;
            case "PlayerLineC":
                return playerLineC;
            case "QuantumTunnel":
                return quantumTunnel;
            case "EnemyLineA":
                return enemyLineA;
            case "EnemyLineB":
                return enemyLineB;
            case "EnemyLineC":
                return enemyLineC;
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
            foreach (CardPropertiesDrag card in line)
            {
                card.AssignInfo();
            }
            return;
        }

        // Add damage per card to each card
        foreach (CardPropertiesDrag card in line)
        {
            if (card.card.IsAlive())
            {
                card.card.ResetDamage();
                card.card.AddDamage(damagePerCard);
            }
        }

        // Add extra damage to first card in case the damage per
        // doesn't round nicely
        int extraDamage = (int) Mathf.Ceil(
                (float) totalDamage / line.Count) - damagePerCard;
        line[0].card.AddDamage(extraDamage);

        foreach (CardPropertiesDrag card in line)
        {
            card.AssignInfo();
        }
    }

    private void CalculateLineClash(List<CardPropertiesDrag> playerLine,
                                    List<CardPropertiesDrag> enemyLine)
    {
        GameManager.Instance.ResetPlayerDamage();

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
            GameManager.Instance.AddPlayerDamage(enemyAttack);
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
            GameManager.Instance.AddPlayerDamage(enemyAttack - playerDefence);
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

    public void UpdateLists(List<CardPropertiesDrag> currentCards, string listName)
    {
        List<CardPropertiesDrag> targetList = GetListByName(listName);

        if (targetList == null)
        {
            return;
        }

        // Limpiar la lista objetivo para eliminar cualquier carta que ya no esté presente
        targetList.Clear();

        // Agregar las cartas actuales a la lista objetivo
        foreach (CardPropertiesDrag card in currentCards)
        {
            targetList.Add(card);
        }

        // Realizar cálculos de clash después de actualizar las listas
        CalculateLineClash(playerLineA, enemyLineA);
        CalculateLineClash(playerLineB, enemyLineB);
        CalculateLineClash(playerLineC, enemyLineC);
        foreach (CardPropertiesDrag card in quantumTunnel)
        {
            card.AssignInfo();
        }
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
            if (card.card.IsAlive() && card != null)
            {
                card.ShowDamageText(card.card.damage, true, false);
                card.card.ApplyDamage();
            }
        }

        // Deal damage to each player card
        foreach (CardPropertiesDrag card in playerLine)
        {
            if (card.card.IsAlive() && card != null)
            {
                card.ShowDamageText(card.card.damage, true, false);
                card.card.ApplyDamage();
            }
        }
    }

    private void AfterClash()
    {
        DestroyCardsWithZeroHealth(playerLineA);
        DestroyCardsWithZeroHealth(playerLineB);
        DestroyCardsWithZeroHealth(playerLineC);
        DestroyCardsWithZeroHealth(enemyLineA);
        DestroyCardsWithZeroHealth(enemyLineB);
        DestroyCardsWithZeroHealth(enemyLineC);
    }

    private void DestroyCardsWithZeroHealth(List<CardPropertiesDrag> cardList)
    {
        List<CardPropertiesDrag> cardsToRemove = new List<CardPropertiesDrag>();

        foreach (CardPropertiesDrag card in cardList)
        {
            if (card != null && !card.card.IsAlive())
            {
                GameManager.Instance.AddKillScore(card.card.GetScoreValue());
                Debug.Log($"Added {card.card.GetScoreValue()} score for killing card {card.card.cardID}");
                Destroy(card.gameObject);
                cardsToRemove.Add(card);
            }
        }

        foreach (CardPropertiesDrag card in cardsToRemove)
        {
            cardList.Remove(card);
        }
    }


    private void WaitForPlayerTurn()
    {
        StartCoroutine(WaitAndProceed());
    }

    private IEnumerator<object> WaitAndProceed()
    {
        yield return new WaitForSeconds(3f);
        AfterClash();
        this.GetComponent<WaveManager>().NextWave();
        endTurnButton.interactable = true;
    }
    
    public void Clash()
    {
        endTurnButton.interactable = false;
        DealLineDamage(playerLineA, enemyLineA);
        DealLineDamage(playerLineB, enemyLineB);
        DealLineDamage(playerLineC, enemyLineC);

        GameManager.Instance.ApplyPlayerDamage();

        foreach (CardPropertiesDrag card in playerLineA)
        {
            card.AssignInfo();
        }

        foreach (CardPropertiesDrag card in playerLineB)
        {
            card.AssignInfo();
        }

        foreach (CardPropertiesDrag card in playerLineC)
        {
            card.AssignInfo();
        }

        turnFinished = true;
        WaitForPlayerTurn();
    }
}
