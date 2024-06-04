using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // API connection for queries and stuff
    public APIConnection api;
    // Game scene scripts
    public WaveManager waveManager;
    public ClashTime clashTime;
    public HandManager handManager;
    public MoveManager moveManager;
    public GameObject cardManager;
    // Instance of self
    public static GameManager Instance;
    // Game Cards
    public List<CardData> cards;
    // Game information
    public List<int> playersDeck = new List<int>();
    public List<int> playersHand = new List<int>();
    public int playerHealth = 20;
    public int score = 0;
    // Highscores
    public List<GameScore> gameScores;

    public void Start()
    {
        api = GetComponent<APIConnection>();
    }

    // Generate an instance of self that persists throughout scene changes
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

    // Sort the player deck
    public void SortDeck()
    {
        playersDeck.Sort();
    }

    // Shuffle the player deck
    public void ShuffleDeck()
    {
        System.Random rng = new System.Random();
        int n = playersDeck.Count;
        while (n > 1)
        {
            n--;
            int range = rng.Next(n + 1);
            int assess = playersDeck[range];
            playersDeck[range] = playersDeck[n];
            playersDeck[n] = assess;
        }
    }

    // Post game information to the API to be stored in the database
    public void PostGame()
    {
        cardManager = GameObject.FindGameObjectWithTag("CardManager");
        waveManager = cardManager.GetComponent<WaveManager>();
        clashTime = cardManager.GetComponent<ClashTime>();
        handManager = cardManager.GetComponent<HandManager>();
        moveManager = cardManager.GetComponent<MoveManager>();
        api.PostGame(score,
                     waveManager.GetWaveNumber(),
                     handManager.GetKronos(),
                     playersDeck.Count);
    }

    public List<GameScore> GetTopHighscores()
    {
        return gameScores;
    }
}
