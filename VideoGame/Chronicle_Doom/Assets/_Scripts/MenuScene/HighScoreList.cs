using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScoreList : MonoBehaviour
{
    [SerializeField] public GameManager gameManager;
    [SerializeField] public List<GameScore> highscores;
    [SerializeField] public GameObject textMeshPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        highscores = gameManager.GetTopHighscores();
        for (int i = 1; i < highscores.Count; i++)
        {
            GameObject score = Instantiate(textMeshPrefab, gameObject.transform);
            score.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{i}";
            score.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{highscores[i].username}";
            score.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"{highscores[i].score}";
        }
    }
}
