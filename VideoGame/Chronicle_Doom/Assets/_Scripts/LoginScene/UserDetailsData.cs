using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using TMPro;

public class UserDetailsData : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button signInButton;
    [SerializeField] private Button signUpButton;
    [SerializeField] private string url;
    [SerializeField] private string signInEndpoint;
    [SerializeField] private string signUpEndpoint;
    [SerializeField] private GameObject errorMessage;
    [SerializeField] private TextMeshProUGUI errorMessageText;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button configButton;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Button exitSettingsButton;
    [SerializeField] private Button exitSettingsButtonBack;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        signInButton.onClick.AddListener(OnLoginButtonClicked);
        signUpButton.onClick.AddListener(OnSignUpButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        OnConfigButtonClicked(false);
        configButton.onClick.AddListener(() => OnConfigButtonClicked(true));
        exitSettingsButton.onClick.AddListener(() => OnConfigButtonClicked(false));
        exitSettingsButtonBack.onClick.AddListener(() => OnConfigButtonClicked(false));
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
                PlayerPrefs.SetString("username", username);
                gameManager.GetComponent<APIConnection>().GetUsersDeck();
                gameManager.GetComponent<SceneChanger>().ChangeToMenuScene();
                
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

    private void OnExitButtonClicked()
    {
        Application.Quit();
        Debug.Log("JUEGO CERRAO");
    }

    private void OnConfigButtonClicked(bool activate)
    {
        settingsMenu.SetActive(activate);
    }
}
