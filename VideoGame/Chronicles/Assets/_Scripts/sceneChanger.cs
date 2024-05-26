/* 
by: julian ramirez A01027742.
18 de mayo 

This script help us to change the scene from to main menu, to access the game.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private GameObject welcomeMessage;
    public void ChangeToMenuScene()
    {
        welcomeMessage.SetActive(true);
        StartCoroutine(ExecuteAfterTime(3f));
        GoToScene("GameScene");
    }

    public void LetsPlay()
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        if(gameManager.playersDeck.Count > 6)
        {
            GoToScene("GameScene");
        }
    }

    private IEnumerator ExecuteAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public void GoToScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
