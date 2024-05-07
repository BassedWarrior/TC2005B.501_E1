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
    [SerializeField] private List<GameObject> buttons;
    [SerializeField] private GameObject initialScene;
    private int selectiveCards;
    private List<int> selectedCards = new List<int>();
    void Start()        
    {
        selectiveCards= buttons.Count;
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        gameManager.ShuffleDeck();
        Debug.Log("Deck shuffled");
        ShowInitialHand();
    }

    private void ShowInitialHand()
    {
        initialScene.SetActive(true);
        for (int i = 0; i < selectiveCards; i++)
        {
            if (i < gameManager.playersDeck.Count)
            {
                CardCreator card = gameManager.cards[gameManager.playersDeck[i]];
                GameObject buttonObj = buttons[i];
                Image buttonImage = buttonObj.GetComponent<Image>();
                buttonImage.sprite = card.artwork;
                int index= i;
                buttonObj.GetComponent<Button>().onClick.AddListener(() => ToggleSelection(card.ID, index));
            }
        }
    }

    private void ToggleSelection(int ID, int buttonIndex)
    {
        if (selectedCards.Contains(ID))
        {
            selectedCards.Remove(ID);
            buttons[buttonIndex].GetComponent<Image>().color = Color.white; 
        }
        else
        {
            selectedCards.Add(ID);
            buttons[buttonIndex].GetComponent<Image>().color = Color.green; 
            Debug.Log("Selected Cards: " + string.Join(", ", selectedCards));
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
            for(int i=0;  i<(5- gameManager.playersHand.Count); i++)
            {
                GameObject newCard = Instantiate(cardPrefab, cardArea);
            }
        }
    }
}
