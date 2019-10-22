using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockViewModel : MonoBehaviour
{
    public Block block;

    public Text IndexUIElement;
    public Text TimeStampUIElement;
    public Text PreviousHashUIElement;
    public Text CurrentHashUIElement;

    public Transform fatherTransform;

    public void StartViewModel()
    {
        if (block != null)
        {
            IndexUIElement.text = $"Index: {block.Index}";
            TimeStampUIElement.text = $"Timestamp: {block.TimeStamp}";
            PreviousHashUIElement.text = $"Previous Hash: {block.PreviousBlockHash}";
            CurrentHashUIElement.text = $"Current Hash: {block.BlockHash}";

            fatherTransform = GameObject.FindGameObjectWithTag("blockchainUIVisualizer").transform;
            
            gameObject.transform.SetParent(fatherTransform);

            RectTransform rect = gameObject.GetComponent<RectTransform>();

            if (fatherTransform.childCount == 0)
            {
                rect.anchoredPosition = new Vector2(0, 180);
                rect.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                rect.anchoredPosition = new Vector2(0, rect.sizeDelta.y * fatherTransform.childCount);
                rect.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogError("block is null on BlockViewModel");
        }
    }
}
