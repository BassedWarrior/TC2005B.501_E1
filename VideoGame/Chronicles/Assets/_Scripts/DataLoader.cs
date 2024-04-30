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

/*
void Start()
{
no se como coño hacer para meter una esa info dentro de la carta ;)
}

{
    "name": "Juan",
    "description": "completamente rota",
    "artwork": 100201020102001020
}

*/