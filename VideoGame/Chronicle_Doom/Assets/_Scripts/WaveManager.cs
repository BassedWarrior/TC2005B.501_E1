using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class WaveManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] string url = "http://localhost:3000";
    [SerializeField] string getEnemyWaveEndPoint = "/enemy/wave/";
    [SerializeField] private ClashTime clashTime;
    [SerializeField] private HandManager handManager;
    [SerializeField] private TextMeshProUGUI waveText;
    private int waveNumber;
    [SerializeField] private GameObject wavePanel;

    void Start()
    {
        waveNumber = 1;
        GetCards();
        clashTime.RelocateEnemies();
        waveNumber++;
    }

    public void GetCards()
    {
        StartCoroutine(RequestGet(url + getEnemyWaveEndPoint + waveNumber));
    }

    public void NextWave()
    {
        clashTime.RelocateEnemies();
        handManager.DrawCard();
        handManager.AddKhronos();
        StartCoroutine(HideWavePanel());
        GetCards();
    }

    IEnumerator RequestGet(string url)
    {
        using(UnityWebRequest www = UnityWebRequest.Get(url)) 
        {
            yield return www.SendWebRequest();

            if(www.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log("Request failed: " + www.error);
            } 
            else
            {
                string result = www.downloadHandler.text;
                Debug.Log("The response was: " + result);
                CardData[] cardDataArray = JsonUtility.FromJson<CardDataArrayWrapper>(result).cards;
                gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
                foreach(CardData cardData in cardDataArray) 
                {
                    UpdateCards(cardData);
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

    void UpdateCards(CardData cardData)
    {
        gameManager.cards.Add(cardData);
    }

    [System.Serializable]
    private class CardDataArrayWrapper
    {
        public CardData[] cards;
    }
}
