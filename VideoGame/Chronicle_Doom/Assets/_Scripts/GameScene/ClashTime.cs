using System.Collections.Generic;
using UnityEngine;

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

        foreach (Transform child in children)
        {
            Debug.Log("Relocating enemies");
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
        if (enemylineA.Count > 0)
        {
            Clash(timelineA, enemylineA);
        }
        if (enemylineB.Count > 0)
        {
            Clash(timelineB, enemylineB);
        }
        if (enemylineC.Count > 0)
        {
            Clash(timelineC, enemylineC);
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
            Debug.Log("No player cards in line. Damaging player directly...");
            gameManager.playerHealth -= enemyAttack;
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
            // Deal excess damage to the player directly
            gameManager.playerHealth -= enemyAttack - playerDefence;
        }
        // The enemy is outnumbered in that line
        // and dealt more damage than its cards can defend
        else if (playerLine.Count > enemyLine.Count && playerAttack > enemyDefence)
        {
            // Reduce damage dealt by enemies
            enemyAttack -= playerAttack - playerDefence;
        }

        // Calculate damage dealt to each player card
        int playerDamagePerCard = (int) Mathf.Floor(enemyAttack / playerLine.Count);
        // Deal extra damage to first card in case the damage per doesn't
        // round nicely
        playerLine[0].card.health -= (int) Mathf.Ceil(
                enemyAttack / playerLine.Count) - playerDamagePerCard;
        // Deal damage to each card, ensuring health is never negative
        foreach (CardPropertiesDrag card in playerLine)
        {
            card.card.health = Mathf.Max(
                    0, card.card.health - playerDamagePerCard);
        }

        // Calculate damage dealt to each enemy card
        int enemyDamagePerCard = (int) Mathf.Floor(playerAttack / enemyLine.Count);
        // Deal extra damage to first card in case the damage per doesn't
        // round nicely
        enemyLine[0].card.health -= (int) Mathf.Ceil(
                playerAttack / enemyLine.Count) - enemyDamagePerCard;
        // Deal damage to each card, ensuring health is never negative
        foreach (CardPropertiesDrag card in enemyLine)
        {
            card.card.health = Mathf.Max(
                    0, card.card.health - enemyDamagePerCard);
        }

        foreach (CardPropertiesDrag card in playerLine)
        {
            card.AssignInfo();
        }

        foreach (CardPropertiesDrag card in enemyLine)
        {
            card.AssignInfo();
        }
    }
}
