using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionForm : MonoBehaviour
{
    public InputField Port;

    void Start()
    {
        
    }

    public void ClickedConnection()
    {
        GlobalConfigInfo.nodeClient?.ConnectToNode(int.Parse(Port.text));
        GlobalConfigInfo.playingIdentifier = PlayingIdentifier.sender;
        GlobalConfigInfo.MyTurn = true;
    }
}
