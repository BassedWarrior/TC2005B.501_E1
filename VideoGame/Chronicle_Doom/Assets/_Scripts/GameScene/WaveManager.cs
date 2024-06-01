using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        waveNumber = 1;
        playersHealthText.text = "Player Health: " + gameManager.playerHealth;
        ObtainWave(waveNumber);
    }
    public void FirstWave()
    {
        MakeWaveAppear();
        clashTime.RelocateEnemies();
        MakeWaveAppear();
        StartCoroutine(HideWavePanel());
        handManager.AddKhronos();
    }
    public void NextWave()
    {
        playersHealthText.text = "Player Health: " + gameManager.playerHealth;
        if (waveNumber > 10)
        {
            waveText.text = "There is no more waves, you win!";
            wavePanel.SetActive(true);
            return;
        }
        if (gameManager.playerHealth <= 0)
        {
            waveText.text = "You Lose, GAMEOVER!";
            wavePanel.SetActive(true);
            return;
        }
        clashTime.RelocateEnemies();

        MakeWaveAppear();
        handManager.DrawCard();
        handManager.AddKhronos();
        StartCoroutine(HideWavePanel());
    }
    private void MakeWaveAppear()
    {
        Debug.Log("MakeWaveAppear");
        foreach (CardData card in enemyWave)
        {
            GameObject newCard= Instantiate(enemyPrefab, enemyArea);
            newCard.tag= "Enemy";
            CardPropertiesDrag cardProperties = newCard.GetComponent<CardPropertiesDrag>();
            cardProperties.card= card.DeepCopy();
            cardProperties.AssignInfo();
        }
        waveNumber++;
        ObtainWave(waveNumber);
    }
    public void ObtainWave(int waveNumber)
    {
        StartCoroutine(CallWave(waveNumber));
    }
    IEnumerator CallWave(int waveNumber)
    {
        string url= "http://localhost:3000/enemy/wave/" + waveNumber;
        using (UnityWebRequest www= UnityWebRequest.Get(url)){
            yield return www.SendWebRequest();

            if(www.result != UnityWebRequest.Result.Success){
                Debug.Log("Request failed: " + www.error);
            } else {
                string result = www.downloadHandler.text;
                Debug.Log("The response was: " + result);

                DeckCard[] deckCards = JsonUtility.FromJson<DeckCardArrayWrapper>("{\"deckCards\":" + result + "}").deckCards;
                enemyWave.Clear();
                foreach (var card in deckCards)
                {
                    for (int j = 0; j < card.card_times; j++)
                    {
                        enemyWave.Add(gameManager.cards[card.cardID-1]);
                    }
                }
            }
        }
    }
    private IEnumerator HideWavePanel()
    {
        waveText.text = "Turn Wave " + (waveNumber-1);
        wavePanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        wavePanel.SetActive(false);
    }
}
