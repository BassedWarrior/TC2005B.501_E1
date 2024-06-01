
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [SerializeField] string url = "http://localhost:3000";
    [SerializeField] string getEnemyWaveEndPoint = "/enemy/wave/";
    [SerializeField] private HandManager handManager;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject wavePanel;
    [SerializeField] private Transform cardArea;
    [SerializeField] private TextMeshProUGUI cardsRemaining;
    [SerializeField] private Transform enemyAreaA;
    [SerializeField] private Transform enemyAreaB;
    [SerializeField] private Transform enemyAreaC;
    [SerializeField] private Transform enemySpawner;
    [SerializeField] private Transform selectiveArea;

    private int waveNumber;
    private GameManager gameManager;
    public List<CardData> cards = new List<CardData>();

    void Start()
    {
        waveNumber = 1;
        gameManager = GameManager.Instance; // Asegúrate de que GameManager esté inicializado
        GetCards();
        RelocateEnemies();
        waveNumber++;
        CreateWave();
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

    public void NextWave()
    {
        RelocateEnemies();
        handManager.DrawCard();
        handManager.AddKhronos();
        StartCoroutine(HideWavePanel());
        GetCards();
    }

    public void RelocateEnemies()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in enemySpawner)
        {
            if (child != enemySpawner)
            {
                children.Add(child);
            }
        }

        foreach (Transform child in children)
        {
            Debug.Log("Relocating enemies");
            if (child.CompareTag("Enemy"))
            {
                CardPropertiesDrag card = child.GetComponent<CardPropertiesDrag>();
                if (card != null)
                {
                    int randomLine = Random.Range(0, 3);
                    switch (randomLine)
                    {
                        case 0:
                            card.transform.SetParent(enemyAreaA);
                            break;
                        case 1:
                            card.transform.SetParent(enemyAreaB);
                            break;
                        case 2:
                            card.transform.SetParent(enemyAreaC);
                            break;
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

    private void CreateWave()
    {
        foreach (int index in gameManager.enemyWavesCards)
        {
            GameObject newCard = Instantiate(cardPrefab, cardArea);
            CardPropertiesDrag cardProperties = newCard.GetComponent<CardPropertiesDrag>();
            cardProperties.card = gameManager.cards[index];
            cardProperties.AssignInfo();
        }
    }

    void UpdateCards(CardData cardData)
    {
        gameManager.cards.Add(cardData); // Agrega el CardData a la lista de cartas en GameManager
        gameManager.enemyWavesCards.Add(cardData.cardID); // Agrega el ID de la carta a la lista enemyWavesCards
    }

    [System.Serializable]
    public class CardData
    {
        public int cardID;
    }

    [System.Serializable]
    public class CardDataArrayWrapper
    {
        public CardData[] cards;
    }
}
