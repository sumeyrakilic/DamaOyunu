using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;

using UnityEngine;

public class GamePiece
{
    public Player player { get; private set; }
    public bool isKing; // Taşın kral olup olmadığını belirten değişken
    public int pieceNumber { get; private set; }
    public string pieceType;

    public GamePiece(Player tempPlayer, int temp)
    {
        player = tempPlayer;
        pieceNumber = temp;
        pieceType = Constants.NORMAL_PIECE;
    }

}
