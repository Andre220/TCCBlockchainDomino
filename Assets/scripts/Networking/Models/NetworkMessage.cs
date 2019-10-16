using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMessage
{
    public NodeInfo Sender;
    public DataEvents MessageType;
    public object Message;

    public NetworkMessage(DataEvents messageType, object message)
    {
        Sender = GlobalConfigInfo.ThisNode;
        MessageType = messageType;
        Message = message;
    }
}
