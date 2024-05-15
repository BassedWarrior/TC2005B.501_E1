using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class CollectionManager : MonoBehaviour
{
    [SerializeField] private GameObject cardInfo;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardArea;
    [SerializeField] private GameObject deckPrefab;
    [SerializeField] private Transform deckArea;
    private GameManager gameManager;
    private GameObject removeButton;
    private GameObject addButton;
    private Camera mainCamera;
    private RaycastHit2D hit;
    private string selectedCard;
    private float clickStartTime = 0f;
    private float requiredHoldTime = 0.5f;
    private bool isLongClick = false;

    private void Start()
    {
        ShowInfo(false);
        InicializarCartas();
        ActualizarDeck();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            ShowInfo(false);
        }

        if (Input.GetKeyUp(KeyCode.Space) && cardInfo.activeSelf)
        {
            if (addButton.activeSelf)
            {
                AddCard();
            }
            else if (removeButton.activeSelf)
            {
                RemoveCard();
            }
        }

        if (Input.GetMouseButtonDown(0) && !isLongClick && !cardInfo.activeSelf)
        {
            clickStartTime = Time.time;
        }

        if ((Input.GetMouseButtonUp(0)) && !cardInfo.activeSelf)
        {
            float clickDuration = Time.time - clickStartTime;
            if (clickDuration >= requiredHoldTime && !isLongClick)
            {
                isLongClick = true;
                Debug.Log("Clic prolongado");
                if (ComprobarTouch() && addButton.activeSelf)
                {
                    AddCard();
                }
                else if (removeButton.activeSelf)
                {
                    RemoveCard();
                }
                isLongClick = false;
            }
            else
            {
                Debug.Log("Clic normal");
                isLongClick = false;
                ComprobarTouch();
            }
        }
    }

    private void InicializarCartas()
    {
        int index = 0;
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        removeButton = cardInfo.transform.Find("RemoveButton").gameObject;
        addButton = cardInfo.transform.Find("AddButton").gameObject;
        foreach (CardCreator card in gameManager.cards)
        {
            GameObject newCard = Instantiate(cardPrefab, cardArea);
            CardProperties cardProperties = newCard.GetComponent<CardProperties>();
            cardProperties.card= card;
            newCard.name= card.name;
            cardProperties.cardIndex= index;
            index++;
        }
    }

    private bool ComprobarTouch()
    {
        hit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));
        if (hit.collider != null && hit.collider.CompareTag("Card") && hit.collider.name != "EmptyCard")
        {
            CardProperties cardProperties = hit.collider.GetComponent<CardProperties>();
            ShowInfo(true);
            
            if (cardProperties != null)
            {
                cardProperties.AssignInfo();
                Debug.Log("tap = " + cardProperties.card.name);
                selectedCard = cardProperties.card.name;
                isSelected(cardProperties.cardIndex);
        
                Image cardImage = cardInfo.transform.Find("CardImage").GetComponent<Image>();
                cardImage.sprite = cardProperties.card.artwork;
        
                return true;
            }
        }
        return false;
    }


    public void ShowInfo(bool show)
    {
        cardInfo.SetActive(show);
    }

    public void AddCard()
    {
        CardCreator selectedCardCreator = gameManager.cards.Find(card => card.name == selectedCard);
        if (selectedCardCreator != null)
        {
            int index = gameManager.cards.IndexOf(selectedCardCreator);
            if (index != -1 && !gameManager.playersDeck.Contains(index))
            {
                gameManager.playersDeck.Add(index);
                ActualizarDeck();
            }
        }
        ShowInfo(false);
    }

    public void RemoveCard()
    {
        int indexToRemove = gameManager.cards.FindIndex(card => card.name == selectedCard);
        if (indexToRemove != -1 && gameManager.playersDeck.Contains(indexToRemove))
        {
            gameManager.playersDeck.Remove(indexToRemove);
            ActualizarDeck();
        }
        ShowInfo(false);
    }

    public void ActualizarDeck()
    {
        foreach (Transform child in deckArea)
        {
            Destroy(child.gameObject);
        }
        int index=0;
        for (int i = 0; i < 18; i++)
        {
            GameObject collectionCard = Instantiate(cardPrefab, deckArea);
            if (i < gameManager.playersDeck.Count && gameManager.playersDeck[i] >= 0 && gameManager.playersDeck[i] < gameManager.cards.Count && gameManager.cards[gameManager.playersDeck[i]] != null)
            {
                Color color = collectionCard.GetComponent<Image>().color;
                Color newColor = new Color(color.r, color.g, color.b, 1f);
                collectionCard.GetComponent<Image>().color = newColor;
                
                CardProperties cardProperties = collectionCard.GetComponent<CardProperties>();
                cardProperties.card= gameManager.cards[gameManager.playersDeck[i]];
                collectionCard.name= gameManager.cards[gameManager.playersDeck[i]].name;
                cardProperties.cardIndex= index;
                index++;
            }
            else
            {
                collectionCard.name = "EmptyCard";
            }
        }
    }

    private void isSelected(int index)
    {
        if (removeButton != null && addButton != null)
        {
            if (gameManager.playersDeck.Contains(index))
            {
                removeButton.SetActive(true);
                addButton.SetActive(false);
            }
            else
            {
                removeButton.SetActive(false);
                addButton.SetActive(true);
            }
        }
    }

    public void PlayScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}