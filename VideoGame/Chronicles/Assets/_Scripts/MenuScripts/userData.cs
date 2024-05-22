using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public string input;
    public string secret;
    public List<string> UserName;
    public List<string> password;

    // Start is called before the first frame update
    void Start()
    {
        // Inicializar las listas
        UserName = new List<string>();
        password = new List<string>();

        // Imprimir el contenido de las listas al iniciar
        ImprimirListas();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadUserName(string name)
    {
        input = name;
        UserName.Add(input);
        Debug.Log("Nombre de usuario añadido: " + name);

        // Imprimir la lista de nombres de usuario
        ImprimirLista(UserName, "UserName");
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
        ImprimirLista(UserName, "UserName");
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
