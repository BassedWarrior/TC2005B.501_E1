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
    [SerializeField] private GameObject cardInfo;
    private bool openInfo;
    private CardCreator selectedCard;
    private bool canDrag= true;
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowInfo(false);
        }

        if (Input.GetMouseButtonDown(0) && canDrag)
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
                //currentCard.spriteRenderer.sortingLayerName = "ForegroundCanvas";
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
            //currentCard.spriteRenderer.sortingLayerName = "GameObjects";
        }
        else if (currentCard != null && currentCard.isDrag)
        {
            mousePosition = Input.mousePosition;
            mousePosition.z = -mainCamera.transform.position.z;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            //currentCard.spriteRenderer.sortingLayerName = "ForegroundCanvas";
            currentCard.transform.localScale = new Vector3(2.2f, 2.2f, 0);
            currentCard.transform.position = new Vector3(worldPosition.x, worldPosition.y, currentCard.transform.position.z);
        }
        if(Input.GetMouseButtonDown(1))
        {
            hitInfo = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));
            if (hitInfo.collider != null && hitInfo.collider.CompareTag("Card") && !openInfo)
            {
                CardPropertiesDrag cardProperties = hitInfo.collider.GetComponent<CardPropertiesDrag>();
                if (cardProperties != null)
                {
                    selectedCard= cardProperties.card;
                    cardProperties.AssignInfo();
            
                    Image cardImage = cardInfo.transform.Find("CardImage").GetComponent<Image>();
                    //cardImage.sprite = selectedCard.artwork;
                    ShowInfo(true);
                }
            }
        }
    }
    public void ShowInfo(bool show)
    {
        cardInfo.SetActive(show);
        canDrag = !show;
    }
}
