using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIConnection : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] string url;
    [SerializeField] string getEndpoint;
    [SerializeField] string getCardsEndpoint;
    [SerializeField] string getUnitCardsEndpoint;
    [SerializeField] string getParadoxCardsEndpoint;
    [SerializeField] string getUsersDeck;
    [SerializeField] string updateUsersDeck;
    [SerializeField] string postGame;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        GetCards();
    }

    public void GetCards()
    {
        StartCoroutine(RequestGet(url + getCardsEndpoint));
    }

    public void GetUnitCards()
    {
        StartCoroutine(RequestGet(url + getUnitCardsEndpoint));
    }

    public void GetParadoxCards()
    {
        StartCoroutine(RequestGet(url + getParadoxCardsEndpoint));
    }

    public void GetUsersDeck()
    {
        StartCoroutine(SetUsersDeck(url + getUsersDeck + "/" + PlayerPrefs.GetString("username")));
    }

    public void UpdateUsersDeck()
    {
        StartCoroutine(ChangesInDeck(url + updateUsersDeck));
    }

    IEnumerator RequestGet(string url)
    {
        // Prepare the request object
        using(UnityWebRequest www = UnityWebRequest.Get(url))
        {
            // Make the request and wait for it to respond
            yield return www.SendWebRequest();

            // Validate the response
            if(www.result != UnityWebRequest.Result.Success) {
                Debug.Log("Request failed: " + www.error);
            } else {
                string result = www.downloadHandler.text;
                //Debug.Log("The response was: " + result);
                CardData[] cardDataArray = JsonUtility.FromJson<CardDataArrayWrapper>(result).cards;
                foreach(CardData cardData in cardDataArray) {
                    UpdateCards(cardData);
                }
            }
        }
    }

    IEnumerator RequestPost(string url, string json)
    {
        // Prepare the request object
        using(UnityWebRequest www = UnityWebRequest.Post(
                url, json, "application/json"))
        {
            // Make the request and wait for it to respond
            yield return www.SendWebRequest();

            // Validate the response
            if(www.result != UnityWebRequest.Result.Success) {
                Debug.Log("Request failed: " + www.error);
            } else {
                string result = www.downloadHandler.text;
                Debug.Log("The response was: " + result);
            }
        }
    }

    IEnumerator SetUsersDeck(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url)) {
            yield return www.SendWebRequest();

            if(www.result != UnityWebRequest.Result.Success) {
                Debug.Log("Request failed: " + www.error);
            } else {
                string result = www.downloadHandler.text;
                Debug.Log("The response was: " + result);
                
                DeckCard[] deckCards = JsonUtility.FromJson<DeckCardArrayWrapper>("{\"deckCards\":" + result + "}").deckCards;
                gameManager.playersDeck.Clear();
                foreach (var card in deckCards)
                {
                    for (int j = 0; j < card.card_times; j++)
                    {
                        gameManager.playersDeck.Add(card.cardID-1);
                    }
                }
            }
        }
    }

    IEnumerator ChangesInDeck(string url)
    {
        List <DeckCard> deckCards = new List<DeckCard>();
        foreach (var card in gameManager.playersDeck)
        {
            if (deckCards.Exists(x => x.cardID == card))
            {
                deckCards.Find(x => x.cardID == card).card_times++;
            }
            else
            {
                deckCards.Add(new DeckCard { cardID = card, card_times = 1 });
            }
        }
        //string jsonData = JsonUtility.ToJson(deckCards);
        string jsonData = ConvertToCompleteJson(PlayerPrefs.GetString("username"), PlayerPrefs.GetString("username") + "'s deck", deckCards);
        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Request failed: " + www.error);
            }
            else
            {
                Debug.Log("Request success: " + www.downloadHandler.text);
            }
        }

    }

    void UpdateCards(CardData cardData)
    {
        gameManager.cards.Add(cardData);
    }

    public string ConvertToCompleteJson(string username, string deckName, List<DeckCard> deckCards)
    {
        // Perdón gente voy a hacer el JSON a mano ;(
        string jsonData = "{";

        jsonData += "\"username\": \"" + username + "\",";
        jsonData += "\"deck_name\": \"" + deckName + "\",";
        jsonData += "\"card_JSON\": [";

        for (int i = 0; i < deckCards.Count; i++)
        {
            jsonData += "{";
            jsonData += "\"cardID\": " + (deckCards[i].cardID+1) + ",";
            jsonData += "\"card_times\": " + deckCards[i].card_times;
            jsonData += "}";
            if (i < deckCards.Count - 1)
            {
                jsonData += ",";
            }
        }

        jsonData += "]";
        jsonData += "}";
        return jsonData;
    }

    public void PostGame(int score, int gameRound, int kronos, int deckCards)
    {
        // Post game information to the API
        // Create JSON string with all game information
        string jsonData = "{"
            + $"\"score\": {score},"
            + $"\"gameRound\": {gameRound},"
            + $"\"kronos\": {kronos},"
            + $"\"deckCards\": {deckCards}"
        + "}";

        // Create a POST request with the JSON data
        StartCoroutine(RequestPost(
                url + postGame + "/" + PlayerPrefs.GetString("username"),
                jsonData));
    }
}
