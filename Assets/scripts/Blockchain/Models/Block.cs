using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

//Lembre-se: Os blocos são um conjunto de transações
//Esse script é responsável também pela mineração
public class Block : MonoBehaviour
{
    public int Index { get; set; }
    public DateTime TimeStamp { get; set; }
    public string PreviousBlockHash { get; set; }
    public string BlockHash { get; set; }
    public IList<Transaction> Transactions { get; set; }
    public int nonce { get; set; } = 0;

    public Block(DateTime timeStamp, string previousHash, IList<Transaction> transaction)
    {
        Index = 0;
        TimeStamp = timeStamp;
        PreviousBlockHash = previousHash;
        Transactions = transaction;
        BlockHash = CalculateHash();
    }

    public string CalculateHash()
    {
        SHA256 sha256 = SHA256.Create();

        byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousBlockHash ?? ""}-{JsonConvert.SerializeObject(Transactions)}-{nonce}");
        byte[] outputBytes = sha256.ComputeHash(inputBytes);

        return Convert.ToBase64String(outputBytes);
    }

    public async void Mine(int difficult)
    {
        var leadingZeros = new string('0', difficult);
        while (this.BlockHash == null || this.BlockHash.Substring(0, difficult) != leadingZeros)
        {
            this.nonce++;
            this.BlockHash = this.CalculateHash();
        }
    }
}
