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
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button signInButton;
    [SerializeField] private Button signUpButton;
    [SerializeField] private string url;
    [SerializeField] private string signInEndpoint;
    [SerializeField] private string signUpEndpoint;
    [SerializeField] private GameObject errorMessage;
    [SerializeField] private TextMeshProUGUI errorMessageText;
    [SerializeField] private SceneChanger sceneChanger;

    private void Start()
    {
        signInButton.onClick.AddListener(OnLoginButtonClicked);
        signUpButton.onClick.AddListener(OnSignUpButtonClicked);
    }

    private void OnLoginButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        StartCoroutine(SendRequest(username, password, url + signInEndpoint));
    }

    private void OnSignUpButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        Debug.Log("Username: " + username + " Password: " + password);
        Debug.Log("URL: " + url + signUpEndpoint);
        StartCoroutine(SendRequest(username, password, url + signUpEndpoint));
    }

    private IEnumerator SendRequest(string username, string password, string url)
    {
        LoginData loginData = new LoginData(username, password);
        string jsonData = JsonUtility.ToJson(loginData);
        
        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                StartCoroutine(ShowErrorMessage(www.downloadHandler.text));
            }
            else
            {
                Debug.Log("Respuesta del servidor: " + www.downloadHandler.text);
                sceneChanger.ChangeToMenuScene();
            }
        }
    }
    private IEnumerator ShowErrorMessage(string message)
    {
        errorMessageText.text = message;
        errorMessage.SetActive(true);
        yield return new WaitForSeconds(2f);
        errorMessage.SetActive(false);
    }
}