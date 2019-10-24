using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameForm : MonoBehaviour
{
    public List<GameObject> FormsToDisable;
    public List<GameObject> FormsToEnable;

    public List<GameObject> PlayerBaralho;//Vai se tornar as pecas do player

    public Text MovesCount;

    public GameObject TransactionPrefab;
    public GameObject PecaPrefab;

    public Camera mainCamera;
    public Color PlayingBackgroundColor;
    public Color LobbyBackgroundColor;

    void Start()
    {
        LobbyBackgroundColor = mainCamera.backgroundColor;

        GlobalConfigInfo.nodeServer.ConnectEvent += SetupGamePlay; //ativa os objetos e gera o baralho caso o player seja o sender
        GlobalConfigInfo.nodeServer.PecasDoJogo += pecasDoJogoReceived; // Quando o player recebe as pecas do jogo
        GlobalConfigInfo.nodeServer.PecaEvent += pecaReceived; // quando o player recebe uma peca
        GlobalConfigInfo.nodeServer.EndGame += EndGameConfig; // quando o player recebe uma peca

        if (GlobalConfigInfo.gameFormInstance != null)
        {
            Destroy(GlobalConfigInfo.gameFormInstance.gameObject);
            GlobalConfigInfo.gameFormInstance = this;
        }
        else
        {
            GlobalConfigInfo.gameFormInstance = this;
        }
    }

    void Update()
    {
        MovesCount.text = GlobalConfigInfo.movesCount.ToString();    
    }

    void SetupGamePlay(ConnectionInfo ci)
    {
        GlobalConfigInfo.CurrentAdversary = ci;

        foreach (GameObject g in FormsToDisable)
        {
            g?.SetActive(false);
        }

        foreach (GameObject g in FormsToEnable)
        {
            g?.SetActive(true);
        }

        SetupScene();
        SetupPlayingIdentifier();

        if (GlobalConfigInfo.playingIdentifier == PlayingIdentifier.sender)
        {
            GlobalConfigInfo.pecasDoJogo = GlobalConfigInfo.dominoAdm.pecas();

            NetworkMessage baralhoDoJogo = new NetworkMessage(DataEvents.PecasDoJogo, GlobalConfigInfo.pecasDoJogo);

            GlobalConfigInfo.nodeClient.SendMessage(baralhoDoJogo, GlobalConfigInfo.CurrentAdversary);

            pecasDoJogoReceived(GlobalConfigInfo.pecasDoJogo);
        }
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
    }

    void pecasDoJogoReceived(DominoPecas pecas) // esse evento so acontece no player que foi desafiado
    {
        if (GlobalConfigInfo.playingIdentifier == PlayingIdentifier.sender) // Isso faz sentido?
        {
            GenerateBaralho(pecas.playerSenderPecas, TipoPeca.pecaJogador);
        }
        else if (GlobalConfigInfo.playingIdentifier == PlayingIdentifier.receiver)
        {
            GenerateBaralho(pecas.playerReceiverPecas, TipoPeca.pecaJogador);
        }

        GeneratePeca(pecas.pecaInicial, TipoPeca.pecaInicial);

        foreach (Peca p in pecas.pecasParaComprar)
        {
            GeneratePeca(p, TipoPeca.pecaComprar);
        }

        GlobalConfigInfo.dominoAdm.ValorExtremidadeA = pecas.pecaInicial.ValorA;
        GlobalConfigInfo.dominoAdm.ValorExtremidadeB = pecas.pecaInicial.ValorB;

            //if (GlobalConfigInfo.blockchain.TransactionPool.Count != 0) // o ato de receber as pecas precisa ser transacionado?
            //{
            //    GameObject instance = Instantiate(TransactionPrefab);
            //    instance.GetComponent<TransactionViewModel>().transaction = GlobalConfigInfo.blockchain.TransactionPool[GlobalConfigInfo.blockchain.TransactionPool.Count - 1];
            //    instance.GetComponent<TransactionViewModel>().StartViewModel();
            //}
    }

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

        PecaViewModel pvm = Instantiate(PecaPrefab).GetComponent<PecaViewModel>();
        pvm.peca = peca;
        pvm.StartViewModel();
        pvm.fatherTransform = GameObject.FindGameObjectWithTag("baralhoPecaAdversarioUIVisualizer").transform;
        pvm.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        GlobalConfigInfo.dominoAdm.JogadaValida(pvm);

        Debug.Log("Moves count: " + GlobalConfigInfo.movesCount);
        Debug.Log($"A : {peca.ValorA} | B : {peca.ValorB}");
    }

    public void GenerateBaralho(List<Peca> pecas, TipoPeca tipoDasPecas)
    {
        foreach (Peca p in pecas)
        {
            GeneratePeca(p, tipoDasPecas);
        }
    }

    public void GeneratePeca(Peca p, TipoPeca tipo)
    {
        GameObject instance = Instantiate(PecaPrefab);
        instance.GetComponent<PecaViewModel>().tipoPeca = tipo;
        instance.GetComponent<PecaViewModel>().peca = p;
        instance.GetComponent<PecaViewModel>().StartViewModel();
    }

    public void ShowTransactionsUI(RectTransform transactionUI)
    {
        transactionUI.anchoredPosition = new Vector3(0, 0, 0);
    }

    public void UnshowTransactionsUI(RectTransform transactionUI)
    {
        transactionUI.anchoredPosition = new Vector3(400, 0, 0);
    }

    public void EndGame()
    {
        EndGameConfig();

        GlobalConfigInfo.blockchain.ProcessTransactionPool(GlobalConfigInfo.ThisNode.nickName);

        NetworkMessage engaGameMessage = new NetworkMessage(DataEvents.EndGame, GlobalConfigInfo.ThisNode.nickName);

        GlobalConfigInfo.nodeClient.SendMessage(engaGameMessage, GlobalConfigInfo.CurrentAdversary);

        GlobalConfigInfo.CurrentAdversary = null;
    }

    public void EndGameConfig()
    {
        mainCamera.backgroundColor = PlayingBackgroundColor;

        foreach (GameObject g in FormsToDisable)
        {
            if (g.name != "(Form)Login")
                g?.SetActive(true);
        }

        foreach (GameObject g in FormsToEnable)
        {
            g?.SetActive(false);
        }

        GlobalConfigInfo.MyTurn = false;
        GlobalConfigInfo.movesCount = 0;
    }
}
