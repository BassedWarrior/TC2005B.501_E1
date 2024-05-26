/* 
by: julian ramirez A01027742.
18 de mayo 

This script help us to change the scene from to main menu, 
to access the game.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public static void GoTo(string sceneName){
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);}
}
