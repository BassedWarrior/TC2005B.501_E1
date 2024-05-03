using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotController : MonoBehaviour
{
    [SerializeField] private MoveManager moveManager;
    [SerializeField] private int maxElements;
    [SerializeField] private float spacing;
    private BoxCollider2D boxCollider;
    private int lastChildCount;
    private bool previousDragState;
    public bool isDeck;

    private void Start()
    {
        lastChildCount = transform.childCount;
        OrganizarCartas();
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        previousDragState = false;
    }

    void Update()
    {
        int currentChildCount = transform.childCount;
        boxCollider.enabled = moveManager.isDragging;

        if (currentChildCount != lastChildCount)
        {
            lastChildCount = currentChildCount;
            OrganizarCartas();
        }
        else if (previousDragState != moveManager.isDragging)
        {
            OrganizarCartas();
            previousDragState = moveManager.isDragging;
        }
    }

    private void OrganizarCartas()
    {
        Transform deckTransform = transform;
        int cardCount = deckTransform.childCount;
        Vector3 startPosition = deckTransform.position + Vector3.left * spacing * (cardCount - 1) / 2f;

        for (int i = 0; i < cardCount; i++)
        {
            Transform cardTransform = deckTransform.GetChild(i);
            if (!isDeck)
            {
                cardTransform.localScale = new Vector3(1f, 1.43f, 0);
            }
            else
            {
                cardTransform.localScale = new Vector3(1.43f, 2f, 0);
            }
            Vector3 cardPosition = startPosition - Vector3.left * i * spacing;
            cardTransform.position = cardPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Card") && lastChildCount < maxElements)
        {
            Debug.Log("entro");
            CardPropertiesDrag card = other.GetComponent<CardPropertiesDrag>();
            if (card != null && card.isDrag)
            {
                card.actualParent = transform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Card"))
        {
            CardPropertiesDrag card = other.GetComponent<CardPropertiesDrag>();
            if (card != null && card.actualParent == transform) 
            {
                card.actualParent = card.originalParent; 
            }
        }
    }
}
