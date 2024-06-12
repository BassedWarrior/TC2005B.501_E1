using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotController : MonoBehaviour
{
    [SerializeField] private MoveManager moveManager;
    [SerializeField] private ClashTime clashTime;
    [SerializeField] private int maxElements;
    [SerializeField] private float spacing;
    private BoxCollider2D boxCollider;
    private int lastChildCount;
    private bool previousDragState;
    [SerializeField] private bool isEnemy;
    [SerializeField] private bool isDeck;
    [SerializeField] private bool isParadoxCollector;
    [SerializeField] private bool isQuantumTunnel;
    private List<CardPropertiesDrag> currentCards;

    private void Start()
    {
        lastChildCount = transform.childCount;
        currentCards = new List<CardPropertiesDrag>();
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        previousDragState = false;
        UpdateCardList();
        OrganizeCards();
    }

    void Update()
    {
        int currentChildCount = transform.childCount;
        
        if (GameManager.Instance.turnFinished)
        {
            StartCoroutine(TurnFinished());
        }
        else if (currentChildCount != lastChildCount || moveManager.cardPlaced || previousDragState != moveManager.isDragging)
        {
            if (currentChildCount != lastChildCount)
            {
                if (!isDeck)
                {
                    UpdateCardList();
                }
                lastChildCount = currentChildCount;
            }
            OrganizeCards();
            if (previousDragState != moveManager.isDragging)
            {
                previousDragState = moveManager.isDragging;
            }
            moveManager.cardPlaced = false;
        }

        if (moveManager.isParadoxCard)
        {
            boxCollider.enabled = isParadoxCollector;
        }
        else if (moveManager.isDragging && !isEnemy)
        {
            if (!moveManager.isOnBoard)
            {
                boxCollider.enabled = isQuantumTunnel;
            }
            else
            {
                boxCollider.enabled = !isDeck && !isParadoxCollector;
            }
        }
        else
        {
            boxCollider.enabled = false;
        }
    }

    private void UpdateCardList()
    {
        currentCards.Clear();
        
        foreach (Transform child in transform)
        {
            CardPropertiesDrag card = child.GetComponent<CardPropertiesDrag>();
            if (card != null && card.card.IsAlive())
            {
                card.card.ResetDamage();
                currentCards.Add(card);
            }
        }

        clashTime.UpdateLists(currentCards, transform.name);
    }

    private void OrganizeCards()
    {
        Transform deckTransform = transform;
        int cardCount = deckTransform.childCount;
        Vector3 startPosition = deckTransform.position + Vector3.left * spacing * (cardCount - 1) / 2f;

        for (int i = 0; i < cardCount; i++)
        {
            Transform cardTransform = deckTransform.GetChild(i);
            cardTransform.localScale = isDeck ? new Vector3(2.2f, 2.2f, 0) : new Vector3(1.8f, 1.8f, 0);
            Vector3 cardPosition = startPosition - Vector3.left * i * spacing;
            cardTransform.position = cardPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isEnemy && other.CompareTag("Card") && lastChildCount < maxElements)
        {
            CardPropertiesDrag card = other.GetComponent<CardPropertiesDrag>();
            if (card != null && card.isDrag)
            {
                card.actualParent = transform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isEnemy && other.CompareTag("Card"))
        {
            CardPropertiesDrag card = other.GetComponent<CardPropertiesDrag>();
            if (card != null && card.actualParent != card.originalParent) 
            {
                card.actualParent = card.originalParent;
            }
        }
    }

    private IEnumerator TurnFinished()
    {
        OrganizeCards();
        UpdateCardList();
        yield return new WaitForSeconds(1f);
        GameManager.Instance.turnFinished = false;
    }
}