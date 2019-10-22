using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockchainVisualizer : MonoBehaviour
{
    public GameObject blockViewModelPrefab;

    void Start()
    {
        GlobalConfigInfo.blockchain.BlockchainSizeChanged += AddBlocksToUI;
        AddBlocksToUI();
    }

    void AddBlocksToUI()
    {
        foreach (Block b in GlobalConfigInfo.blockchain.Chain)
        {
            GameObject instance = Instantiate(blockViewModelPrefab);
            instance.GetComponent<BlockViewModel>().block = b;
            instance.GetComponent<BlockViewModel>().StartViewModel();
        }
    }
}
