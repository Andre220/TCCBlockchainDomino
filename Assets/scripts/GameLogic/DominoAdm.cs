using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoAdm
{
    public int ValorExtremidadeA;
    public int ValorExtremidadeB;

    public Transform posicaoExtremidadeA;
    public Transform posicaoExtremidadeB;

    List<Peca> pecasGeradas = new List<Peca>();

    public DominoAdm()
    {
    }

    public DominoPecas pecas()
    {
        DominoPecas result = new DominoPecas();

        GerarPecas();

        DistribuirPecasEntreJogadores(result);

        SortearInicial(result);

        result.pecasParaComprar = pecasGeradas;

        return result;
    }

    private void GerarPecas()
    {
        pecasGeradas.Clear();

        for (int i = 0; i <= 27; i++)
        {
            Peca p = new Peca();

            if (i <= 6)
            {
                p.ValorA = 0;
                p.ValorB = i;
            }
            else if (i > 6 && i <= 12)
            {
                p.ValorA = 1;
                p.ValorB = i - 6;
            }
            else if (i > 12 && i <= 17)
            {
                p.ValorA = 2;
                p.ValorB = i - 11;
            }
            else if (i > 17 && i <= 21)
            {
                p.ValorA = 3;
                p.ValorB = i - 15;
            }
            else if (i > 21 && i <= 24)
            {
                p.ValorA = 4;
                p.ValorB = i - 18;
            }
            else if (i > 25 && i <= 26)
            {
                p.ValorA = 5;
                p.ValorB = i - 20;
            }
            else
            {
                p.ValorA = 6;
                p.ValorB = 6;
            }

            pecasGeradas.Add(p);
        }
    }

    private void DistribuirPecasEntreJogadores(DominoPecas dp)
    {
        for (int i = 0; i < 14; i++)
        {
            int choosed = UnityEngine.Random.Range(0, pecasGeradas.Count);

            if (i < 7)//Setando baralho do player que enviou o pedido para jogar
            {
                dp.playerSenderPecas.Add(pecasGeradas[choosed]);
            }
            else//Setando baralho do player que recebeu o pedido para jogar
            {
                dp.playerReceiverPecas.Add(pecasGeradas[choosed]);
            }
            pecasGeradas.RemoveAt(choosed);
        }
    }

    private void SortearInicial(DominoPecas dp)
    {
        int choosed = UnityEngine.Random.Range(0, pecasGeradas.Count);
        dp.pecaInicial = pecasGeradas[choosed];
        ValorExtremidadeA = dp.pecaInicial.ValorA;
        ValorExtremidadeB = dp.pecaInicial.ValorB;
    }

    private void EmbaralharPecasParaComprar()
    {
        Debug.LogError("Embaralhar pecas n'ao implementado!");
    }

    public bool JogadaValida(PecaViewModel p)
    {
        RectTransform rect = p.GetComponent<RectTransform>();

        if (p.peca.ValorA == ValorExtremidadeA)
        {
            p.transform.SetParent(posicaoExtremidadeA);
            rect.anchoredPosition = new Vector2(0, 0);
            p.transform.SetParent(p.fatherTransform);

            ValorExtremidadeA = p.peca.ValorB;

            rect.Rotate(new Vector3(0, 0, 270));
            posicaoExtremidadeA.GetComponent<RectTransform>().anchoredPosition = new Vector2(posicaoExtremidadeA.GetComponent<RectTransform>().anchoredPosition.x - 100, posicaoExtremidadeA.GetComponent<RectTransform>().anchoredPosition.y);
            return true;
        }
        else if (p.peca.ValorA == ValorExtremidadeB)
        {
            p.transform.SetParent(posicaoExtremidadeB);
            rect.anchoredPosition = new Vector2(0,0);
            p.transform.SetParent(p.fatherTransform);

            ValorExtremidadeB = p.peca.ValorB;

            rect.Rotate(new Vector3(0, 0, 90));
            posicaoExtremidadeB.GetComponent<RectTransform>().anchoredPosition = new Vector2(posicaoExtremidadeB.GetComponent<RectTransform>().anchoredPosition.x + 100, posicaoExtremidadeB.GetComponent<RectTransform>().anchoredPosition.y);
            return true;
        }
        else if (p.peca.ValorB == ValorExtremidadeA)
        {
            p.transform.SetParent(posicaoExtremidadeA);
            rect.anchoredPosition = new Vector2(0, 0);
            p.transform.SetParent(p.fatherTransform);

            ValorExtremidadeA = p.peca.ValorA;

            rect.Rotate(new Vector3(0, 0, 90));
            posicaoExtremidadeA.GetComponent<RectTransform>().anchoredPosition = new Vector2(posicaoExtremidadeA.GetComponent<RectTransform>().anchoredPosition.x - 100, posicaoExtremidadeA.GetComponent<RectTransform>().anchoredPosition.y);
            return true;
        }
        else if (p.peca.ValorB == ValorExtremidadeB)
        {
            p.transform.SetParent(posicaoExtremidadeB);
            rect.anchoredPosition = new Vector2(0, 0);
            p.transform.SetParent(p.fatherTransform);

            ValorExtremidadeB = p.peca.ValorA;

            rect.Rotate(new Vector3(0, 0, 270));
            posicaoExtremidadeB.GetComponent<RectTransform>().anchoredPosition = new Vector2(posicaoExtremidadeB.GetComponent<RectTransform>().anchoredPosition.x + 100, posicaoExtremidadeB.GetComponent<RectTransform>().anchoredPosition.y);
            return true;
        }
        else
        {
            return false;
        }
    }
}
