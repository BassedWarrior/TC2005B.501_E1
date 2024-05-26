using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
public class registrationJson : MonoBehaviour
{

    public TMP_InputField usernameTMP;
    public TMP_InputField passwordTMP;


    [System.Serializable]
    public class InputData
    {
        public string username;
        public string password;
    }

    public void JsonFile()
    {
        InputData inputData= new InputData();
        inputData.username = usernameTMP.text;
        inputData.password = passwordTMP.text;

        string json = JsonUtility.ToJson(inputData);
        Debug.Log("Json genereted: " + json);

        /*
        // Ruta del directorio donde se guardará el archivo JSON
        string directoryPath = Application.persistentDataPath; // Directorio persistente de la aplicación

        // Nombre del archivo
        string fileName = "userData.json";

        // Ruta completa del archivo
        string filePath = Path.Combine(directoryPath, fileName);

        // Escribe el JSON en el archivo
        File.WriteAllText(filePath, json);

        Debug.Log("Archivo guardado en: " + filePath);*/

    }



}
