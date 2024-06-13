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
    public string musicVolumeParameter;
    public string sfxVolumeParameter;
    [SerializeField] private float maxDecibels;
    [SerializeField] private float minDecibels;
    //EFECTO DE SONIDO
    [SerializeField] public AudioClip cardSound1;
    [SerializeField] public AudioClip cardSound2;
    [SerializeField] public AudioClip cardSound3;
    [SerializeField] public AudioClip cardSound4;
    [SerializeField] public AudioClip cardSound5;
    [SerializeField] public AudioClip cardSound6;
    public bool turnFinished = false;

    public void Start()
    {
        api = GetComponent<APIConnection>();
        //OBTENER LOS ULTIMOS VALORES DE VOLUMEN DE MUSICA Y SFX
        //SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.0f));
        //SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0.0f));
        
        //REINICIAR EL VOLUMEN DE MUSICA Y SFX AL INICIAR PARTIDA
        PlayerPrefs.SetFloat("MusicVolume", maxDecibels);
        PlayerPrefs.SetFloat("SFXVolume", maxDecibels);
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
        ResetGame();
    }

    public List<GameScore> GetTopHighscores()
    {
        return gameScores;
    }

    // AUDIO MANAGER FUNCTIONS
    public void SetMusicVolume(float normalizedVolume)
    {
        float decibelVolume = ConvertToDecibels(normalizedVolume);
        audioMixer.SetFloat(musicVolumeParameter, decibelVolume);
        PlayerPrefs.SetFloat("MusicVolume", normalizedVolume);
    }

    public void SetSFXVolume(float normalizedVolume)
    {
        float decibelVolume = ConvertToDecibels(normalizedVolume);
        audioMixer.SetFloat(sfxVolumeParameter, decibelVolume);
        PlayerPrefs.SetFloat("SFXVolume", normalizedVolume);
    }

    public float GetVolume(string parameter)
    {
        float decibelValue;
        audioMixer.GetFloat(parameter, out decibelValue);
        return ConvertToNormalized(decibelValue);
    }

    private float ConvertToDecibels(float normalizedValue)
    {
        return minDecibels + (maxDecibels - minDecibels) * normalizedValue;
    }

    private float ConvertToNormalized(float decibelValue)
    {
        return (decibelValue - minDecibels) / (maxDecibels - minDecibels);
    }

    //FORMULA PARA CALCULAR EL VOLUMEN DE SONIDO
    // VOLUME = Mathf.Log10(volume) * 20
    public void ResetGame()
    {
        this.score = 0;
        this.playerHealth = 20;
        this.playerDamage = 0;
        this.turnFinished = false;
        this.playersDeck.Clear();
        this.playersHand.Clear();
        this.gameScores.Clear();
        api.GetTopHighscores();
        api.GetCards();
        api.GetUsersDeck();
    }
}
