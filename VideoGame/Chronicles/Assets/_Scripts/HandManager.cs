using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform selectiveArea;
    [SerializeField] private GameObject selectiveCard;
    [SerializeField] private Transform cardArea;
    [SerializeField] private List<GameObject> buttons;
    [SerializeField] private GameObject initialScene;
    private int selectiveCards = 5;
    private List<int> selectedCards = new List<int>();
    private List<bool> activeButtons = new List<bool>();

    void Start()        
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
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
            Debug.Log("Selected Cards: " + string.Join(", ", selectedCards));
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
            cardProperties.card= gameManager.cards[index];
            cardProperties.AssignInfo();
        }
        if( gameManager.playersHand.Count < 5)
        {
            Debug.Log("Cartas Ahora: " + gameManager.playersHand.Count);
            Debug.Log("Cartas Faltantes: " + (5- gameManager.playersHand.Count));
            int faltantes= (5-gameManager.playersHand.Count);
            for(int i=0;  i<faltantes; i++)
            {
                GameObject newCard = Instantiate(cardPrefab, cardArea);
                CardPropertiesDrag cardProperties = newCard.GetComponent<CardPropertiesDrag>();
                if(cardProperties != null)
                {
                    int card= gameManager.playersDeck[i];
                    cardProperties.card= gameManager.cards[card];
                    cardProperties.AssignInfo();
                    gameManager.playersHand.Add(card);
                    gameManager.playersDeck.Remove(card);
                }
            }
        }
    }

    public void DrawCard()
    {
        if(gameManager.playersDeck.Count > 0)
        {
            int card= gameManager.playersDeck[0];
            gameManager.playersHand.Add(card);
            gameManager.playersDeck.Remove(card);
            GameObject newCard = Instantiate(cardPrefab, cardArea);
            CardPropertiesDrag cardProperties = newCard.GetComponent<CardPropertiesDrag>();
            cardProperties.card= gameManager.cards[card];
            cardProperties.AssignInfo();
        }
    }
}
