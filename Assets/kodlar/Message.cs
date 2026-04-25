using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class Message : MonoBehaviour
{
    private TMP_Text myText; // TMP_Text türünden değişken

    private void Start()
    {
        myText = GetComponent<TMP_Text>(); // TextMeshProUGUI component'ini al
        GameManager.instance.Message += UpdateMessage;

        // Oyun başladığında varsayılan mesajı ayarla
        SetMessage("Click a Piece", Player.WHİTE); // Başlangıçta beyaz oyuncunun sırası olduğu varsayımıyla ayarlandı
    }

    public void UpdateMessage(Player player, string messageText)
    {
        // Mesaj güncelleme işlemleri
        switch (messageText)
        {
            case Constants.CLICK:
                SetMessage(player == Player.WHİTE ? "White turn: Click a Piece" : "Black turn: Click a Piece", player);
                break;

            case Constants.MOVE:
                SetMessage(player == Player.WHİTE ? "White turn: Move the Piece" : "Black turn: Move the Piece", player);
                break;

            /*case Constants.FINISHED:
                SetMessage(player == Player.WHİTE ? "Black Wins" : "White Wins", player);
                break;*/

            default:
                break;
        }
    }

    // Mesajı ayarlamak için yardımcı metod
    private void SetMessage(string message, Player player)
    {
        if (myText == null)
        {
            Debug.LogError("TMP_Text component is not found on Message object.");
            return;
        }

        // Oyuncu rengine göre mesaj rengini ayarla
        myText.text = message;
        myText.color = player == Player.WHİTE ? Color.gray : Color.black;

        // GameManager üzerinden clickedPiece'e eriş ve mesajda göster
        if (GameManager.instance.clickedPiece != null)
        {
            myText.text += "\n" + GameManager.instance.clickedPiece.pieceType;
        }
    }
}

