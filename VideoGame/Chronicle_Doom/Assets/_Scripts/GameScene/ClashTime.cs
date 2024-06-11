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
    [SerializeField] private Transform paradoxCollector;
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Button endTurnButton;

    public void Start()
    {
        endTurnButton.onClick.AddListener(() => StartCoroutine(Clash()));
    }

    private List<CardPropertiesDrag> GetListByName(string name)
    {
        switch (name)
        {
            case "TimeLineA": return timelineA;
            case "TimeLineB": return timelineB;
            case "TimeLineC": return timelineC;
            case "EnemyLineA": return enemylineA;
            case "EnemyLineB": return enemylineB;
            case "EnemyLineC": return enemylineC;
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

        // Initialize enemy count for each line
        int enemiesInA = enemyAreaA.childCount;
        int enemiesInB = enemyAreaB.childCount;
        int enemiesInC = enemyAreaC.childCount;

        System.Random random = new System.Random();

        foreach (Transform child in children)
        {
            if (child.CompareTag("Enemy"))
            {
                CardPropertiesDrag card = child.GetComponent<CardPropertiesDrag>();
                if (card != null)
                {
                    bool assigned = false;
                    List<int> lines = new List<int> { 0, 1, 2 };
                    
                    while (lines.Count > 0 && !assigned)
                    {
                        int index = random.Next(lines.Count);
                        int randomLine = lines[index];
                        lines.RemoveAt(index);

                        switch (randomLine)
                        {
                            case 0:
                                if (enemiesInA < 4)
                                {
                                    card.transform.SetParent(enemyAreaA);
                                    enemiesInA++;
                                    assigned = true;
                                }
                                break;
                            case 1:
                                if (enemiesInB < 4)
                                {
                                    card.transform.SetParent(enemyAreaB);
                                    enemiesInB++;
                                    assigned = true;
                                }
                                break;
                            case 2:
                                if (enemiesInC < 4)
                                {
                                    card.transform.SetParent(enemyAreaC);
                                    enemiesInC++;
                                    assigned = true;
                                }
                                break;
                        }
                    }

                    // Destroy the card if it couldn't be assigned to a line, maybe you would die too xd
                    if (!assigned)
                    {
                        Destroy(card.gameObject);
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

        // Reset damage dealt to each card
        ResetDamage(line);

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

        // Add extra damage to first card in case the damage per doesn't round nicely
        int extraDamage = (int) Mathf.Ceil(
                (float) totalDamage / line.Count) - damagePerCard;
        line[0].card.AddDamage(extraDamage);

        // Add damage per card to each card
        foreach (CardPropertiesDrag card in line)
        {
            if (card.card.IsAlive())
            {
                card.card.AddDamage(damagePerCard);
                card.ShowFloatingText(card.transform.position, card.card.damage, true, true);
            }
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
            // Debug.Log("List not found");
            return;
        }

        // Limpiar la lista objetivo para eliminar cualquier carta que ya no esté presente
        targetList.Clear();

        // Saltar si no hay cartas en la lista actual
        if (currentCards.Count == 0)
        {
            // Debug.Log("No cards in list");
            return;
        }
        // Agregar las cartas actuales a la lista objetivo
        foreach (CardPropertiesDrag card in currentCards)
        {
            targetList.Add(card);
        }

        GameManager.Instance.DeleteDots();
        // Realizar cálculos de clash después de actualizar las listas
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
            if (card.card.IsAlive() && card != null)
            {
                card.ShowFloatingText(card.transform.position, card.card.damage, true, false);
                card.card.ApplyDamage();
            }
        }

        // Deal damage to each player card
        foreach (CardPropertiesDrag card in playerLine)
        {
            if (card.card.IsAlive() && card != null)
            {
                card.ShowFloatingText(card.transform.position, card.card.damage, true, false);
                card.card.ApplyDamage();
            }
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
            Destroy(card.gameObject);
        }
    }
    
    private IEnumerator<object> Clash()
    {
        endTurnButton.interactable = false;

        if (paradoxCollector.childCount > 0)
        {
            ResetDamage(timelineA);
            ResetDamage(timelineB);
            ResetDamage(timelineC);
            ResetDamage(enemylineA);
            ResetDamage(enemylineB);
            ResetDamage(enemylineC);
            foreach (Transform child in paradoxCollector)
            {   
                if (child != null && child.CompareTag("Card"))
                {
                    CardPropertiesDrag card = child.GetComponent<CardPropertiesDrag>();
                    if (card != null)
                    {
                        foreach (CardAbility ability in card.cardAbilities)
                        {
                            EvokeAbilities(ability);
                        }
                    }
                }
            }

            DealLineDamage(timelineA, enemylineA);
            DealLineDamage(timelineB, enemylineB);
            DealLineDamage(timelineC, enemylineC);
            InfoUpdateAll();
            yield return new WaitForSeconds(3f);
            AfterClash();
            CalculateLineClash(timelineA, enemylineA);
            CalculateLineClash(timelineB, enemylineB);
            CalculateLineClash(timelineC, enemylineC);
        }

        DealLineDamage(timelineA, enemylineA);
        DealLineDamage(timelineB, enemylineB);
        DealLineDamage(timelineC, enemylineC);

        GameManager.Instance.ApplyPlayerDamage();

        InfoUpdateAll();

        DestroyUsedCards(paradoxCollector);
        yield return new WaitForSeconds(3f);
        AfterClash();
        this.GetComponent<WaveManager>().NextWave();
        endTurnButton.interactable = true;
    }

    private void InfoUpdateAll()
    {
        InfoUpdate(timelineA);
        InfoUpdate(timelineB);
        InfoUpdate(timelineC);
        InfoUpdate(enemylineA);
        InfoUpdate(enemylineB);
        InfoUpdate(enemylineC);
    }

    private void DestroyUsedCards(Transform cardCollector)
    {
        foreach (Transform child in cardCollector)
        {
            if (child.CompareTag("Card"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void InfoUpdate(List<CardPropertiesDrag> timeline)
    {
        foreach (CardPropertiesDrag card in timeline)
        {
            card.AssignInfo();
        }
    }

    public void EvokeAbilities(CardAbility ability)
    {
        foreach (string target in ability.targets)
        {
            List<CardPropertiesDrag> targetList = GetListByName(target);
            if (targetList != null)
            {
                foreach (CardPropertiesDrag card in targetList)
                {
                    if (ability.damage != 0)
                    {
                        Debug.Log($"Dealing {ability.damage} damage to {card.card.name} in row {target}");
                        card.card.AddDamage(ability.damage);
                    }
                    if (ability.heal != 0)
                    {
                        Debug.Log($"Healing {ability.heal} health to {card.card.name} in row {target}");
                        card.card.Heal(ability.heal);
                    }
                    if (ability.attack != 0)
                    {
                        Debug.Log($"Adding {ability.attack} attack to {card.card.name} in row {target}");
                        card.card.AddAttack(ability.attack);
                    }
                }
            }
            else
            {
                //Debug.LogWarning($"No target list found for {target}");
            }
        }
    }

    private void ResetDamage(List<CardPropertiesDrag> line)
    {
        foreach (CardPropertiesDrag card in line)
        {
            card.card.ResetDamage();
        }
    }
}
