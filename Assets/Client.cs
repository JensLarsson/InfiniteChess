using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Client : MonoBehaviour
{
    public Text chatWindow;
    public string clientName;

    private bool socketReady = false;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    public void ConectToServer()
    {
        if (socketReady)
        {
            return;
        }

        string host = "127.0.0.1";
        int port = 6321;

        string h = GameObject.Find("InputField").GetComponent<InputField>().text;
        if (h != "")
        {
            host = h;
        }

        //Create socket
        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;

        }
        catch (Exception ex)
        {
            Debug.LogError("Socket Error : " + ex.Message);
        }

    }

    private void Update()
    {
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                string data = reader.ReadLine();
                if (data != null)
                {
                    OnIncomingData(data);
                }
            }
        }
    }

    private void OnIncomingData(string data)
    {
        if (data == "%NAME")
        {
            SendTextMessage("&NAME|" + clientName);
            return;
        }
        chatWindow.text += data + "\n";
    }
    public void SendTextMessage(string data)
    {
        if (!socketReady) return;
        writer.WriteLine(data);
        writer.Close();
        writer.Flush();
    }

    void CloseSocket()
    {
        if (!socketReady) return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }
    private void OnDisable()
    {
        CloseSocket();
    }
}
