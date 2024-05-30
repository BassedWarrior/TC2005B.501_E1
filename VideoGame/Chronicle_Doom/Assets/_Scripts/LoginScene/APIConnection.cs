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

    void Start()
    {
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

    IEnumerator RequestGet(string url)
    {
        // Prepare the request object
        using(UnityWebRequest www = UnityWebRequest.Get(url)) {
            // Make the request and wait for it to respond
            yield return www.SendWebRequest();

            // Validate the response
            if(www.result != UnityWebRequest.Result.Success) {
                Debug.Log("Request failed: " + www.error);
            } else {
                string result = www.downloadHandler.text;
                //Debug.Log("The response was: " + result);
                CardData[] cardDataArray = JsonUtility.FromJson<CardDataArrayWrapper>(result).cards;
                gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
                foreach(CardData cardData in cardDataArray) {
                    UpdateCards(cardData);
                }
            }
        }
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
