using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public string evaluasiSceneName = "Evaluasi";

    public void OnNextClicked()
    {
        // GameResult.endingNumber sudah tersimpan dari GameManager
        // Tidak perlu set lagi di sini
        LoadingScreenManager.targetScene = evaluasiSceneName;
        SceneManager.LoadScene("LoadingScreen");
    }
}