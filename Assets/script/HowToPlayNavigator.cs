using UnityEngine;

public class HowToPlayNavigator : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;

    // BARU: referensi ke PausePanel
    // Drag PausePanel dari Hierarchy ke field ini di Inspector
    public GameObject pausePanel;

    public void OnNextClicked()
    {
        panel1.SetActive(false);
        panel2.SetActive(true);
    }

    public void OnPreviousClicked()
    {
        panel2.SetActive(false);
        panel1.SetActive(true);
    }

    // BARU: dipanggil oleh BtnX di Panel 1 DAN Panel 2
    public void OnCloseClicked()
    {
        // Sembunyikan kedua panel HowToPlay
        panel1.SetActive(false);
        panel2.SetActive(false);

        // Tampilkan kembali PausePanel
        if (pausePanel != null)
            pausePanel.SetActive(true);
    }
}