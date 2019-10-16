using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockchainInstancer : MonoBehaviour
{
    public static Blockchain blockchain = new Blockchain();//Singleton of the blockchain

    public int LastChainSize;
    public int CurrentChainSize;

    void Start()
    {
        LastChainSize = blockchain.Chain.Count;
    }

    void Update()
    {
        CurrentChainSize = blockchain.Chain.Count;

        if (CurrentChainSize != LastChainSize)
        {
            LastChainSize = CurrentChainSize;
            Debug.Log("CHAIN SIZE CHANGED FROM " + LastChainSize + " TO " + CurrentChainSize);
        }
    }
}
