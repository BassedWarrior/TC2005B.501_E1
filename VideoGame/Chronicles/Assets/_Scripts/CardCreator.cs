using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName= "ScriptableObjects/Cards")]
public class CardCreator : ScriptableObject
{
    public int ID;
    public new string name;
    public string description;
    public Sprite artwork;
    public int energyCost;
    public int health;
    public int attack;
}
/*
public class UserCard : MonoBehaviour
{
    public CardCreator card;

    private void Start()
    {
        Debug.Log($"Int value: {card.intValue}");
        Debug.Log($"Float value: {card.floatValue}");
        Debug.Log($"String value: {card.stringValue}");
    }
}
*/