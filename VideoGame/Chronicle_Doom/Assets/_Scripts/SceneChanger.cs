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
    public void ChangeToMenuScene()
    {
        StartCoroutine(ExecuteAfterTime(2f, () => {
            GoToScene("CollectionScene");
        }));
        GameManager.Instance.ResetGame();
    }

    public void ChangeToLoginScene()
    {
        GoToScene("LoginScene");
    }

    public void ChangeToGameScene()
    {
        StartCoroutine(ExecuteAfterTime(0f, () => {
            GoToScene("GameScene");
        }));
    }

    private IEnumerator ExecuteAfterTime(float seconds, System.Action afterWaitAction)
    {
        yield return new WaitForSeconds(seconds);
        afterWaitAction?.Invoke();
    }

    public void GoToScene(string sceneName)
    {   
        if (sceneName == "Exit")
        {
            Application.Quit();
            Debug.Log("JUEGO CERRAO");
            return;
        }
        SceneManager.LoadScene(sceneName);
    }
}
