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

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        waveNumber = 1;
        UpdateHealthBar();
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

        if (gameManager.playerHealth <= 0)
        {
            DisplayEndMessage("You Lose, GAMEOVER!");
            return;
        }

        if (waveNumber > 10)
        {
            DisplayEndMessage("There is no more waves, you win!");
            return;
        }

        clashTime.RelocateEnemies();
        StartNextTurn();
    }

    private void StartNextTurn()
    {
        waveNumber++;
        ObtainWave();
        StartCoroutine(HideWavePanel());
        MakeWaveAppear();
        handManager.DrawCard();
        handManager.AddKhronos();
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
        }
    }

    public void ObtainWave()
    {
        StartCoroutine(CallWave());
    }

    IEnumerator CallWave()
    {
        Debug.Log("CallWave: " + waveNumber);
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
                Debug.Log("The response was: " + result);

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
    }

    private void UpdateHealthBar()
    {
        playersHealthText.text = "Player Health: " + gameManager.playerHealth;
        healthBar.fillAmount = (float)gameManager.playerHealth / 20;
    }
}
