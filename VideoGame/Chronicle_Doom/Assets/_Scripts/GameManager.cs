using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

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
    public List<GameObject> textDots = new List<GameObject>();
    // Player information
    public int playerHealth = 20;
    public int playerDamage = 0;
    // Highscores
    public List<GameScore> gameScores;
    // Score information
    public int score = 0;
    [SerializeField] public TextMeshProUGUI scoreText;
    // AUDIO MANAGER
    public AudioMixer audioMixer;
    public string masterVolumeParameter;
    public string musicVolumeParameter;
    public string sfxVolumeParameter;

    public void Start()
    {
        api = GetComponent<APIConnection>();
        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 0.5f));
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.5f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0.5f));
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

    public void DeleteDots()
    {
        foreach (GameObject dot in textDots)
        {
            Destroy(dot);
        }
        textDots.Clear();
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
    
    public void ResetPlayerDamage()
    {
        this.playerDamage = 0;
    }

    public void AddPlayerDamage(int damage)
    {
        this.playerDamage += damage;
    }

    public void ApplyPlayerDamage()
    {
        this.playerHealth -= this.playerDamage;
        this.playerDamage = 0;
    }

    public void AddKillScore(int killScore)
    {
        this.score += killScore;
    }

    public void AddWaveScore(int wave)
    {
        this.score += 100 * wave;
    }

    public void UpdateScoreText()
    {
        this.scoreText.text = $"Score: {this.score}";
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

    // AUDIO MANAGER FUNCTIONS
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat(masterVolumeParameter, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(musicVolumeParameter, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(sfxVolumeParameter, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public float GetVolume(string parameter)
    {
        float value;
        audioMixer.GetFloat(parameter, out value);
        return Mathf.Pow(10, value / 20);
    }
}
