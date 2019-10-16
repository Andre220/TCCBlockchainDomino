using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockchainVisualizer : MonoBehaviour
{
    public Text blockchainText;

    // Start is called before the first frame update
    void Start()
    {
        BlockchainExportImporter.RetrieveBlockFromBlockchain();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
