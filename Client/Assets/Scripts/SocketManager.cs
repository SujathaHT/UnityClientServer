using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json.Linq;

public class SocketManager : MonoBehaviour
{
    WebSocket socket;
    public GameObject player;
    public PlayerData data;

    // Start is called before the first frame update
    void Start()
    {
        socket = new WebSocket("ws://localhost:8080");
        socket.Connect();

        socket.OnMessage += (sender, e) =>
        {
            if (e.IsText)
            {
                JObject dataJson = JObject.Parse(e.Data);
                if (dataJson["id"] != null)
                {
                    data.id = dataJson["id"].ToString();
                    Debug.Log("data: " + dataJson.ToString());
                    // todo: handle login result
                    return;
                }

            }
        };
        socket.OnClose += (sender, e) =>
        {
            Debug.Log("close: " + e.Code + ", " + e.Reason);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if(socket == null)
        {
            return;
        }

        if (player && data.id.Length > 0)
        {
            data.xPos = player.transform.position.x;
            data.yPos = player.transform.position.y;
            data.zPos = player.transform.position.z;

            System.DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
            double timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;
            data.timestamp = timestamp;
            string dataJsonString = JsonUtility.ToJson(data);
            socket.Send(dataJsonString);

        }

        if (Input.GetKeyDown(KeyCode.M))
        {

            string messageJson = "{\"message\":\"test\"}";
            socket.Send(messageJson);
        }
    }

    public void Send(string data)
    {
        if (socket != null)
        {
            Debug.Log("Sending data: " + data);
            socket.Send(data);
        }
    }

    private void OnDestroy()
    {
        if(socket != null)
        {
            socket.Close();
        }
    }
}
