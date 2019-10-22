using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DominoPecas
{
    public List<Peca> playerAPecas = new List<Peca>(7);
    public List<Peca> playerBPecas = new List<Peca>(7);
    public List<Peca> pecasParaComprar = new List<Peca>(13);

    public Peca pecaInicial;
}
