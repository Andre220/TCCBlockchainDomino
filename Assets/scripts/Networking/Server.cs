using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour, IServer
{
    public event Action<ConnectionInfo> ConnectEvent;

    public event Action DataEvent;
    
    public event Action PlayerMove;
    
    public event Action<Peca> PecaEvent;

    public event Action<DominoPecas> PecasDoJogo;
    
    public event Action PlayRequest;

    public event Action Syncronizantion;

    public event Action EndGame;

    //public event Action<DominoPecas, int, NodeInfo> StartPlay;

    public event Action<int> DisconnectEvent;

    public List<ConnectionInfo> KnowNodes;

    private const int MAX_CONNECTION = 20;

    public int socketID;

    private int reliableChannel;
    private int unreliableChannel;

    private byte error;

    public int serverPort; // Port that is open and listening for messages.

    void Start()
    {
        ConfigureNetworkInit();
        GlobalConfigInfo.nodeServer = this;
    }

    void Update()
    {
        ServerUpdate();
    }

    void ConfigureNetworkInit()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        cc.PacketSize = GlobalConfigInfo.GlobalPacketSize;
        cc.FragmentSize = GlobalConfigInfo.GlobalFragmentSize;

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        socketID = NetworkTransport.AddHost(topo, serverPort, null);// the ip is null because we are at localhost - i should test it with 127.0.0.1 to see how it behave

        GlobalConfigInfo.ThisNode.socketID = socketID;

        KnowNodes = new List<ConnectionInfo>();
    }

    void ServerUpdate()
    {
        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[2500];
        int bufferSize = 2500;
        int dataSize;
        byte error;

        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

        if ((NetworkError)error != NetworkError.Ok)
        {
            Debug.LogError((NetworkError)error);
        }
        else
        {
            switch (recData)
            {
                case NetworkEventType.Nothing:
                    break;

                case NetworkEventType.ConnectEvent:
                    OnConnectEvent(recHostId, connectionId, (NetworkError)error);
                    break;

                case NetworkEventType.DataEvent:
                    OnDataEvent(recHostId, connectionId, recBuffer, (NetworkError)error);
                    break;

                case NetworkEventType.DisconnectEvent:
                    OnDisconnectEvent(recHostId, connectionId, (NetworkError)error);
                    break;
                case NetworkEventType.BroadcastEvent:
                    break;
            }
        }
    }

    public void OnConnectEvent(int socketID, int connectionId, NetworkError error)
    {
        //connectionsID.Add(connectionId);

        ConnectionInfo connectedNode = new ConnectionInfo
        {
            ConnectionID = connectionId,
            connectedNode = new NodeInfo
            {
                socketID = socketID,
                port = -1,
                nickName = "Guest" + DateTime.Now,
            } 
        };

        KnowNodes.Add(connectedNode);

        ConnectEvent?.Invoke(connectedNode);

        print("|Connection event: " +
            " |HostId: " + socketID +
            " |ConnectionId : " + connectionId +
            " |Error: " + error.ToString());
    }

    public void OnDataEvent(int socketID, int connectionId, byte[] buffer, NetworkError error)
    {
        DataEvent?.Invoke();

        var JsonText = Encoding.Unicode.GetString(buffer);

        NetworkMessage message = JsonConvert.DeserializeObject<NetworkMessage>(JsonText);

        switch (message.MessageType)
        {
            case DataEvents.ConnectionInfoRequest: // Get connection info and save in connections pool
                //OnConnectionInfoEvent(hostId, connectionId, message, buffer, error);
                break;
            case DataEvents.PlayRequest: // if connection response is ok, continue. Else, disconnect from node.
                PlayRequest?.Invoke();
                break;

            case DataEvents.PecasDoJogo: // if connection response is ok, continue. Else, disconnect from node.
                try
                {
                    DominoPecas dominoPecas = JsonConvert.DeserializeObject<DominoPecas>(message.Message.ToString());
                    GlobalConfigInfo.blockchain.CreateTransaction(GlobalConfigInfo.CurrentAdversary.connectedNode.nickName, GlobalConfigInfo.ThisNode.nickName, dominoPecas);
                    PecasDoJogo?.Invoke(dominoPecas);
                }
                catch (Exception e)
                {
                    string exc = e.ToString();
                }
                break;

            case DataEvents.PlayerMove: // call event to deal with this event and every gameplay script who sould know about network info should do your action
                try
                {
                    Peca peca = JsonConvert.DeserializeObject<Peca>(message.Message.ToString());
                    GlobalConfigInfo.blockchain.CreateTransaction(GlobalConfigInfo.CurrentAdversary.connectedNode.nickName, GlobalConfigInfo.ThisNode.nickName, peca);
                    PecaEvent?.Invoke(peca);
                }
                catch (Exception e)
                {
                    string exc = e.ToString();
                }

                //PlayerMove?.Invoke();

                break;
            case DataEvents.EndGame: // call event to deal with this event and every gameplay script who sould know about network info should do your action
                EndGame?.Invoke();
                break;
        }

        print("|Data event: " +
            "/n|HostId: " + socketID +
            "/n|ConnectionId : " + connectionId +
            "/n|Error: " + error.ToString());
    }

    public void OnDisconnectEvent(int hostId, int connectionId, NetworkError error)
    {
        DisconnectEvent?.Invoke(connectionId);

        print("|Disconnect event: " +
            "/n|HostId: " + hostId +
            "/n|ConnectionId : " + connectionId +
            "/n|Error: " + error.ToString());
    }

}
