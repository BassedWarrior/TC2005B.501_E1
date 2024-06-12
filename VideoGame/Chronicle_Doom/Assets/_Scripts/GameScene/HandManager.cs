using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandManager : MonoBehaviour
{
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
        cardsRemaining.text = GameManager.Instance.playersDeck.Count.ToString();
        khronosQuantity.text = khronos.ToString();
        GameManager.Instance.ShuffleDeck();
        ShowInitialHand();
    }

    private void ShowInitialHand()
    {
        initialScene.SetActive(true);
        for (int i = 0; i < selectiveCards; i++)
        {
            if (i < GameManager.Instance.playersDeck.Count)
            {
                CardData card = GameManager.Instance.cards[GameManager.Instance.playersDeck[i]];
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
            GameManager.Instance.playersHand.Add(decision);
            GameManager.Instance.playersDeck.Remove(decision);
        }
        selectedCards.Clear();
        Destroy(initialScene);

        CreateHand();
    }

    private void CreateHand()
    {
        foreach (int index in GameManager.Instance.playersHand)
        {
            GameObject newCard = Instantiate(cardPrefab, cardArea);
            CardPropertiesDrag cardProperties = newCard.GetComponent<CardPropertiesDrag>();
            cardProperties.card= GameManager.Instance.cards[index - 1].DeepCopy();
            cardProperties.AssignInfo();
        }
        if( GameManager.Instance.playersHand.Count < 5)
        {
            int faltantes= (5-GameManager.Instance.playersHand.Count);
            for(int i=0;  i<faltantes; i++)
            {
                GameObject newCard = Instantiate(cardPrefab, cardArea);
                CardPropertiesDrag cardProperties = newCard.GetComponent<CardPropertiesDrag>();
                if(cardProperties != null)
                {
                    int card= GameManager.Instance.playersDeck[i];
                    cardProperties.card= GameManager.Instance.cards[card].DeepCopy();
                    cardProperties.AssignInfo();
                    GameManager.Instance.playersHand.Add(card);
                    GameManager.Instance.playersDeck.Remove(card);
                }
            }
        }
        cardsRemaining.text = GameManager.Instance.playersDeck.Count.ToString();
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
            if(GameManager.Instance.playersDeck.Count > 0)
            {
                int card= GameManager.Instance.playersDeck[0];
                GameManager.Instance.playersHand.Add(card);
                GameManager.Instance.playersDeck.Remove(card);
                GameObject newCard = Instantiate(cardPrefab, cardArea);
                CardPropertiesDrag cardProperties = newCard.GetComponent<CardPropertiesDrag>();
                cardProperties.card = GameManager.Instance.cards[card].DeepCopy();
                cardProperties.AssignInfo();
                cardsRemaining.text = GameManager.Instance.playersDeck.Count.ToString();
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

    public int GetKronos()
    {
        return khronos;
    }
}
