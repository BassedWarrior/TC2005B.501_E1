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
