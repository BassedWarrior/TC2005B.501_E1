using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginData : MonoBehaviour
{
    public string input;
    public string secret;
    public List<string> username = new List<string>();
    public List<string> password = new List<string>();

    public void ReadUserName(string name)
    {
        input = name;
        username.Add(input);
        Debug.Log("Nombre de usuario añadido: " + name);

        ImprimirLista(username, "UserName");
    }

    public void ReadPassword(string secret)
    {
        input = secret;
        password.Add(secret);
        Debug.Log("Contraseña añadida: " + secret);

        // Imprimir la lista de contraseñas
        ImprimirLista(password, "password");
    }

    // Método para imprimir el contenido de las listas
    public void ImprimirListas()
    {
        ImprimirLista(username, "UserName");
        ImprimirLista(password, "password");
    }

    // Método genérico para imprimir una lista
    public void ImprimirLista(List<string> lista, string nombreLista)
    {
        Debug.Log("Contenido de la lista " + nombreLista + ":");
        foreach (string elemento in lista)
        {
            Debug.Log(elemento);
        }
    }
}
