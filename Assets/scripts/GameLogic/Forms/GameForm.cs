using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameForm : MonoBehaviour
{
    public List<GameObject> forms;

    public List<GameObject> Buttons;

    public GameObject Panel; 
    public Text MovesCount;
    public Text TransactionPoolData;

    public Camera mainCamera;
    public Color PlayingBackgroundColor;

    void Start()
    {
        GlobalConfigInfo.nodeServer.ConnectEvent += SetupGamePlay;
        GlobalConfigInfo.nodeServer.PlayerMove += gameDataReceived;
    }

    void SetupGamePlay(ConnectionInfo ci)
    {
        GlobalConfigInfo.CurrentAdversary = ci;

        print("Connection ID: " + GlobalConfigInfo.CurrentAdversary.ConnectionID);
        print("SocketID: " + GlobalConfigInfo.CurrentAdversary.connectedNode.socketID);
        print("Port: " + GlobalConfigInfo.CurrentAdversary.connectedNode.port);
        print("Nickname: " + GlobalConfigInfo.CurrentAdversary.connectedNode.nickName);

        foreach (GameObject g in forms)
        {
            g.SetActive(false);
        }

        TransactionPoolData.gameObject?.SetActive(true);
        MovesCount.gameObject?.SetActive(true);
        Panel.SetActive(true);

        SetupScene();
        SetupPlayingIdentifier();
    }

    void SetupScene()
    {
        mainCamera.backgroundColor = PlayingBackgroundColor;
    }

    void SetupPlayingIdentifier()
    {
        GlobalConfigInfo.currentState = NodeState.Playing;

        if (GlobalConfigInfo.playingIdentifier == PlayingIdentifier.unknown)
        {
            GlobalConfigInfo.playingIdentifier = PlayingIdentifier.receiver;
        }

        switch (GlobalConfigInfo.playingIdentifier)
        {
            case PlayingIdentifier.receiver:
                Buttons[(int)PlayingIdentifier.receiver].SetActive(true);
                break;

            case PlayingIdentifier.sender:
                Buttons[(int)PlayingIdentifier.sender].SetActive(true);
                break;
        }
    }

    public void SendMove(int value)
    {
        NetworkMessage message = new NetworkMessage(DataEvents.PlayerMove, value);

        GlobalConfigInfo.nodeClient.SendMessage(message, GlobalConfigInfo.CurrentAdversary);
    }

    public void SendCountMove()
    {
        if (GlobalConfigInfo.currentPlayerTurn == GlobalConfigInfo.playingIdentifier)
        {
            TransactionPoolData.text = "";

            GlobalConfigInfo.movesCount += 1;

            MovesCount.text = GlobalConfigInfo.movesCount.ToString();

            GlobalConfigInfo.currentPlayerTurn = GlobalConfigInfo.playingIdentifier == PlayingIdentifier.receiver ? PlayingIdentifier.sender : PlayingIdentifier.receiver;

            NetworkMessage message = new NetworkMessage(DataEvents.PlayerMove, GlobalConfigInfo.movesCount);
            
            GlobalConfigInfo.nodeClient.SendMessage(message, GlobalConfigInfo.CurrentAdversary);

            foreach (Transaction transaction in GlobalConfigInfo.blockchain.TransactionPool)
            {
                TransactionPoolData.text += $"From: {transaction.FromAddress}| To: {transaction.ToAddress}| Data: {transaction.Data}";
            }
        }
        else
        {
            print("Não é a sua vez!");
        }
    }

    void gameDataReceived()
    {
        GlobalConfigInfo.movesCount += 1;

        MovesCount.text = GlobalConfigInfo.movesCount.ToString();

        GlobalConfigInfo.currentPlayerTurn = GlobalConfigInfo.playingIdentifier;

        TransactionPoolData.text = "";

        foreach (Transaction transaction in GlobalConfigInfo.blockchain.TransactionPool)
        {
            TransactionPoolData.text += $"From: {transaction.FromAddress}| To: {transaction.ToAddress}| Data: {transaction.Data}";
        }

        Debug.Log("Moves count: " + GlobalConfigInfo.movesCount);
    }
}
