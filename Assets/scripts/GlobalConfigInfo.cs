using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalConfigInfo
{
    public static NodeState currentState;

    public static NodeInfo ThisNode;
    
    public static ConnectionInfo CurrentAdversary;

    public static bool MyTurn = false;

    public static PlayingIdentifier playingIdentifier = PlayingIdentifier.unknown;
    
    public static PlayingIdentifier currentPlayerTurn = PlayingIdentifier.sender;//First move always from the sender.

    public static int movesCount = 0;

    public static ushort GlobalPacketSize { get; set; } = 2000;

    public static ushort GlobalFragmentSize { get; set; } = 665;

    public static Blockchain blockchain;

    public static IClient nodeClient;
    public static IServer nodeServer;
}
