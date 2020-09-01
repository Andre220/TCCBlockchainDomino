using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blockchain
{
    public Action BlockchainSizeChanged;
    public Action TransactionPoolSizeChanged;
    public int Difficcult { get; set; } = 2;
    public int Reward { get; set; } = 1; //Reward to the miner who manage to mine the block

    public List<Transaction> TransactionPool = new List<Transaction>();
    public List<Block> Chain { get; set; }

    public int LastChainSize;
    public int LastPoolSize;
    public Blockchain()
    {
        InitializeChain();
        //Find ou if the machine have a copy of blockchain. If yes, catch it, else create one 

        LastChainSize = Chain.Count;

        BlockchainSizeChanged += OnChainSizeChanged;
        TransactionPoolSizeChanged += OnTransactionPoolSizeChanged;
    }

    public void InitializeChain()
    {
        if (BlockchainExportImporter.RetrieveChainFromBlockchainFile() == null)
        {
            Chain = new List<Block>();
            AddGenesisBlock();
        }
        else
        {
            Chain = BlockchainExportImporter.RetrieveChainFromBlockchainFile();
        }

        BlockchainSizeChanged?.Invoke();
    }

    public void ProcessTransactionPool(string minerAddress)
    {
        //Creating block to be processed
        Block block = new Block(DateTime.Now, GetLastBlock().BlockHash, TransactionPool);

        //Calculating block generation time
        var startTimeCreate = DateTime.Now;

        AddBlock(block);

        var endTimeCreate = DateTime.Now;

        BlockchainExportImporter.InsertBlockIntoBlockchainFile(block);

        Debug.Log($"Duracao = {endTimeCreate - startTimeCreate}");

        //Reset transactionPool
        TransactionPool = new List<Transaction>();

        //Premiando o minerador
        CreateTransaction(new Transaction("blockChainInstance", minerAddress, Reward));
    }

    public void CreateTransaction(Transaction transaction)
    {
        TransactionPool.Add(transaction);
        TransactionPoolSizeChanged?.Invoke();
    }

    public void CreateTransaction(string from, string to, object Data)
    {
        Transaction transactionToBeAdded = new Transaction(from, to, Data);

        TransactionPool.Add(transactionToBeAdded);
        TransactionPoolSizeChanged?.Invoke();
    }

    public Block CreateGenesisBlock()
    {
        return new Block(DateTime.Now, null, new List<Transaction> { });
    }

    public void AddGenesisBlock()
    {
        BlockchainExportImporter.InsertBlockIntoBlockchainFile(CreateGenesisBlock());
        Chain.Add(CreateGenesisBlock());
    }

    public Block GetLastBlock()
    {
        return Chain[Chain.Count - 1];
    }

    public async void AddBlock(Block block)
    {
        Block lastestBlock = GetLastBlock();
        block.Index = lastestBlock.Index + 1;
        block.PreviousBlockHash = lastestBlock.BlockHash;
        block.Mine(this.Difficcult);
        //block.BlockHash = block.CalculateHash();
        Chain.Add(block);
        BlockchainSizeChanged?.Invoke();
    }

    public IList<Transaction> GetBlockTransactionsByBlockID(int ID)
    {
        return Chain[ID].Transactions;
    }

    public IList<Transaction> GetTransactionPoolTransactions()
    {
        return TransactionPool;
    }

    public bool IsValid()
    {
        for (int i = 0; i < Chain.Count - 1; i++)
        {
            Block currentBlock = Chain[i];
            Block previousBlock = Chain[i - 1];

            if (currentBlock.BlockHash != currentBlock.CalculateHash())
            {
                return false;
            }

            if (currentBlock.PreviousBlockHash != previousBlock.BlockHash)
            {
                return false;
            }
        }
        return true;
    }

    public void OnChainSizeChanged()
    {
        UnityEngine.Debug.Log($"Chain changed from {LastChainSize} to {Chain.Count}");
        LastChainSize = Chain.Count;
    }

    public void OnTransactionPoolSizeChanged()
    {
        UnityEngine.Debug.Log($"Transaction Pool changed from {LastPoolSize} to {TransactionPool.Count}");
        LastPoolSize = TransactionPool.Count;
    }
}
