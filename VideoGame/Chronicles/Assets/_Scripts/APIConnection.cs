using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIConnection : MonoBehaviour
{
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
                // Start the process to create the simon buttons
                // TODO: Add controller
                // controller.apiData = result;
                // controller.PrepareButtons();
            }
        }
    }
}
