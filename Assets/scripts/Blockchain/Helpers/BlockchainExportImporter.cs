using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;

public class BlockchainExportImporter : MonoBehaviour
{
    static string path = "Assets/Resources/blockchainJson.txt";

    [MenuItem("Tools/Write file")]
    public static void InsertBlockIntoBlockchain(Block block)
    {
        string blockJson = JsonConvert.SerializeObject(block);

        if (!File.Exists(path))
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(blockJson);
            }
        }
        else
        {
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine(blockJson);
            }
        }
    }

    public static Block RetrieveBlockFromBlockchain()
    {
        string blockchainJson = "";

        using (StreamReader sr = File.OpenText(path))
        {
            blockchainJson = sr.ReadToEnd();
        }

        return JsonConvert.DeserializeObject<Block>(blockchainJson);
    }
}
