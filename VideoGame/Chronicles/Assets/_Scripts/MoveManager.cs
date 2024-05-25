using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoveManager : MonoBehaviour
{
    private HandManager handManager;
    private RaycastHit2D hitInfo;
    private Camera mainCamera;
    private Vector3 mousePosition;
    private CardPropertiesDrag currentCard;
    [SerializeField] private GameObject cardInfo;
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cardDescription;
    [SerializeField] private TextMeshProUGUI cardCost;
    [SerializeField] private TextMeshProUGUI cardAttack;
    [SerializeField] private TextMeshProUGUI cardHealth;
    private CardCreator selectedCard;
    private bool canDrag = true;
    public bool isDragging;
    private bool openInfo;

    private void Start()
    {
        mainCamera = Camera.main;
        handManager = GameObject.FindGameObjectWithTag("CardManager").GetComponent<HandManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowInfo(false);
        }
        
        if (Input.GetMouseButtonDown(0) && canDrag && !openInfo)
        {
            hitInfo = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));
            if (hitInfo.collider != null && hitInfo.collider.CompareTag("Card"))
            {
                currentCard = hitInfo.collider.GetComponent<CardPropertiesDrag>();
                if (currentCard != null && currentCard.card != null && (handManager.khronos >= currentCard.card.energyCost || currentCard.isOnBoard))
                {
                    currentCard.isOnBoard= true;
                    isDragging = true;
                    currentCard.isDrag = true;
                    ChangeSortingLayer(hitInfo.collider.transform, "ForegroundCanvas");
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && currentCard != null && currentCard.isDrag && !openInfo)
        {
            isDragging = false;
            handManager.EnergyWaste(currentCard.card.energyCost);
            currentCard.isDrag = false;
            if (currentCard.actualParent != null)
            {
                currentCard.transform.SetParent(currentCard.actualParent);
            }
            ChangeSortingLayer(hitInfo.collider.transform, "GameObject");
        }
        else if (currentCard != null && currentCard.isDrag)
        {
            mousePosition = Input.mousePosition;
            mousePosition.z = -mainCamera.transform.position.z;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            ChangeSortingLayer(hitInfo.collider.transform, "ForegroundCanvas");
            currentCard.transform.localScale = new Vector3(2.2f, 2.2f, 0);
            currentCard.transform.position = new Vector3(worldPosition.x, worldPosition.y, currentCard.transform.position.z);
        }
        if (Input.GetMouseButtonDown(1) && !openInfo)
        {
            hitInfo = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));
            if (hitInfo.collider != null && hitInfo.collider.CompareTag("Card") && !openInfo)
            {
                CardPropertiesDrag cardProperties = hitInfo.collider.GetComponent<CardPropertiesDrag>();
                if (cardProperties != null)
                {
                    selectedCard = cardProperties.card;
                    cardProperties.AssignInfo();
                    ControlInfo(selectedCard);
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

    private void ControlInfo(CardCreator card)
    {
        Debug.Log(card.name);
        if (card != null)
        {
            cardImage.sprite = card.artwork;
            cardName.text = card.name;
            cardDescription.text = card.description;
            cardCost.text = card.energyCost.ToString();
            cardAttack.text = card.attack.ToString();
            cardHealth.text = card.health.ToString();
        }
    }

    private void ChangeSortingLayer(Transform cardTransform, string sortingLayer)
    {
        Canvas canvas = cardTransform.GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            canvas.sortingLayerName = sortingLayer;
        }
    }

}