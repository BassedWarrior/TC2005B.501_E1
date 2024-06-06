using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class WaveManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private HandManager handManager;
    [SerializeField] private ClashTime clashTime;
    private int waveNumber;
    [SerializeField] private GameObject wavePanel;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private List<CardData> enemyWave;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemyArea;
    [SerializeField] private TextMeshProUGUI playersHealthText;
    [SerializeField] private Image healthBar;
    [SerializeField] private Button returnMenuButton;
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] public TextMeshProUGUI roundText;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        waveNumber = 1;
        UpdateHealthBar();
        UpdateScoreText();
        UpdateWaveText();
        ObtainWave();
    }

    public void FirstWave()
    {
        MakeWaveAppear();
        clashTime.RelocateEnemies();
        StartNextTurn();
    }

    public void NextWave()
    {
        UpdateHealthBar();
        UpdateScoreText();
        UpdateWaveText();

        if (gameManager.playerHealth <= 0)
        {
            DisplayEndMessage("You Lose, GAMEOVER!");
            gameManager.PostGame();
            return;
        }

        if (waveNumber > 10)
        {
            DisplayEndMessage("There is no more waves, you win!");
            gameManager.PostGame();
            return;
        }

        clashTime.RelocateEnemies();
        StartNextTurn();
    }

    private void StartNextTurn()
    {
        Debug.Log($"Adding {waveNumber * 100} score for round {waveNumber}!");
        gameManager.AddWaveScore(waveNumber);
        waveNumber++;
        ObtainWave();
        StartCoroutine(HideWavePanel());
        MakeWaveAppear();
        handManager.DrawCard();
        handManager.AddKhronos();
        gameManager.AddWaveScore(waveNumber);
    }

    private void MakeWaveAppear()
    {
        foreach (CardData card in enemyWave)
        {
            GameObject newCard = Instantiate(enemyPrefab, enemyArea);
            newCard.tag = "Enemy";
            CardPropertiesDrag cardProperties = newCard.GetComponent<CardPropertiesDrag>();
            cardProperties.card = card.DeepCopy();
            cardProperties.AssignInfo();
            cardProperties.card.CalculateScoreValue();
        }
    }

    public void ObtainWave()
    {
        StartCoroutine(CallWave());
    }

    IEnumerator CallWave()
    {
        string url = "http://localhost:3000/enemy/wave/" + waveNumber;
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Request failed: " + www.error);
            }
            else
            {
                string result = www.downloadHandler.text;
                //Debug.Log("The response was: " + result);

                DeckCard[] deckCards = JsonUtility.FromJson<DeckCardArrayWrapper>("{\"deckCards\":" + result + "}").deckCards;
                enemyWave.Clear();
                foreach (var card in deckCards)
                {
                    for (int j = 0; j < card.card_times; j++)
                    {
                        enemyWave.Add(gameManager.cards[card.cardID - 1]);
                    }
                }
            }
        }
    }

    private IEnumerator HideWavePanel()
    {
        waveText.text = "Turn Wave " + (waveNumber - 1);
        wavePanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        wavePanel.SetActive(false);
    }

    private void DisplayEndMessage(string message)
    {
        waveText.text = message;
        wavePanel.SetActive(true);
        returnMenuButton.gameObject.SetActive(true);
    }

    private void UpdateHealthBar()
    {
        playersHealthText.text = "Health: " + gameManager.playerHealth;
        healthBar.fillAmount = (float)gameManager.playerHealth / 20;
    }

    public void UpdateScoreText()
    {
        this.scoreText.text = $"Score: {this.gameManager.score}";
    }
    
    public void UpdateWaveText()
    {
        this.roundText.text = $"Wave: {this.waveNumber}";
    }
    
    public int GetWaveNumber()
    {
        return waveNumber;
    }
}
