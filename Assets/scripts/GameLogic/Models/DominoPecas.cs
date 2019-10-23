using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DominoPecas
{
    public List<Peca> playerSenderPecas = new List<Peca>(7);
    public List<Peca> playerReceiverPecas = new List<Peca>(7);
    public List<Peca> pecasParaComprar = new List<Peca>(13);

    public Peca pecaInicial;
}
