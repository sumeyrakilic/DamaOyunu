using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject block;
    [SerializeField] GameObject whitePiecePrefab; // Prefab for white pieces
    [SerializeField] GameObject blackPiecePrefab; // Prefab for black pieces
    [SerializeField] GameObject whiteKingPrefab;  // Prefab for white king pieces
    [SerializeField] GameObject blackKingPrefab;  // Prefab for black king pieces

    public static GameManager instance;

    Dictionary<GamePiece, GameObject> pieceDictionary;
    Board myBoard;
    bool hasGameFinished, canMove;
    Player currentPlayer;
    string gameState;
    public GamePiece clickedPiece;
    public delegate void UpdateMessage(Player player, string temp);
    public event UpdateMessage Message;

    public Dictionary<GamePiece, Grid> GetPlayerPositions()
    {
        return myBoard.playerPositions;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SpawnBlocks();
        myBoard = new Board();
        gameState = Constants.CLICK;
        canMove = false;
        hasGameFinished = false;
        currentPlayer = Player.WHİTE;
        pieceDictionary = new Dictionary<GamePiece, GameObject>();

        // Taşların ilk konumlarını oluştur
        SetupPieces();
    }

    void SetupPieces()
    {
        Dictionary<GamePiece, Grid> posGrid = myBoard.playerPositions;
        foreach (KeyValuePair<GamePiece, Grid> pair in posGrid)
        {
            GameObject pieceObject;

            // Instantiate the correct prefab based on the player
            if (pair.Key.player == Player.WHİTE)
                pieceObject = Instantiate(whitePiecePrefab);
            else
                pieceObject = Instantiate(blackPiecePrefab);

            pieceObject.transform.position = new Vector3(pair.Value.x, -pair.Value.y, -2f);
            pieceDictionary[pair.Key] = pieceObject;
        }
    }

    void SpawnBlocks()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject temp = Instantiate(block);
                temp.transform.position = new Vector3(i, -j, -1f);
                temp.GetComponent<SpriteRenderer>().color = (i + j) % 2 == 0 ? Color.black : Color.grey;
            }
        }
    }

    void Update()
    {
        if (hasGameFinished) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Grid clickedGrid = new Grid() { x = (int)(mousePos.x + 0.5), y = (int)-(mousePos.y - 0.5) };

            switch (gameState)
            {
                case Constants.CLICK:
                    canMove = false;
                    clickedPiece = myBoard.GetPieceAtGrid(clickedGrid);

                    if (clickedPiece != null && clickedPiece.player == currentPlayer)
                    {
                        myBoard.CalculateMoves(currentPlayer);
                        var moveDictionary = myBoard.playerMoves;

                        if (moveDictionary.ContainsKey(clickedPiece))
                        {
                            canMove = true;
                            gameState = Constants.MOVE;
                            Message(currentPlayer, Constants.MOVE);
                        }
                    }
                    break;

                case Constants.MOVE:
                    List<Moves> moves = myBoard.playerMoves[clickedPiece];
                    bool canCaptureMore = false;

                    foreach (Moves currentMove in moves)
                    {
                        if (currentMove.end.x == clickedGrid.x && currentMove.end.y == clickedGrid.y)
                        {
                            // Taşın yeni konumunu güncelle
                            pieceDictionary[clickedPiece].transform.position = new Vector3(clickedGrid.x, -clickedGrid.y, -2f);

                            // Yakalama işlemi varsa
                            if (currentMove.isCapture)
                            {
                                // Yakalanan taşı etkisiz hale getir ve listeden kaldır
                                pieceDictionary[currentMove.capturedPiece].SetActive(false);
                                pieceDictionary.Remove(currentMove.capturedPiece);

                                // Başka bir taşı daha yakalayabilir mi?
                                myBoard.CalculateMoves(currentPlayer);
                                var nextMoves = myBoard.playerMoves[clickedPiece];

                                foreach (Moves nextMove in nextMoves)
                                {
                                    // Aynı taşla başka bir yakalama mümkün mü?
                                    if (nextMove.isCapture && nextMove.start.x == currentMove.end.x && nextMove.start.y == currentMove.end.y)
                                    {
                                        canCaptureMore = true;
                                        break;
                                    }
                                }
                            }

                            // Taşın hareketini oyun tahtasında güncelle
                            myBoard.UpdateMove(currentMove);

                            // Piyonun krala dönüşme durumu kontrolü
                            if (currentMove.end.y == 7 && currentPlayer == Player.WHİTE)
                            {
                                myBoard.UpgradePiece(clickedPiece);
                                pieceDictionary[clickedPiece].SetActive(false);
                                GameObject kingObject = Instantiate(whiteKingPrefab, pieceDictionary[clickedPiece].transform.position, Quaternion.identity);
                                pieceDictionary[clickedPiece] = kingObject;
                            }

                            if (currentMove.end.y == 0 && currentPlayer == Player.BLACK)
                            {
                                myBoard.UpgradePiece(clickedPiece);
                                pieceDictionary[clickedPiece].SetActive(false);
                                GameObject kingObject = Instantiate(blackKingPrefab, pieceDictionary[clickedPiece].transform.position, Quaternion.identity);
                                pieceDictionary[clickedPiece] = kingObject;
                            }

                            // Oyun durumunu güncelle (çoklu yakalama varsa MOVE, yoksa CLICK)
                            gameState = canCaptureMore ? Constants.MOVE : Constants.CLICK;
                            myBoard.CalculateMoves(currentPlayer);

                            // Eğer devam edilecek başka bir hamle yoksa oyuncu değiştir ve CLICK mesajı gönder
                            if (!canCaptureMore || !myBoard.isCapturedMoves)
                            {
                                currentPlayer = currentPlayer == Player.WHİTE ? Player.BLACK : Player.WHİTE;
                                Message(currentPlayer, Constants.CLICK);
                            }
                            else
                            {
                                // Çoklu yakalama durumunda mesajı gönder
                                Message(currentPlayer, Constants.MULTICAPTURE);
                            }

                            return;
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }

    public void AnaMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

