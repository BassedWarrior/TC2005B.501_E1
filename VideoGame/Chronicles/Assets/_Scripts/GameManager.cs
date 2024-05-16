using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<CardCreator> cards;
    public List<int> playersDeck = new List<int>();
    public List<int> playersHand = new List<int>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);    
        }
    }
    public void SortDeck()
    {
        playersDeck.Sort();
    }
    public void ShuffleDeck()
    {
        System.Random rng = new System.Random();
        int n = playersDeck.Count;
        while (n > 1)
        {
            n--;
            int porto = rng.Next(n + 1);
            int value = playersDeck[porto];
            playersDeck[porto] = playersDeck[n];
            playersDeck[n] = value;
        }
    }
}
