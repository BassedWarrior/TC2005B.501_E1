using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class EnemyWave
{
    public string waveName;
    public List<CardCreator> enemies;
}

public class WaveManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private HandManager handManager;
    private int waveNumber;
    [SerializeField] private GameObject wavePanel;
    [SerializeField] private TextMeshProUGUI waveText;
    //enemy wave
    private List<CardCreator> enemyWave;
    [SerializeField] private List<EnemyWave> enemyWaves;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemyArea;

    public void StartWave()
    {
        SetWave(waveNumber);
        waveNumber++;
        waveText.text = "Turn Wave " + waveNumber;
        wavePanel.SetActive(true);
        StartCoroutine(HideWavePanel());
        if (waveNumber > 1)
        {
            handManager.DrawCard();
        }
        handManager.AddKhronos();
    }
    private IEnumerator HideWavePanel()
    {
        yield return new WaitForSeconds(1f);
        wavePanel.SetActive(false);
    }

    public void SetWave(int waveIndex)
    {
        Debug.Log("Wave " + waveIndex);
        enemyWave = enemyWaves[waveIndex].enemies;
        foreach (CardCreator card in enemyWave)
        {
            GameObject newCard= Instantiate(enemyPrefab, enemyArea);
            newCard.tag= "Enemy";
            CardPropertiesDrag cardProperties = newCard.GetComponent<CardPropertiesDrag>();
            cardProperties.card= card;
            cardProperties.AssignInfo();
        }
    }
}
