using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockchainForm : MonoBehaviour
{
    #region TransacionCreation
    public InputField Sender;
    public InputField Receiver;
    public InputField Amount;
    #endregion TransacionCreation

    #region BlockCreation
    public InputField MinerAddress;
    #endregion BlockCreation

    #region BlockVisualization
    public InputField SearchId;
    public Text BlockInfo;
    #endregion BlockVisualization

    //Criar testes para ver se algum campo esta vazio

    public void AddTransactionButton()
    {
        BlockchainInstancer.blockchain.CreateTransaction(new Transaction(Sender.text, Receiver.text, int.Parse(Amount.text)));
    }

    public void AddNewBlockButton()
    {
        BlockchainInstancer.blockchain.ProcessTransactionPool(MinerAddress.text);
    }


    public void ShowBlockInfo()
    {
        var transactions = BlockchainInstancer.blockchain.GetBlockTransactionsByBlockID(int.Parse(SearchId.text));

        foreach (Transaction t in transactions)
        {
            BlockInfo.text += "| from " + t.FromAddress + " to " + t.ToAddress + " the amount of " + t.AmountAddress + " |  ";
        }
    }
}
