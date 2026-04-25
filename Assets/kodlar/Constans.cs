using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player { WHİTE, BLACK };

public struct Grid { public int x, y; internal Color color; private int v1; private int v2; public Grid(int v1, int v2, Color color) : this() { this.v1 = v1; this.v2 = v2; this.color = color; } };

public struct Moves { public Grid start, end; public bool isCapture; public GamePiece capturedPiece; }


public static class Constants
{
    public const string CLICK = "CLICK";
    public const string MOVE = "MOVE";
    public const string FINISHED = "FINISHED";
    public const string NORMAL_PIECE = "NORMAL";
    public const string KING_PIECE = "KING";
    public const string MULTICAPTURE = "MULTICAPTURE";

}
