using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class EnemyWave
{
    public string waveName;
    public List<CardData> enemies;
}

public class WaveManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private HandManager handManager;
    [SerializeField] private ClashTime clashTime;
    private int waveNumber;
    [SerializeField] private GameObject wavePanel;
    [SerializeField] private TextMeshProUGUI waveText;
    private List<CardData> enemyWave;
    [SerializeField] private List<EnemyWave> enemyWaves;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemyArea;
    [SerializeField] private TextMeshProUGUI playersHealthText;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        waveNumber = 0;
        playersHealthText.text = "Player Health: " + gameManager.playerHealth;
    }
    public void NextWave()
    {
        if (waveNumber > enemyWaves.Count)
        {
            waveText.text = "There is no more waves, you win!";
            wavePanel.SetActive(true);
            return;
        }
        if (gameManager.playerHealth <= 0)
        {
            waveText.text = "You Lose, GAMEOVER!";
            wavePanel.SetActive(true);
            return;
        }
        playersHealthText.text = "Player Health: " + gameManager.playerHealth;
        clashTime.RelocateEnemies();
        SetWave(waveNumber);
        waveNumber++;
        handManager.DrawCard();
        handManager.AddKhronos();
        StartCoroutine(HideWavePanel());
    }
    private IEnumerator HideWavePanel()
    {
        waveText.text = "Turn Wave " + (waveNumber-1);
        wavePanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        wavePanel.SetActive(false);
    }

    private void SetWave(int waveIndex)
    {
        enemyWave = enemyWaves[waveIndex].enemies;
        foreach (CardData card in enemyWave)
        {
            GameObject newCard= Instantiate(enemyPrefab, enemyArea);
            newCard.tag= "Enemy";
            CardPropertiesDrag cardProperties = newCard.GetComponent<CardPropertiesDrag>();
            cardProperties.card= card;
            cardProperties.AssignInfo();
        }
    }
    public void FirstWave()
    {
        SetWave(waveNumber);
        clashTime.RelocateEnemies();
        waveNumber++;
        SetWave(waveNumber);
        waveNumber++;
        StartCoroutine(HideWavePanel());
        handManager.AddKhronos();
    }
}
