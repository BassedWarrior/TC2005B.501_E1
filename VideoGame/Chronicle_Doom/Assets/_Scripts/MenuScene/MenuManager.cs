using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject cardInfo;
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cardDescription;
    [SerializeField] private TextMeshProUGUI cardCost;
    [SerializeField] private TextMeshProUGUI cardAttack;
    [SerializeField] private TextMeshProUGUI cardHealth;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject dummyCard;
    [SerializeField] private Transform cardArea;
    [SerializeField] private Transform deckArea;
    [SerializeField] private GameObject settings;
    [SerializeField] private Button principalButton;
    [SerializeField] private Button collectionButton;
    [SerializeField] private Button highScoreButton;
    [SerializeField] private Button updateDeckButton;
    [SerializeField] private TextMeshProUGUI deckName;
    [SerializeField] private GameObject deckErrorPanel;
    [SerializeField] private TextMeshProUGUI collectionName;
    [SerializeField] private TextMeshProUGUI deckError;
    private Camera mainCamera;
    private RaycastHit2D hit;
    private bool openInfo;
    private CardData selectedCard = null;
    private float transitionDuration= 0.5f;
    private float initialCameraX;
    private bool transitioning = false;
    private float transitionTimer = 0.0f;
    private float targetXPosition;
    public List<GameObject> displayCards = new List<GameObject>();
    public GameObject deckMessage;
    public Button playButton;
    public Button exitButton;
    private void Start()
    {
        mainCamera = Camera.main;
        initialCameraX = mainCamera.transform.position.x;
        principalButton.interactable = false;
        if (GameManager.Instance.cards.Count > 0)
        {
           selectedCard = GameManager.Instance.cards[0];
        }
        ShowInfo(false);
        InitializeCards();
        UpdateDeck();
        deckName.text = PlayerPrefs.GetString("username") + "'s Deck";
        collectionName.text = PlayerPrefs.GetString("username")
                + "'s Collection";
        deckErrorPanel.SetActive(false);
        deckMessage.SetActive(false);
        playButton.onClick.AddListener(() => LetsPlay());
        exitButton.onClick.AddListener(() => GameManager.Instance.GetComponent<SceneChanger>().ChangeToLoginScene());
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
        if (GameManager.Instance.playersDeck.Count < 18 && updateDeckButton.onClick != null)
        {
            updateDeckButton.onClick.RemoveAllListeners();
            updateDeckButton.onClick.AddListener(() => ShowDeckError("You must have 18 cards in your deck!"));
        }
        else if (GameManager.Instance.playersDeck.Count >= 18 && updateDeckButton.onClick != null)
        {
            updateDeckButton.onClick.RemoveAllListeners();
            updateDeckButton.onClick.AddListener(() => GameManager.Instance.GetComponents<APIConnection>()[0].UpdateUsersDeck());
            updateDeckButton.onClick.AddListener(() => ShowDeckError("Deck Updated!"));
        }
    }
    private void InitializeCards()
    {
        foreach (CardData card in GameManager.Instance.cards)
        {
            GameObject newCard = Instantiate(cardPrefab, cardArea);
            displayCards.Add(newCard);
            CardProperties cardProperties = newCard.GetComponentInChildren<CardProperties>();
            cardProperties.inclusiveType = true;
            cardProperties.SetCardData(card);
            newCard.name = card.name;
        }
    }
   private void ComprobarTouch(bool status)
{
    hit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));

    if (hit.collider == null)
    {
        return;
    }

    CardProperties cardProperties = hit.collider.GetComponent<CardProperties>();

    if (cardProperties == null || !hit.collider.CompareTag("Card") || hit.collider.transform.parent == null || hit.collider.transform.parent.name == "EmptyCard" || openInfo)
    {
        return; 
    }

    selectedCard = cardProperties.card;
    ControlInfo(selectedCard);

    if (status && !cardProperties.withMask)
    {
        CardInstruction(selectedCard.cardID-1, cardProperties.inclusiveType);
    }
    else if (!status)
    {
        ShowInfo(true);
    }
}

    private void CardInstruction(int index, bool isInclusive)
    {
        if (!isInclusive)
        {
            if (ListContains(GameManager.Instance.playersDeck, index, 1))
            {
                GameManager.Instance.playersDeck.Remove(index);
                UpdateDeck();
            }
        }
        else
        {
            if (!ListContains(GameManager.Instance.playersDeck, index, 3))
            {
                if(GameManager.Instance.playersDeck.Count < 18)
                {
                    GameManager.Instance.playersDeck.Add(index);
                    UpdateDeck();
                }
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
            if(GameManager.Instance.playersDeck.Count >= 18)
            {
                cardProperties.withMask = true;
                mask.SetActive(true);
            }
            else 
            {
                if(ListContains(GameManager.Instance.playersDeck, cardProperties.card.cardID-1, 3))
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
        }
        GameManager.Instance.SortDeck();
        for (int i = 0; i < 18; i++)
        {
            GameObject collectionCard;

            if (i < GameManager.Instance.playersDeck.Count && GameManager.Instance.playersDeck[i] >= 0 && GameManager.Instance.playersDeck[i] < GameManager.Instance.cards.Count && GameManager.Instance.cards[GameManager.Instance.playersDeck[i]] != null)
            {
                collectionCard = Instantiate(cardPrefab, deckArea);

                CardProperties cardProperties = collectionCard.GetComponentInChildren<CardProperties>();
                cardProperties.inclusiveType = false;
                cardProperties.SetCardData(GameManager.Instance.cards[GameManager.Instance.playersDeck[i]]);
                collectionCard.name = cardProperties.card.name;
            }
            else
            {
                collectionCard = Instantiate(dummyCard, deckArea);
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

    private void ControlInfo(CardData card)
    {
        if (card != null)
        {
            Sprite loadedSprite = Resources.Load<Sprite>("Sprite/Artwork" + card.cardID.ToString());
            if (loadedSprite != null)
            {
                cardImage.sprite = loadedSprite;
            }
            cardName.text = card.name;
            cardDescription.text = card.description;
            cardCost.text = card.cost.ToString();
            cardAttack.text = card.attack.ToString();
            cardHealth.text = card.health.ToString();
        }
    }

    public void ShowDeckError(string message)
    {
        StartCoroutine(ErrorHandler(message));
    }

    private IEnumerator ErrorHandler(string message)
    {
        deckError.text = message;
        deckErrorPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        deckErrorPanel.SetActive(false);
    }

    private void LetsPlay()
    {
        if (GameManager.Instance.playersDeck.Count == 18)
        {
            GameManager.Instance.GetComponent<SceneChanger>().ChangeToGameScene();
        }
        else
        {
            deckMessage.SetActive(true);
        }
    }
}
