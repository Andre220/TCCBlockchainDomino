using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour, IClient
{
    public const int MAX_CONNECTION = 20;

    int reliableChannel;
    int reliableFragmentedChannel;
    int unreliableChannel;

    private byte error;

    void Start()
    {
        ConfigureNetworkInit();
        GlobalConfigInfo.nodeClient = this;
    }

    void ConfigureNetworkInit()
    {
        ConnectionConfig cc = new ConnectionConfig();

        cc.PacketSize = GlobalConfigInfo.GlobalPacketSize;
        cc.FragmentSize = GlobalConfigInfo.GlobalFragmentSize;

        reliableChannel = cc.AddChannel(QosType.Reliable);
        reliableFragmentedChannel = cc.AddChannel(QosType.ReliableFragmented);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        NetworkTransport.AddHost(topo, 0);
    }

    public ConnectionInfo ConnectToNode(int port)
    {
        //Connect to node

        int connectionID = NetworkTransport.Connect(GlobalConfigInfo.ThisNode.socketID, "127.0.0.1", port, 0, out error);

        //return connection info with this connection ID

        ConnectionInfo CI = new ConnectionInfo
        {
            ConnectionID = connectionID,
            connectedNode = new NodeInfo
            {
                socketID = -1,
                port = -1,
                nickName = "Guest " + DateTime.Now,
            }
        };

        return CI;
    }

    public void SendMessage(NetworkMessage message, ConnectionInfo destination)
    {
        string messageBaseObjectJson = JsonConvert.SerializeObject(message);
        byte[] buffer = Encoding.Unicode.GetBytes(messageBaseObjectJson);

        if (message.MessageType == DataEvents.PlayerMove)
        {
            GlobalConfigInfo.blockchain.CreateTransaction(GlobalConfigInfo.ThisNode.nickName, GlobalConfigInfo.CurrentAdversary.connectedNode.nickName, message);
        }

        NetworkTransport.Send(GlobalConfigInfo.ThisNode.socketID, destination.ConnectionID, reliableChannel, buffer, messageBaseObjectJson.Length * sizeof(char), out error);

        if ((NetworkError)error != NetworkError.Ok)
        {
            Debug.LogError((NetworkError)error);
        }
    }

    public void DisconnectFromNode(int connectionID)
    {
        bool success = NetworkTransport.Disconnect(GlobalConfigInfo.ThisNode.socketID, connectionID, out error);
    }
}
