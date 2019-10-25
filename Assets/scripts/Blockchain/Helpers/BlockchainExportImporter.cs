using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using System;

public class BlockchainExportImporter : MonoBehaviour
{
    public static List<Block> RetrieveChainFromBlockchainFile()
    {
        string blockchainJson = "";

        try
        {
            using (StreamReader sr = File.OpenText(getPath()))
            {
                blockchainJson = sr.ReadToEnd();
            }
        }
        catch (FileNotFoundException)
        {
            using (StreamWriter sw = File.CreateText(getPath()))
            {
                sw.WriteLine(blockchainJson);
            }

            return null;
        }

        return JsonConvert.DeserializeObject<List<Block>>(blockchainJson);
    }

    public static void InsertBlockIntoBlockchainFile(Block block)
    {
        IList<Block> blockchain;

        if (RetrieveChainFromBlockchainFile() == null)
        {
            blockchain = new List<Block>();
        }
        else
        {
            blockchain = RetrieveChainFromBlockchainFile();
        }

        blockchain.Add(block);

        string blockchainJson = JsonConvert.SerializeObject(blockchain);

        if (!File.Exists(getPath()))
        {
            using (StreamWriter sw = File.CreateText(getPath()))
            {
                sw.WriteLine(blockchainJson);
            }
        }
        else
        {
            using (StreamWriter sw = new StreamWriter(getPath(), false))
            {
                sw.WriteLine(blockchainJson);
            }
        }
    }

    private static string getPath()
    {
        if (Application.isEditor)
        {
            return "Assets/Resources/blockchainJson.txt";
        }
        else 
        {
            return Application.dataPath + "/Resources/blockchainJson.txt";
        }
    }
}
