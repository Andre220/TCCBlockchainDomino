using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginForm : MonoBehaviour
{
    public InputField Port;
    public InputField NickName;

    public void ClickedLogin()
    {
        GameObject node = new GameObject("Node");

        GlobalConfigInfo.ThisNode = new NodeInfo
        {
            nickName = NickName.text,
            port = int.Parse(Port.text),
        };

        GlobalConfigInfo.blockchain = new Blockchain();

        node.AddComponent<Server>().serverPort = int.Parse(Port.text);
        node.AddComponent<Client>();
    }
}
