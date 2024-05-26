/* 
Julian Ramirez
23 de mayo 2024

This script saves the user info, and turns it into a 
json file.
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using System.IO;


/* 
// Clase que representa los detalles de un usuario
[System.Serializable]
public class UserDetail
{
    public string UserName;
    public string Password;
}

// Clase que contiene una lista de UserDetail
[System.Serializable]
public class UserDetailsList
{
    public List<UserDetail> userDetails = new List<UserDetail>();
}

public class UserDetailsData
{   
    private string fileName = "UserDetails.json"; // Asegúrate de usar .json en minúsculas
    private string filePath;
    private UserDetailsList userDetailsList;

    private static UserDetailsData instance;

    // Implementación del patrón Singleton
    public static UserDetailsData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UserDetailsData();
                instance.LoadData();
            }   
            return instance;
        }
    }

    // Constructor privado para el patrón Singleton
    private UserDetailsData()
    {
        Debug.Log("Constructor");
        userDetailsList = new UserDetailsList();
        filePath = Path.Combine(Application.persistentDataPath, fileName);
    }

    // Método para añadir un nuevo usuario
    public void AddUser(UserDetail userDetail)
    {
        userDetailsList.userDetails.Add(userDetail);
        SaveData();
    }

    // Método para guardar los datos en un archivo JSON
    private void SaveData()
    {
        string json = JsonUtility.ToJson(userDetailsList, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Data saved to " + filePath);
    }

    // Método para cargar los datos desde un archivo JSON
    private void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            userDetailsList = JsonUtility.FromJson<UserDetailsList>(json);
            Debug.Log("Data loaded from " + filePath);
        }
        else
        {
            Debug.Log("File not found at " + filePath);
        }
    }

    // Método para obtener todos los detalles de los usuarios
    public List<UserDetail> GetAllUsers()
    {
        return userDetailsList.userDetails;
    }
}

*/


public class UserDetailsData
{   
    private string fileName = "UserDetails.Json";
    private string filePath = "";
    private UserDetailsList userDetailsList;
    private static UserDetailsData Instance;




    public UserDetailsData()
    {
        Debug.Log("Constructor");
        userDetailsList = new UserDetailsList();
        filePath = Application.persistentDataPath + "/" + fileName;
    }

    public static UserDetailsData GetInstance()
    {
        if(Instance == null)
        {
            Debug.Log("Initiate");
            Instance = new UserDetailsData();
        }

        return Instance;
    }





    public UserDetailsList GetUserDetailsList(){

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            Debug.Log("Load Json data" + jsonData);
            userDetailsList = JsonUtility.FromJson<UserDetailsList>(jsonData);
        }

        return userDetailsList;
    }





    public void AddUserDetails(UserDetails userDetails)
    {
        userDetailsList.userDetailsList.add(userDetails);

        string UserDetailsJson = JsonUtility.ToJson(userDetailsList);

        Debug.Log("Save Json data" + UserDetailsJson);
        File.WriteAllText(filePath, UserDetailsJson);
    }

    
}
