using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionForm : MonoBehaviour
{
    public InputField Port;

    public void ClickedConnection()
    {
        GlobalConfigInfo.nodeClient?.ConnectToNode(int.Parse(Port.text));
        GlobalConfigInfo.playingIdentifier = PlayingIdentifier.sender;
        GlobalConfigInfo.currentState = NodeState.Playing;
        GlobalConfigInfo.MyTurn = true;
    }
}
