using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreenManager : MonoBehaviour
{
    [Header("Referensi UI")]
    public Slider loadingSlider; // Drag LoadingSlider ke sini

    // Variabel static — diisi oleh script lain sebelum pindah ke LoadingScreen
    public static string targetScene = "";

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(targetScene);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;

            if (operation.progress >= 0.9f)
            {
                loadingSlider.value = 1f;
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}