using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject cardInfo;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardArea;
    [SerializeField] private Transform deckArea;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject objectToFollow;
    [SerializeField] private Button principalButton;
    [SerializeField] private Button collectionButton;
    [SerializeField] private Button highScoreButton;
    private GameManager gameManager;
    private Camera mainCamera;
    private RaycastHit2D hit;
    private bool openInfo;
    private CardCreator selectedCard;
    private float transitionDuration= 0.5f;
    private float initialCameraX;
    private bool transitioning = false;
    private float transitionTimer = 0.0f;
    private float targetXPosition;
    public List<GameObject> displayCards = new List<GameObject>();
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        mainCamera = Camera.main;
        initialCameraX = mainCamera.transform.position.x;
        principalButton.interactable = false;
        if (gameManager.cards.Count > 0)
        {
           selectedCard = gameManager.cards[0];
        }
        ShowInfo(false);
        InitializeCards();
        UpdateDeck();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            ShowInfo(false);
            ShowSettings(false);
        }
        if ((Input.GetMouseButtonDown(0)) && !openInfo)
        {
            ComprobarTouch(true);
        }
        if ((Input.GetMouseButtonDown(1)) && !openInfo)
        {
            ComprobarTouch(false);
        }
        if (transitioning)
        {
            transitionTimer += Time.deltaTime;

            float t = Mathf.Clamp01(transitionTimer / transitionDuration);
            float newXPosition = Mathf.Lerp(initialCameraX, targetXPosition, t);

            Vector3 newPosition = new Vector3(newXPosition, mainCamera.transform.position.y, mainCamera.transform.position.z);
            mainCamera.transform.position = newPosition;

            if (t >= 1.0f)
            {
                initialCameraX= targetXPosition;
                transitioning = false;
            }
        }
        if (objectToFollow != null && settings != null)
        {
            objectToFollow.transform.position = new Vector3(mainCamera.transform.position.x, objectToFollow.transform.position.y, objectToFollow.transform.position.z);
            settings.transform.position = new Vector3(mainCamera.transform.position.x, settings.transform.position.y, settings.transform.position.z);
        }
    }
    private void InitializeCards()
    {
        foreach (CardCreator card in gameManager.cards)
        {
            GameObject newCard = Instantiate(cardPrefab, cardArea);
            displayCards.Add(newCard);
            CardProperties cardProperties = newCard.GetComponentInChildren<CardProperties>();
            cardProperties.inclusiveType = true;
            cardProperties.card = card;
            newCard.name = card.name;
        }
    }
    private bool ComprobarTouch(bool mood)
    {
        hit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));
        if (hit.collider != null && hit.collider.CompareTag("Card") && hit.collider.name != "EmptyCard" && !openInfo)
        {
            CardProperties cardProperties = hit.collider.GetComponent<CardProperties>();
            if (cardProperties != null)
            {
                selectedCard= cardProperties.card;
                cardProperties.AssignInfo();
                //Debug.Log("tap = " + cardProperties.card.name);
        
                Image cardImage = cardInfo.transform.Find("CardImage").GetComponent<Image>();
                cardImage.sprite = selectedCard.artwork;
            }
            Debug.Log("tap = " + cardProperties.withMask);
            if(mood && !cardProperties.withMask)
            {
                CardInstruction(selectedCard.ID, cardProperties.inclusiveType);
            }
            else if(!mood)
            {
                ShowInfo(true);
            }
            return true;
        }
        return false;
    }
    private void CardInstruction(int index, bool isInclusive)
    {
        if (!isInclusive)
        {
            if (ListContains(gameManager.playersDeck, index, 1))
            {
                gameManager.playersDeck.Remove(index);
                UpdateDeck();
            }
        }
        else
        {
            if (!ListContains(gameManager.playersDeck, index, 3))
            {
                gameManager.playersDeck.Add(index);
                UpdateDeck();
            }
        }
    }
    public void UpdateDeck()
    {
        foreach (Transform child in deckArea)
        {
            Destroy(child.gameObject);
        }
        foreach(GameObject card in displayCards)
        {
            CardProperties cardProperties = card.GetComponentInChildren<CardProperties>();
            GameObject mask = card.transform.Find("cardSelectedMask").gameObject;
            if(ListContains(gameManager.playersDeck, cardProperties.card.ID, 3))
            {
                cardProperties.withMask = true;
                mask.SetActive(true);
            }
            else
            {
                cardProperties.withMask = false;
                mask.SetActive(false);
            }
        }
        gameManager.SortDeck();
        for (int i = 0; i < 18; i++)
        {
            GameObject collectionCard = Instantiate(cardPrefab, deckArea);
            if (i < gameManager.playersDeck.Count && gameManager.playersDeck[i] >= 0 && gameManager.playersDeck[i] < gameManager.cards.Count && gameManager.cards[gameManager.playersDeck[i]] != null)
            {
                CardProperties cardProperties = collectionCard.GetComponentInChildren<CardProperties>();
                cardProperties.inclusiveType = false;
                cardProperties.card= gameManager.cards[gameManager.playersDeck[i]];
                collectionCard.name= cardProperties.card.name;
            }
            else
            {
                collectionCard.name = "EmptyCard";
            }
        }
    }
    public void ShowSettings(bool show)
    {
        settings.SetActive(show);
        openInfo= show;
    }
    public void ShowInfo(bool show)
    {
        cardInfo.SetActive(show);
        openInfo= show;
    }
    public void MoveToMenu(int menuPosition)
    {
        if (!transitioning)
        {
            targetXPosition = menuPosition;
            transitionTimer = 0.0f;
            transitioning = true;

            principalButton.interactable = (menuPosition != 0);
            collectionButton.interactable = (menuPosition != -20);
            highScoreButton.interactable = (menuPosition != 20);
        }
    }
    bool ListContains(List<int> integerList, int searchingNumber, int timesToFind)
    {
        var query = integerList.Where(x => x == searchingNumber);
        return query.Count() >= timesToFind;
    }
    public void PlayScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}