using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[Serializable]

public struct LoginCredentials
{
    public string username;
    public string password;
    public string message_type;
}

public class LoginButton : MonoBehaviour
{
    public InputField username;
    public InputField password;
    [SerializeField] public Button button = null;
    public SocketManager socketManager;
    // Start is called before the first frame update


    void Start()
    {
        button.onClick.AddListener(() =>
        {
            LoginCredentials credentials;
            credentials.username = username.text;
            credentials.password = password.text;
            credentials.message_type = "login";
            string dataJsonString = JsonUtility.ToJson(credentials);
            socketManager.Send(dataJsonString);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
