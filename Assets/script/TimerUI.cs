using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text timerText;              // Drag TimerText ke sini

    [Header("Audio")]
    public AudioClip timerWarningClip;      // Timer10sec.mp3
    private AudioSource audioSource;
    private bool warningPlayed = false;     // Flag agar suara hanya main sekali

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f;      // 2D sound — suara warning tidak perlu 3D
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Update()
    {
        if (GameManager.instance == null) return;

        float t = GameManager.instance.GetTimeRemaining();

        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);

        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);

        // Warna merah saat sisa waktu <= 30 detik
        timerText.color = t <= 30f ? Color.red : Color.white;

        // Mainkan suara warning SEKALI saat timer menyentuh 10 detik
        if (t <= 10f && !warningPlayed)
        {
            warningPlayed = true;
            PlayWarning();
        }

        // Reset flag jika timer di-reset (misal level diulang)
        if (t > 10f)
            warningPlayed = false;
    }

    private void PlayWarning()
    {
        if (timerWarningClip == null) return;

        audioSource.clip = timerWarningClip;
        audioSource.Play();
    }
}