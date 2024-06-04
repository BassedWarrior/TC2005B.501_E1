using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<CardData> cards;
    public List<int> playersDeck = new List<int>();
    public List<int> playersHand = new List<int>();
    public List<GameObject> textDots = new List<GameObject>();
    public int playerHealth = 20;
    public int playerDamage = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);    
        }
    }

    public void DeleteDots()
    {
        foreach (GameObject dot in textDots)
        {
            Destroy(dot);
        }
        textDots.Clear();
    }

    public void SortDeck()
    {
        playersDeck.Sort();
    }

    public void ShuffleDeck()
    {
        System.Random rng = new System.Random();
        int n = playersDeck.Count;
        while (n > 1)
        {
            n--;
            int range = rng.Next(n + 1);
            int assess = playersDeck[range];
            playersDeck[range] = playersDeck[n];
            playersDeck[n] = assess;
        }
    }

    public void ResetPlayerDamage()
    {
        this.playerDamage = 0;
    }

    public void AddPlayerDamage(int damage)
    {
        this.playerDamage += damage;
    }

    public void ApplyPlayerDamage()
    {
        this.playerHealth -= this.playerDamage;
        this.playerDamage = 0;
    }
}
