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
        if (currentChildCount != lastChildCount)
        {
            if(!isDeck)
            {
                UpdateCardList();
            }
            OrganizeCards();
            lastChildCount = currentChildCount;
        }

        if (moveManager.cardPlaced)
        {
            OrganizeCards();
        }
        else if (previousDragState != moveManager.isDragging)
        {
            OrganizeCards();
            previousDragState = moveManager.isDragging;
        }

        if (moveManager.isDragging && !isEnemy)
        {
            if (!moveManager.isOnBoard)
            {
                if(isQuantumTunnel)
                {
                    boxCollider.enabled = true;
                }
                else
                {
                    boxCollider.enabled = false;
                }
            }
            else
            {
                if (isDeck)
                {
                    boxCollider.enabled = false;
                }
                else
                {
                    boxCollider.enabled = true;
                }
            }
        }
        else
        {
            boxCollider.enabled = false;
        }
    }

    private void UpdateCardList()
    {
        List<CardPropertiesDrag> previousCards = new List<CardPropertiesDrag>(currentCards);
        currentCards.Clear();
        
        foreach (Transform child in transform)
        {
            CardPropertiesDrag card = child.GetComponent<CardPropertiesDrag>();
            if (card != null)
            {
                currentCards.Add(card);
            }
        }

        clashTime.UpdateLists(previousCards, currentCards, transform.name);
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
        moveManager.cardPlaced = false;
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
}
