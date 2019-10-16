using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transaction : MonoBehaviour
{
    public string FromAddress { get; set; }
    public string ToAddress { get; set; }
    public int AmountAddress { get; set; }
    public object Data { get; set; }

    public Transaction(string from, string to, int amount)
    {
        FromAddress = from;
        ToAddress = to;
        AmountAddress = amount;
    }

    public Transaction(string from, string to, object data)
    {
        FromAddress = from;
        ToAddress = to;
        AmountAddress = 0;
        Data = data;
    }
}
