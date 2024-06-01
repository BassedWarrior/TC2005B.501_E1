[System.Serializable]
public class CardData
{
    public int cardID;
    public string name;
    public string description;
    public int attack;
    public int health;
    public int cost;

    public CardData DeepCopy()
    {
        CardData other = (CardData) this.MemberwiseClone();
        other.attack = this.attack;
        other.health = this.health;

        return other;
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