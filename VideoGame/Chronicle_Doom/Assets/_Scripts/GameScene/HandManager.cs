using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardArea;
    [SerializeField] private GameObject selectiveCard;
    [SerializeField] private Transform selectiveArea;
    [SerializeField] private GameObject initialScene;
    private List<GameObject> buttons = new List<GameObject>();
    private List<int> selectedCards = new List<int>();
    private List<bool> activeButtons = new List<bool>();
    [SerializeField] private TextMeshProUGUI cardsRemaining;
    [SerializeField] private TextMeshProUGUI khronosQuantity;
    private int selectiveCards = 5;
    public int khronos;

    void Start()        
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        cardsRemaining.text = gameManager.playersDeck.Count.ToString();
        khronosQuantity.text = khronos.ToString();
        gameManager.ShuffleDeck();
        ShowInitialHand();
    }

    private void ShowInitialHand()
    {
        initialScene.SetActive(true);
        for (int i = 0; i < selectiveCards; i++)
        {
            if (i < gameManager.playersDeck.Count)
            {
                CardData card = gameManager.cards[gameManager.playersDeck[i]];
                GameObject buttonObj= Instantiate(selectiveCard, selectiveArea);
                buttonObj.GetComponent<SelectiveCards>().card= card;
                buttonObj.GetComponent<SelectiveCards>().AssignInfo();
                buttons.Add(buttonObj);
                int index= i;
                buttonObj.GetComponent<Button>().onClick.AddListener(() => ToggleSelection(card.cardID, index));
                activeButtons.Add(true);
            }
        }
    }

    private void ToggleSelection(int ID, int buttonIndex)
    {
        if (!activeButtons[buttonIndex])
        {
            selectedCards.Remove(ID);
            buttons[buttonIndex].GetComponent<SelectiveCards>().artworkImage.color = Color.white;
            activeButtons[buttonIndex] = true;
        }
        else
        {
            selectedCards.Add(ID);
            buttons[buttonIndex].GetComponent<SelectiveCards>().artworkImage.color = Color.green; 
            activeButtons[buttonIndex] = false;
        }
    }

    public void ConfirmSelection()
    {
        foreach (int decision in selectedCards)
        {
            gameManager.playersHand.Add(decision);
            gameManager.playersDeck.Remove(decision);
        }
        selectedCards.Clear();
        Destroy(initialScene);

        CreateHand();
    }

    private void CreateHand()
    {
        foreach (int index in gameManager.playersHand)
        {
            GameObject newCard = Instantiate(cardPrefab, cardArea);
            CardPropertiesDrag cardProperties = newCard.GetComponent<CardPropertiesDrag>();
            cardProperties.card= gameManager.cards[index - 1].DeepCopy();
            cardProperties.AssignInfo();
        }
        if( gameManager.playersHand.Count < 5)
        {
            int faltantes= (5-gameManager.playersHand.Count);
            for(int i=0;  i<faltantes; i++)
            {
                GameObject newCard = Instantiate(cardPrefab, cardArea);
                CardPropertiesDrag cardProperties = newCard.GetComponent<CardPropertiesDrag>();
                if(cardProperties != null)
                {
                    int card= gameManager.playersDeck[i];
                    cardProperties.card= gameManager.cards[card].DeepCopy();
                    cardProperties.AssignInfo();
                    gameManager.playersHand.Add(card);
                    gameManager.playersDeck.Remove(card);
                }
            }
        }
        cardsRemaining.text = gameManager.playersDeck.Count.ToString();
        MoveManager moveManager = GameObject.FindGameObjectWithTag("CardManager").GetComponent<MoveManager>();
        moveManager.cardPlaced =true;
    }

    public void DrawCard()
    {   
        if(cardArea.childCount >= 5)
        {
            return;
        }
        //dos veces
        for(int i = 0; i < 2; i++)
        {
            if(gameManager.playersDeck.Count > 0)
            {
                int card= gameManager.playersDeck[0];
                gameManager.playersHand.Add(card);
                gameManager.playersDeck.Remove(card);
                GameObject newCard = Instantiate(cardPrefab, cardArea);
                CardPropertiesDrag cardProperties = newCard.GetComponent<CardPropertiesDrag>();
                cardProperties.card = gameManager.cards[card].DeepCopy();
                cardProperties.AssignInfo();
                cardsRemaining.text = gameManager.playersDeck.Count.ToString();
            }
        }
    }

    public void AddKhronos()
    {
        khronos += 4;
        khronosQuantity.text = khronos.ToString();
    }

    public void EnergyWaste(int cost)
    {
        if(khronos >= cost)
        {
            khronos -= cost;
            khronosQuantity.text = khronos.ToString();
        }
    }
}
