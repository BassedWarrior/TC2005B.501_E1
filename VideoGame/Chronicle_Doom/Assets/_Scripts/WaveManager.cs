using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;




[System.Serializable]
public class EnemyWave
{
    public string waveName;
    public List<CardData> enemies;
}

public class WaveManager : MonoBehaviour
{

    [SerializeField] string url = "http://localhost:3000";
    [SerializeField] string getEnemyWaveEndPoint = "/enemy/wave/";
    private GameManager gameManager;
    [SerializeField] private HandManager handManager;
    [SerializeField] private ClashTime clashTime;
    private int waveNumber;
    [SerializeField] private GameObject wavePanel;
    [SerializeField] private TextMeshProUGUI waveText;
    private List<CardData> enemyWave;
    [SerializeField] private List<EnemyWave> enemyWaves;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemyArea;
    [SerializeField] private Transform enemyAreaA;
    [SerializeField] private Transform enemyAreaB;
    [SerializeField] private Transform enemyAreaC;

    public void NextWave()
    {
        clashTime.RelocateEnemies();
        SetWave(waveNumber);
        waveNumber++;
        handManager.DrawCard();
        handManager.AddKhronos();
        StartCoroutine(HideWavePanel());
    }
    private IEnumerator HideWavePanel()
    {
        waveText.text = "Turn Wave " + (waveNumber-1);
        wavePanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        wavePanel.SetActive(false);
    }


public void GetCards()
    {
        StartCoroutine(RequestGet(url + getEnemyWaveEndPoint + waveNumber));
    }

    IEnumerator RequestGet(string url)
    {
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

                // Deserializa el JSON en un array de CardData
                CardDataArrayWrapper cardDataWrapper = JsonUtility.FromJson<CardDataArrayWrapper>(result);
                CardData[] cardDataArray = cardDataWrapper.cards;

                // Itera sobre los datos de las tarjetas y agrégalas a la lista cards
                foreach (CardData cardData in cardDataArray)
                {
                    cards.Add(cardData); // Actualiza la lista de cards
                    UpdateCards(cardData);
                }
            }
        }
    }

    private void SetWave(int waveIndex)
    {
        enemyWave = enemyWaves[waveIndex].enemies;
        foreach (CardData card in enemyWave)
        {
            GameObject newCard= Instantiate(enemyPrefab, enemyArea);
            newCard.tag= "Enemy";
            CardPropertiesDrag cardProperties = newCard.GetComponent<CardPropertiesDrag>();
            cardProperties.card= card;
            cardProperties.AssignInfo();
        }
    }
    public void FirstWave()
    {
        SetWave(waveNumber);
        clashTime.RelocateEnemies();
        waveNumber++;
        SetWave(waveNumber);
        waveNumber++;
        StartCoroutine(HideWavePanel());
        handManager.AddKhronos();
    }
    
}