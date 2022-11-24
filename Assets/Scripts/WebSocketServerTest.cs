using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
using System;

public class WebSocketServerTest : MonoBehaviour
{
    //ws://185.246.65.199:9090/ws

    private string url;

    [SerializeField]
    private OdometerBehaviour odometerBehaviour;

    [SerializeField]
    private UiBehavior uiBehavior;

    private string jsonToSend;

    WebSocket webSocket;

    private float oldValue;

    private struct SendRequestBody
    {
        public string operation; 
    }

    private struct SendResponse 
    {
        public string operation;
        public float odometer;
    }

    private async void Start()
    {
        SendRequestBody body = new SendRequestBody()
        {
            operation = "getCurrentOdometer"
        };

        var sData = SavingSystem.LoadServerData();

        url = $"ws://{sData.ServerIp}:{sData.ServerPort}/ws"; 

        jsonToSend = JsonConvert.SerializeObject(body);
        var c = Connect();
        await c;

        if (!c.IsCompletedSuccessfully)
            await c;
    }

    private IEnumerator Reconnect(WebSocket webSocket)
    {
        yield return null;
        bool isConnect = false;

        while (!isConnect)
        {
            yield return new WaitForSeconds(2);
            var s = webSocket.Connect();
            yield return s;

            isConnect = s.IsCompleted;
        }

        StopCoroutine(Reconnect(webSocket));
    }

    [Button]
    private async Task Connect()
    {
        webSocket = new WebSocket(url);
        
        webSocket.OnOpen += () =>
        {
            Debug.Log("Connected");
            uiBehavior.SetGreen();
            StartCoroutine(SendMessCycle());
        };

        webSocket.OnError += (errorMsg) =>
        {
            Debug.Log("Error: " + errorMsg);
            StartCoroutine(Reconnect(webSocket));
        };
        

        webSocket.OnMessage += (bytes) =>
        {
            Debug.Log("OnMessage!");
            var response = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log(response);

            if (response.Contains("currentOdometer"))
            {
                var res = JsonConvert.DeserializeObject<SendResponse>(response);
                if(res.odometer != oldValue)
                {
                    odometerBehaviour.SetUpOdometer(Convert.ToInt32(res.odometer * 100));
                    oldValue = res.odometer;
                }
            }

            // getting the message as a string
            // var message = System.Text.Encoding.UTF8.GetString(bytes);
            // Debug.Log("OnMessage! " + message);
        };

        webSocket.OnClose += (closeCode) => 
        {
            Debug.Log($"Close code: {closeCode}");

            if(closeCode == WebSocketCloseCode.ServerError)
                StartCoroutine(Reconnect(webSocket));

            uiBehavior.SetRed();
        };

        await webSocket.Connect();
    }

    private IEnumerator SendMessCycle()
    {
        while (true) 
        {
            yield return new WaitForSeconds(5);
            Send();
        }
    }

    [Button]
    private async void Send()
    {
        await webSocket.SendText(jsonToSend);
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        webSocket.DispatchMessageQueue();
#endif
    }

}
