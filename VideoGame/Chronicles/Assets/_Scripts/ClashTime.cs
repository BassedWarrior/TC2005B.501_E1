using System.Collections.Generic;
using UnityEngine;

public class ClashTime : MonoBehaviour
{
    public List<CardPropertiesDrag> timelineA = new List<CardPropertiesDrag>();
    public List<CardPropertiesDrag> timelineB = new List<CardPropertiesDrag>();
    public List<CardPropertiesDrag> timelineC = new List<CardPropertiesDrag>();
    public List<CardPropertiesDrag> quantumTunnel = new List<CardPropertiesDrag>();

    public void UpdateLists(List<CardPropertiesDrag> previousCards, List<CardPropertiesDrag> currentCards, string listName)
    {
        List<CardPropertiesDrag> targetList = GetListByName(listName);

        if (targetList == null)
        {
            return;
        }
        foreach (CardPropertiesDrag card in previousCards)
        {
            if (!currentCards.Contains(card))
            {
                targetList.Remove(card);
            }
        }

        foreach (CardPropertiesDrag card in currentCards)
        {
            if (!targetList.Contains(card))
            {
                targetList.Add(card);
            }
        }
    }

    private List<CardPropertiesDrag> GetListByName(string listName)
    {
        switch (listName)
        {
            case "TimeLineA":
                return timelineA;
            case "TimeLineB":
                return timelineB;
            case "TimeLineC":
                return timelineC;
            case "QuantumTunnel":
                return quantumTunnel;
            default:
                return null;
        }
    }
}
