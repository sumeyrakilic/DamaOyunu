using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class WinManager : MonoBehaviour
{
    [SerializeField] GameObject winPanel; // Kazananı gösteren panel
    [SerializeField] TMP_Text winText; // Paneldeki text bileşeni
    [SerializeField] GameManager gameManager; // GameManager referansı

    private void Awake()
    {
        winPanel.SetActive(false); // Oyunun başında paneli kapalı tut
    }

    private void Update()
    {
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        int redPieceCount = 0;
        int bluePieceCount = 0;

        // GameManager'dan tüm taşları al ve say
        Dictionary<GamePiece, Grid> pieces = gameManager.GetPlayerPositions();
        foreach (var piece in pieces.Keys)
        {
            if (piece.player == Player.WHİTE)
            {
                redPieceCount++;
            }
            else if (piece.player == Player.BLACK)
            {
                bluePieceCount++;
            }
        }

        // Eğer herhangi bir renk taş kalmadıysa kazananı belirle
        if (redPieceCount == 0 || bluePieceCount == 0)
        {
            Player winner = redPieceCount == 0 ? Player.BLACK : Player.WHİTE;
            ShowWinPanel(winner);
        }
    }

    public void ShowWinPanel(Player winner)
    {
        winPanel.SetActive(true);
        winText.text = winner == Player.WHİTE ? "WHITE WINS!" : "BLACK WINS!";

        // Yazı stilini ayarlama
        winText.fontSize = 100; // Punto boyutunu ayarlayın
        winText.alignment = TextAlignmentOptions.Center; // Ortaya hizalama

        // Yazı konumunu ayarlama
        RectTransform rectTransform = winText.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 0); // Ortaya konumlandırma
    }
}
