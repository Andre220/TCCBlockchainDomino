using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public interface IServer
{
    event Action<ConnectionInfo> ConnectEvent;
    event Action DataEvent;
    event Action PlayerMove;
    event Action<Peca> PecaEvent;
    event Action PlayRequest;
    event Action Syncronizantion;
    //event Action<DominoPecas, int, NodeInfo> StartPlay;
    event Action<int> DisconnectEvent;

    void OnConnectEvent(int socketID, int connectionId, NetworkError error);

    void OnDataEvent(int socketID, int connectionId, byte[] buffer, NetworkError error);

    void OnDisconnectEvent(int socketID, int connectionId, NetworkError error);

}
