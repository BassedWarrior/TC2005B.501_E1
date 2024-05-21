using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIConnection : MonoBehaviour
{
    [System.Serializable]
    public class CardData
    {
        public int ID;
        public string name;
        public string description;
        public string artworkPath;
        public int energyCost;
        public int health;
        public int attack;
    }
    [SerializeField] string url;
    [SerializeField] string getEndpoint;
    [SerializeField] string getCardsEndpoint;
    [SerializeField] string getUnitCardsEndpoint;
    [SerializeField] string getParadoxCardsEndpoint;

    // TODO: Add game controller
    // controller;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Add game controller
        // controller = ;
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
                Debug.Log("The response was: " + result);
                CardData[] cardDataArray = JsonUtility.FromJson<CardDataArrayWrapper>(result).cards;
                foreach(CardData cardData in cardDataArray) {
                    UpdateScriptableObject(cardData);
                }

                // Start the process to create the simon buttons
                // TODO: Add controller
                // controller.apiData = result;
                // controller.PrepareButtons();
            }
        }
    }
    void UpdateScriptableObject(CardData cardData)
    {
        CardCreator card = ScriptableObject.CreateInstance<CardCreator>();
        card.ID = cardData.ID;
        card.name = cardData.name;
        card.description = cardData.description;
        card.energyCost = cardData.energyCost;
        card.health = cardData.health;
        card.attack = cardData.attack;
        card.artwork = Resources.Load<Sprite>("alien");
        
        string relativeOutputPath = "Assets/PreFabs/ScriptableObjects";
        string outputFilePath = relativeOutputPath + "/" + card.name + ".asset";

        UnityEditor.AssetDatabase.CreateAsset(card, outputFilePath);
    }

    [System.Serializable]
    private class CardDataArrayWrapper
    {
        public CardData[] cards;
    }
}
