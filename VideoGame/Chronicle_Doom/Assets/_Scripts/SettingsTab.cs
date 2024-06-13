using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsTab : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button configButton;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Button exitSettingsButton;
    [SerializeField] private Button exitSettingsButtonBack;
    [SerializeField] private string sceneName;

    void Start()
    {
        exitButton.onClick.AddListener(() => GameManager.Instance.GetComponent<SceneChanger>().GoToScene(sceneName));
        OnConfigButtonClicked(false);
        configButton.onClick.AddListener(() => OnConfigButtonClicked(true));
        exitSettingsButton.onClick.AddListener(() => OnConfigButtonClicked(false));
        exitSettingsButtonBack.onClick.AddListener(() => OnConfigButtonClicked(false));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnConfigButtonClicked(false);
        }   
    }

    private void OnConfigButtonClicked(bool activate)
    {
        settingsMenu.SetActive(activate);
    }
}
