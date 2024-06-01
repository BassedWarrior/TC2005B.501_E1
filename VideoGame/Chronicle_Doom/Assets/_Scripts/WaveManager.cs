using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class WaveManager : MonoBehaviour
{
    [SerializeField] string url = "http://localhost:3000";
    [SerializeField] string getEnemyWaveEndPoint = "/enemy/wave/";

    private GameManager gameManager;
    [SerializeField] private HandManager handManager;
    //[SerializeField] private ClashTime clashTime;
    [SerializeField] private GameObject wavePanel;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardArea;
    [SerializeField] private TextMeshProUGUI waveText;
    //[SerializeField] private List<EnemyWave> enemyWaves;
    [SerializeField] private Transform enemyArea;
    [SerializeField] private Transform enemyAreaA;
    [SerializeField] private Transform enemyAreaB;
    [SerializeField] private Transform enemyAreaC;
    
    
    public int waveNumber = 1;
    //private List<CardData> enemyWave;

    private void Start()
    {
        gameManager = GameManager.Instance;
        GetWaveEnemyDB(waveNumber);
        waveNumber++;
        DisplayEnemyCards();
         // Obtener referencia al GameManager
    }

    public void NextWave()
    {
        gameManager.ClearEnemyWavesCards();
        GetWaveEnemyDB(waveNumber);
        waveNumber++;
        //clashTime.RelocateEnemies();
        handManager.DrawCard();
        handManager.AddKhronos();
        StartCoroutine(HideWavePanel());

    }

    public void GetWaveEnemyDB(int waveID)
    {
        StartCoroutine(RequestGet(url + getEnemyWaveEndPoint + waveID));
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

                // Agregar cartas a enemyWavesCards en GameManager
                foreach (CardData cardData in cardDataArray)
                {
                    gameManager.enemyWavesCards.Add(cardData.cardID);
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

private void DisplayEnemyCards()
    {
        foreach (int index in gameManager.enemyWavesCards)
        {
            GameObject newCard = Instantiate(cardPrefab, cardArea);
            CardPropertiesDrag cardProperties = newCard.GetComponent<CardPropertiesDrag>();
            cardProperties.card= gameManager.cards[index];
            cardProperties.AssignInfo();
        }
    }



    [System.Serializable]
    private class CardDataArrayWrapper
    {
                public CardData[] cards;
    }
}
