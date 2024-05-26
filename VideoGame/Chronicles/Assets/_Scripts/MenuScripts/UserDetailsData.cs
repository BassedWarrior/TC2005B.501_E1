using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using TMPro;

[System.Serializable]
public class LoginData
{
    public string username;
    public string password;

    public LoginData(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}

public class UserDetailsData : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button loginButton;

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    private void OnLoginButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        StartCoroutine(SendLoginRequest(username, password));
    }

    private IEnumerator SendLoginRequest(string username, string password)
    {
        LoginData loginData = new LoginData(username, password);
        string jsonData = JsonUtility.ToJson(loginData);
        Debug.Log("Mandando JsonData: " + jsonData);

        using (UnityWebRequest www = new UnityWebRequest("https://tu-api-endpoint.com/login", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                Debug.Log("Respuesta del servidor: " + www.downloadHandler.text);
            }
        }
    }
}
