using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CardAbility
{
    public int damage;
    public int heal;
    public int attack;
    public List<string> targets;

    public CardAbility(int damage, int heal, int attack, List<string> targets)
    {
        this.damage = damage;
        this.heal = heal;
        this.attack = attack;
        this.targets = targets;
    }
}

[System.Serializable]
public class CardAbilityArrayWrapper
{
    public CardAbility[] cards;
}

public static class CardAbilities
{
    public static CardAbility JollyRogerDamage = new CardAbility(2, 0, 0, new List<string> { "EnemyLineB" });
    public static CardAbility JollyRogerHeal = new CardAbility(0, 2, 0, new List<string> { "TimeLineB" });
    public static CardAbility BloodChalisDamage = new CardAbility(3, 0, 0, new List<string> { "TimeLineC" });
    public static CardAbility BloodChalisBuff = new CardAbility(0, 0, 3, new List<string> { "TimeLineA" });
    public static CardAbility BlackPlague = new CardAbility(0, 0, -3, new List<string> { "EnemyLineA", "EnemyLineB", "EnemyLineC" });
    public static CardAbility Abduction = new CardAbility(0, 5, -5, new List<string> { "EnemyLineA", "EnemyLineB", "EnemyLineC" });
    public static CardAbility MillenialKnowledgeHeal = new CardAbility(0, 5, 0, new List<string> { "TimeLineA" });
    public static CardAbility MillenialKnowledgeBuff = new CardAbility(0, 0, 5, new List<string> { "TimeLineB" });
    public static CardAbility DinosaurMeteor = new CardAbility(10, 0, 0, new List<string> { "EnemyLineA", "EnemyLineB", "EnemyLineC", "TimeLineA", "TimeLineB", "TimeLineC" });
    public static CardAbility TzarBomba = new CardAbility(5, 0, 0, new List<string> { "EnemyLineA", "EnemyLineB", "EnemyLineC" });
}
