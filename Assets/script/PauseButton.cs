using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (PauseManager.instance == null)
        {
            Debug.LogError("PauseButton: PauseManager tidak ditemukan di scene!");
            return;
        }

        // Tombol yang sama bisa buka DAN tutup pause
        PauseManager.instance
            .OpenPause(); // Panggil fungsi OpenPause di PauseManager
    }
}