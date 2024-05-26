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

    public void UpdateLists(List<CardPropertiesDrag> previousCards, List<CardPropertiesDrag> currentCards, string listName)
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

    private void CheckClash()
    {
        if (timelineA.Count > 0 && enemylineA.Count > 0)
        {
            Clash(timelineA, enemylineA);
        }
        if (timelineB.Count > 0 && enemylineB.Count > 0)
        {
            Clash(timelineB, enemylineB);
        }
        if (timelineC.Count > 0 && enemylineC.Count > 0)
        {
            Clash(timelineC, enemylineC);
        }
    }

    private void Clash(List<CardPropertiesDrag> playerLine, List<CardPropertiesDrag> enemyLine)
    {
        int playerAttack = 0;
        int enemyAttack = 0;

        foreach (CardPropertiesDrag card in playerLine)
        {
            playerAttack += card.card.attack;
        }

        foreach (CardPropertiesDrag card in enemyLine)
        {
            enemyAttack += card.card.attack;
        }

        if (playerAttack > enemyAttack)
        {
            Debug.Log("Player wins");
        }
        else if (playerAttack < enemyAttack)
        {
            Debug.Log("Enemy wins");
        }
        else
        {
            Debug.Log("Draw");
        }
    }
}
