using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class PecaViewModel : MonoBehaviour
{
    public Peca peca;
    public Transform fatherTransform;

    public TipoPeca tipoPeca;

    public Text ValorA;
    public Text ValorB;

    public void StartViewModel()
    {
        if (peca != null)
        {
            ValorA.text = peca.ValorA.ToString();
            ValorB.text = peca.ValorB.ToString();

            RectTransform rect = gameObject.GetComponent<RectTransform>();

            switch (tipoPeca)
            {
                case TipoPeca.pecaInicial:
                    
                    fatherTransform = GameObject.FindGameObjectWithTag("baralhoPecaInicialUIVisualizer").transform;

                    GlobalConfigInfo.dominoAdm.posicaoExtremidadeA = fatherTransform.Find("ExtremidadeA");
                    GlobalConfigInfo.dominoAdm.posicaoExtremidadeB = fatherTransform.Find("ExtremidadeB");

                    gameObject.GetComponent<Button>().enabled = false;

                    gameObject.transform.SetParent(fatherTransform);

                    rect.anchoredPosition = new Vector2(0, 0);

                    rect.localScale = new Vector3(1, 1, 1);

                    break;

                case TipoPeca.pecaJogador:
                    fatherTransform = GameObject.FindGameObjectWithTag("baralhoPecaJogadorUIVisualizer").transform;

                    gameObject.transform.SetParent(fatherTransform);

                    if (fatherTransform.childCount == 0)
                    {
                        rect.anchoredPosition = new Vector2(50 * 7, 0);
                    }
                    else
                    {
                        rect.anchoredPosition = new Vector2(50 * (7 - fatherTransform.childCount), 0);
                    }

                    rect.localScale = new Vector3(1, 1, 1);

                    break;

                case TipoPeca.pecaComprar:
                    fatherTransform = GameObject.FindGameObjectWithTag("baralhoPecaComprarUIVisualizer").transform;

                    gameObject.transform.SetParent(fatherTransform);

                    rect.anchoredPosition = new Vector2(0, 0);

                    rect.localScale = new Vector3(1, 1, 1);

                    break;
            }
        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogError("peca is null on PecaViewModel");
        }
    }

    public void SendPeca()
    {
        if (GlobalConfigInfo.currentPlayerTurn == GlobalConfigInfo.playingIdentifier)
        {
            if (fatherTransform.name == "PecasFather")
            {
                if (GlobalConfigInfo.dominoAdm.JogadaValida(this))
                {
                    GlobalConfigInfo.movesCount += 1;

                    //MovesCount.text = GlobalConfigInfo.movesCount.ToString();

                    GlobalConfigInfo.currentPlayerTurn = GlobalConfigInfo.playingIdentifier == PlayingIdentifier.receiver ? PlayingIdentifier.sender : PlayingIdentifier.receiver;

                    NetworkMessage message = new NetworkMessage(DataEvents.PlayerMove, peca);

                    GlobalConfigInfo.nodeClient.SendMessage(message, GlobalConfigInfo.CurrentAdversary);

                    gameObject.GetComponent<Button>().enabled = false;

                    if (GlobalConfigInfo.blockchain.TransactionPool.Count != 0)
                    {
                        GameObject instance = Instantiate(GlobalConfigInfo.gameFormInstance.TransactionPrefab);
                        instance.GetComponent<TransactionViewModel>().transaction = GlobalConfigInfo.blockchain.TransactionPool[GlobalConfigInfo.blockchain.TransactionPool.Count - 1];
                        instance.GetComponent<TransactionViewModel>().StartViewModel();
                    }
                }
                else
                {
                    print("Jogada errada, tente outra peca");
                }
            }
            else if (fatherTransform.name == "PecaComprarFather")
            {
                RectTransform rect = gameObject.GetComponent<RectTransform>();

                fatherTransform = GameObject.FindGameObjectWithTag("baralhoPecaJogadorUIVisualizer").transform;

                gameObject.transform.SetParent(fatherTransform);

                rect.anchoredPosition = new Vector2(50 * (GameObject.FindGameObjectWithTag("baralhoPecaJogadorUIVisualizer").transform.childCount - 1), 0);

                rect.localScale = new Vector3(1, 1, 1);

                print("Comprou!");
            }
            
        } 
        else
        {
            print("Não é sua vez!");
        }

    }
}
