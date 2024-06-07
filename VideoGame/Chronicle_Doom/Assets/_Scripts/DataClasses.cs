using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class CardData
{
    public int cardID;
    public string name;
    public string description;
    public int cost;
    public int attack;
    public int health;
    public int damage = 0;
    public int scoreValue = 0;

    public bool IsAlive()
    {
        return this.health > 0;
    }

    public void ResetDamage()
    {
        this.damage = 0;
    }

    public void AddDamage(int damage)
    {
        this.damage += damage;
    }

    public void ApplyDamage()
    {
        if (this.damage == 0)
        {
            return;
        }

        this.health = Mathf.Max(0, this.health - this.damage);
        this.damage = 0;
    }

    public CardData DeepCopy()
    {
        CardData other = (CardData) this.MemberwiseClone();
        other.attack = this.attack;
        other.health = this.health;
        other.scoreValue = this.scoreValue;

        return other;
    }

    public void CalculateScoreValue()
    {
        this.scoreValue = 20 * this.attack + 10 * this.health;
    }

    public int GetScoreValue()
    {
        return this.scoreValue;
    }
}

[System.Serializable]
public class DeckCard
{
    public int cardID;
    public int card_times;
}

[System.Serializable]
public class Deck {
    public string deck_name;
    public DeckCard[] cards;
}


[System.Serializable]
public class CardDataArrayWrapper
{
    public CardData[] cards;
}

[System.Serializable]
public class DeckCardArrayWrapper
{
    public DeckCard[] deckCards;
}

[System.Serializable]
public class DeckArrayWrapper
{
    public Deck[] Items;
}

[System.Serializable]
public class EnemyWave
{
    public string waveName;
    public List<CardData> enemies;
}

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

[System.Serializable]
public class GameScore
{
    public string username;
    public int score;
}

[System.Serializable]
public class GameScoreWrapper
{
    public List<GameScore> gameScores;
}
