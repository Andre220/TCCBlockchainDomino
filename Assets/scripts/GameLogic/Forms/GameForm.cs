using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameForm : MonoBehaviour
{
    public List<GameObject> FormsToDisable;
    public List<GameObject> FormsToEnable;

    public List<GameObject> Buttons;//Vai se tornar as pecas do player

    public Text MovesCount;

    public GameObject TransactionPrefab;

    public Camera mainCamera;
    public Color PlayingBackgroundColor;

    void Start()
    {
        GlobalConfigInfo.nodeServer.ConnectEvent += SetupGamePlay;
        //GlobalConfigInfo.nodeServer.PlayerMove += gameDataReceived;
        GlobalConfigInfo.nodeServer.PecaEvent += pecaReceived;
    }

    void SetupGamePlay(ConnectionInfo ci)
    {
        GlobalConfigInfo.CurrentAdversary = ci;

        print("Connection ID: " + GlobalConfigInfo.CurrentAdversary.ConnectionID);
        print("SocketID: " + GlobalConfigInfo.CurrentAdversary.connectedNode.socketID);
        print("Port: " + GlobalConfigInfo.CurrentAdversary.connectedNode.port);
        print("Nickname: " + GlobalConfigInfo.CurrentAdversary.connectedNode.nickName);

        foreach (GameObject g in FormsToDisable)
        {
            g.SetActive(false);
        }

        foreach (GameObject g in FormsToEnable)
        {
            g.SetActive(true);
        }

        //MovesCount.gameObject?.SetActive(true);

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

    public void SendPeca(Peca peca)
    {
        NetworkMessage message = new NetworkMessage(DataEvents.PlayerMove, peca);

        GlobalConfigInfo.nodeClient.SendMessage(message, GlobalConfigInfo.CurrentAdversary);
    }

    public void SendCountMove()
    {
        if (GlobalConfigInfo.currentPlayerTurn == GlobalConfigInfo.playingIdentifier)
        {
            GlobalConfigInfo.movesCount += 1;

            MovesCount.text = GlobalConfigInfo.movesCount.ToString();

            GlobalConfigInfo.currentPlayerTurn = GlobalConfigInfo.playingIdentifier == PlayingIdentifier.receiver ? PlayingIdentifier.sender : PlayingIdentifier.receiver;

            NetworkMessage message = new NetworkMessage(DataEvents.PlayerMove, GlobalConfigInfo.movesCount);
            
            GlobalConfigInfo.nodeClient.SendMessage(message, GlobalConfigInfo.CurrentAdversary);

            if (GlobalConfigInfo.blockchain.TransactionPool.Count != 0)
            {
                GameObject instance = Instantiate(TransactionPrefab);
                instance.GetComponent<TransactionViewModel>().transaction = GlobalConfigInfo.blockchain.TransactionPool[GlobalConfigInfo.blockchain.TransactionPool.Count - 1];
                instance.GetComponent<TransactionViewModel>().StartViewModel();
            }
        }
        else
        {
            print("Não é a sua vez!");
        }
    }

    //void gameDataReceived()
    //{
    //    GlobalConfigInfo.movesCount += 1;

    //    MovesCount.text = GlobalConfigInfo.movesCount.ToString();

    //    GlobalConfigInfo.currentPlayerTurn = GlobalConfigInfo.playingIdentifier;

    //    if (GlobalConfigInfo.blockchain.TransactionPool.Count != 0)
    //    {
    //        GameObject instance = Instantiate(TransactionPrefab);
    //        instance.GetComponent<TransactionViewModel>().transaction = GlobalConfigInfo.blockchain.TransactionPool[GlobalConfigInfo.blockchain.TransactionPool.Count - 1];
    //        instance.GetComponent<TransactionViewModel>().StartViewModel();
    //    }

    //    //TransactionPoolData.text = "";

    //    //foreach (Transaction transaction in GlobalConfigInfo.blockchain.TransactionPool)
    //    //{
    //    //    GameObject instance = Instantiate(TransactionPrefab);
    //    //    instance.GetComponent<TransactionViewModel>().transaction = transaction;
    //    //    instance.GetComponent<TransactionViewModel>().StartViewModel();
    //    //    //TransactionPoolData.text += $"From: {transaction.FromAddress}| To: {transaction.ToAddress}| Data: {transaction.Data}";
    //    //}

    //    Debug.Log("Moves count: " + GlobalConfigInfo.movesCount);
    //}

    void pecaReceived(Peca peca)
    {
        GlobalConfigInfo.movesCount += 1;

        MovesCount.text = GlobalConfigInfo.movesCount.ToString();

        GlobalConfigInfo.currentPlayerTurn = GlobalConfigInfo.playingIdentifier;

        if (GlobalConfigInfo.blockchain.TransactionPool.Count != 0)
        {
            GameObject instance = Instantiate(TransactionPrefab);
            instance.GetComponent<TransactionViewModel>().transaction = GlobalConfigInfo.blockchain.TransactionPool[GlobalConfigInfo.blockchain.TransactionPool.Count - 1];
            instance.GetComponent<TransactionViewModel>().StartViewModel();
        }

        //Process the UI

        Debug.Log("Moves count: " + GlobalConfigInfo.movesCount);
        Debug.Log($"A : {peca.ValorA} | B : {peca.ValorB}");
    }
}
