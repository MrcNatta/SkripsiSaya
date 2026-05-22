using UnityEngine;
using System.Collections;

/// <summary>
/// Script untuk tombol pembuka pintu garasi.
/// - Outcome-nya diatur di Inspector (true = berhasil, false = gagal)
/// - Jika gagal, tampilkan pop-up gembok 1.5 detik via SpeechBubble
/// - Jika berhasil, unlock pintu garasi via ExitPoint.SetLocked(false)
/// </summary>
public class GarageDoorButton : MonoBehaviour
{
    [Header("=== REFERENSI EXIT POINT ===")]
    // Reference ke ExitPoint component (dari GarageDoor atau pintu manapun)
    // ExitPoint ini akan di-set locked/unlocked oleh button ini
    public ExitPoint garageDoorExitPoint;
    public GarageDoor garageDoor; // Reference ke GarageDoor untuk toggle animasi pintu saat berhasil

    [Header("=== OUTCOME (DIATUR DI INSPECTOR) ===")]
    // TRUE = tombol BERHASIL unlock pintu
    // FALSE = tombol GAGAL, pop-up gembok muncul 1.5 detik
    public bool doorOpenSuccess = true;

    [Header("=== POP-UP GEMBOK SAAT GAGAL ===")]
    // Reference ke SpeechBubble untuk tampilkan gembok saat gagal (1.5 detik)
    // Bisa gunakan SpeechBubble yang sama atau buat baru, sesuai kebutuhan
    public SpeechBubble failLockBubble;

    [Header("=== AUDIO ===")]
    // Sound effect tombol ditekan
    public AudioClip buttonPressSound;
    
    // Sound effect saat unlock gagal
    public AudioClip failSound;
    
    // Sound effect saat unlock berhasil
    public AudioClip successSound;

    // Reference AudioSource
    private AudioSource audioSource;

    // Flag cooldown untuk prevent spam click
    private bool isOnCooldown = false;
    private float cooldownDuration = 0.5f;

    // ===== UNITY LIFECYCLE =====

    /// <summary>
    /// Start: Validasi semua reference penting
    /// </summary>
    private void Start()
    {
        // Cari AudioSource di GameObject ini sendiri
        audioSource = GetComponent<AudioSource>();
        
        // Jika tidak ada, cari di parent
        if (audioSource == null)
        {
            audioSource = GetComponentInParent<AudioSource>();
        }

        // VALIDASI: Pastikan ExitPoint assign di Inspector
        if (garageDoorExitPoint == null)
        {
            Debug.LogError("GarageDoorButton: ASSIGN garageDoorExitPoint (ExitPoint component) di Inspector!");
        }
    }

    /// <summary>
    /// OnTriggerEnter: Deteksi player masuk ke area tombol
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang masuk adalah Player
        if (other.CompareTag("Player"))
        {
            Debug.Log("GarageDoorButton: Player masuk trigger area tombol.");
        }
    }


    /// <summary>
    /// OnTriggerExit: Deteksi player keluar dari area tombol
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        // Cek apakah yang keluar adalah Player
        if (other.CompareTag("Player"))
        {
            Debug.Log("GarageDoorButton: Player keluar trigger area tombol.");
        }
    }

    // ===== MAIN LOGIC =====

    /// <summary>
    /// Fungsi utama: coba unlock pintu garasi.
    /// Logic:
    /// 1. Play sound tombol ditekan
    /// 2. Cek doorOpenSuccess (diatur di Inspector)
    /// 3. Jika TRUE (berhasil) → ExitPoint.SetLocked(false)
    /// 4. Jika FALSE (gagal) → tampilkan pop-up gembok 1.5 detik
    /// 5. Trigger cooldown 0.5 detik untuk prevent spam
    /// </summary>
    public void AttemptToDoor()
    {
        // Play sound: tombol ditekan
        if (audioSource != null && buttonPressSound != null)
        {
            audioSource.PlayOneShot(buttonPressSound);
        }

        // CEK OUTCOME: doorOpenSuccess diatur di Inspector
        if (doorOpenSuccess)
        {
            // ===== BERHASIL =====
            OnDoorSuccess();
        }
        else
        {
            // ===== GAGAL =====
            OnDoorFail();
        }

        // Trigger cooldown: prevent spam click dalam 0.5 detik
        StartCoroutine(CooldownRoutine());
    }

    /// <summary>
    /// Dipanggil saat unlock BERHASIL (doorOpenSuccess = true).
    /// - Play success sound
    /// - Unlock pintu via ExitPoint.SetLocked(false)
    /// </summary>
    private void OnDoorSuccess()
    {
        // Play success sound
        if (audioSource != null && successSound != null)
        {
            audioSource.PlayOneShot(successSound);
        }

        // Unlock pintu: set isLocked = false di ExitPoint
        // Sekarang player bisa sentuh exit point dan keluar
        if (garageDoorExitPoint != null)
        {
            garageDoorExitPoint.SetLocked(false);
        }


        if (garageDoor != null)
        {
            garageDoor.ToggleDoor(); // Toggle door (buka/tutup) saat tombol berhasil
            Debug.Log("GarageDoorButton: ToggleDoor() dipanggil dari GarageDoor.");
        }
        else
        {
            Debug.LogWarning("GarageDoorButton: garageDoor tidak assign di Inspector! Pastikan untuk assign jika ingin toggle pintu.");
        }


        Debug.Log("GarageDoorButton: Unlock BERHASIL! Pintu sekarang bisa dibuka.");
    }

    /// <summary>
    /// Dipanggil saat unlock GAGAL (doorOpenSuccess = false).
    /// - Play fail sound
    /// - Tampilkan pop-up gembok 1.5 detik via SpeechBubble
    /// </summary>
    private void OnDoorFail()
    {
        // Play fail sound
        if (audioSource != null && failSound != null)
        {
            audioSource.PlayOneShot(failSound);
        }

        // Tampilkan pop-up gembok saat gagal
        ShowFailLockBubble();

        Debug.Log("GarageDoorButton: Unlock GAGAL! Pop-up gembok muncul 1.5 detik.");
    }

    /// <summary>
    /// Tampilkan pop-up gembok saat unlock gagal (1.5 detik).
    /// Menggunakan SpeechBubble.ShowBubble() yang sudah ada.
    /// </summary>
    private void ShowFailLockBubble()
    {
        // Validasi: pastikan failLockBubble assign
        if (failLockBubble == null)
        {
            Debug.LogWarning("GarageDoorButton: failLockBubble tidak assign di Inspector!");
            return;
        }

        // Call ShowBubble() dari SpeechBubble
        // SpeechBubble.ShowBubble() otomatis handle display duration (2 detik default)
        // Untuk 1.5 detik, ubah displayDuration di failLockBubble Inspector ke 1.5
        failLockBubble.ShowBubble();

        Debug.Log("GarageDoorButton: Pop-up gembok ditampilkan via SpeechBubble.");
    }

    /// <summary>
    /// Cooldown routine untuk prevent spam click.
    /// Logic:
    /// 1. Set isOnCooldown = true
    /// 2. Wait 0.5 detik
    /// 3. Set isOnCooldown = false
    /// </summary>
    private IEnumerator CooldownRoutine()
    {
        // Set flag cooldown = true (prevent click sementara)
        isOnCooldown = true;

        // Wait selama cooldownDuration detik
        yield return new WaitForSeconds(cooldownDuration);

        // Set flag cooldown = false (sekarang boleh click lagi)
        isOnCooldown = false;

        Debug.Log("GarageDoorButton: Cooldown selesai, boleh click tombol lagi.");
    }
}