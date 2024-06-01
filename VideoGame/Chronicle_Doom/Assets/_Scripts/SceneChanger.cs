/*
by: Julian Ramirez A01027742.
18 de mayo 

This script helps us to change the scene from the main menu, to access the game.
*/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private GameObject welcomeMessage;
    [SerializeField] private GameObject deckMessage;

    public void ChangeToMenuScene()
    {
        welcomeMessage.SetActive(true);
        StartCoroutine(ExecuteAfterTime(2f, () => {
            GoToScene("CollectionScene");
        }));
    }

    public void LetsPlay()
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        if (gameManager.playersDeck.Count == 18)
        {
            GoToScene("GameScene");
        }
        else
        {
            deckMessage.SetActive(true);
            StartCoroutine(ExecuteAfterTime(2f, () => deckMessage.SetActive(false)));
        }
    }

    private IEnumerator ExecuteAfterTime(float seconds, System.Action afterWaitAction)
    {
        yield return new WaitForSeconds(seconds);
        afterWaitAction?.Invoke();
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
