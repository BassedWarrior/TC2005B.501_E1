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

    public void NextWave()
    {
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
