using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager'ı kullanmak için bu satır eklenmeli

public class AnaMenu : MonoBehaviour
{
    public GameObject panel; // Unity Editörü'nde bağlantı yapacağınız panel nesnesi

    public void Startbutton()
    {
        // Oyun başladığında "Gameplay" sahnesini yükle
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        // Oyundan çıkış yap
        Application.Quit();
    }

    public void nasilOynanir()
    {
        // "Nasıl Oynanır" butonuna basıldığında paneli etkinleştir
        panel.SetActive(true);
    }

    public void panelKapat()
    {
        // Panel kapatma fonksiyonu
        panel.SetActive(false);
    }
}
