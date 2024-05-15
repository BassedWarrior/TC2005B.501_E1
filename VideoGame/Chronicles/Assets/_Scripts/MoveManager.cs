using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoveManager : MonoBehaviour
{
    private RaycastHit2D hitInfo;
    private Camera mainCamera;
    private Vector3 mousePosition;
    private CardPropertiesDrag currentCard;
    public bool isDragging;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hitInfo = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));
            if (hitInfo.collider != null && hitInfo.collider.CompareTag("Card"))
            {
                currentCard = hitInfo.collider.GetComponent<CardPropertiesDrag>();
                if (currentCard.actualParent != currentCard.originalParent) 
                {
                    currentCard.actualParent = currentCard.originalParent; 
                }
                isDragging = true;
                currentCard.isDrag = true;
                currentCard.spriteRenderer.sortingLayerName = "Top";
            }
        }
        else if (Input.GetMouseButtonUp(0) && currentCard != null && currentCard.isDrag)
        {
            isDragging = false;
            currentCard.isDrag = false;
            if (currentCard.actualParent != null) 
            {
                currentCard.transform.SetParent(currentCard.actualParent); 
            }
            currentCard.spriteRenderer.sortingLayerName = "Default";
        }
        else if (currentCard != null && currentCard.isDrag)
        {
            mousePosition = Input.mousePosition;
            mousePosition.z = -mainCamera.transform.position.z;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            currentCard.spriteRenderer.sortingLayerName = "Top";
            currentCard.transform.localScale = new Vector3(1.43f, 2f, 0);
            currentCard.transform.position = new Vector3(worldPosition.x, worldPosition.y, currentCard.transform.position.z);
        }
    }
}
