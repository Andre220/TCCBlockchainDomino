using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransactionViewModel : MonoBehaviour
{
    public Transaction transaction;

    public Text Remetente;
    public Text Destinatario;
    public Text Valor;
    public Text Dados;

    public Transform fatherTransform;

    public void StartViewModel()
    {
        if (transaction != null)
        {
            Remetente.text = $"De: {transaction.FromAddress}";
            Destinatario.text = $"Para: {transaction.ToAddress}";
            Valor.text = $"Valor: {transaction.AmountAddress}";
            Dados.text = $"Info: {transaction.Data}";

            try
            {
                fatherTransform = GameObject.FindGameObjectWithTag("transactionPoolUIVisualizer").transform;

                gameObject.transform.SetParent(fatherTransform);

                RectTransform rect = gameObject.GetComponent<RectTransform>();

                if (fatherTransform.childCount == 0)
                {
                    rect.anchoredPosition = new Vector3(0, 180, 0);
                    rect.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    rect.anchoredPosition = new Vector3(0, rect.sizeDelta.y * fatherTransform.childCount, 0);
                    rect.localScale = new Vector3(1, 1, 1);
                }
            }
            catch (Exception e)
            {
                var exc = e;
            }
           

        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogError("transaction is null on TransactionViewModel");
        }
    }
}
