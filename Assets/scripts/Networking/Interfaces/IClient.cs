using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClient
{
    ConnectionInfo ConnectToNode(int port); //Returns the connection info
    void SendMessage(NetworkMessage message, ConnectionInfo receiver);
    void DisconnectFromNode(int connectionID);
}
