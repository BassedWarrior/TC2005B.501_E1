using UnityEngine;
using System.IO;

public class DataLoader : MonoBehaviour
{
    public CardCreator LoadDataFromJSON(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("No se puede encontrar el archivo.");
            return null;
        }

        string json= File.ReadAllText(path);
        CardCreator card= ScriptableObject.CreateInstance<CardCreator>();
        return card;
    }
}
