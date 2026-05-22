using UnityEngine;
using System.Collections;

/// <summary>
/// Script untuk pintu garasi yang bisa buka/tutup.
/// - Animate pintu dari posisi tutup ke posisi buka
/// - Bisa di-toggle (buka ↔ tutup) berkali-kali
/// - Open/close animation bisa diatur durasi dan jenis
/// </summary>
public class GarageDoor : MonoBehaviour
{
    [Header("=== POSISI PINTU ===")]
    // Posisi pintu saat TERTUTUP (posisi awal, di Inspector set ke posisi tutup)
    private Vector3 closedPosition;

    // Posisi pintu saat TERBUKA (set di Inspector)
    public Vector3 openPosition = new Vector3(0, 0, 5f);

    [Header("=== ANIMATION ===")]
    // Durasi animasi buka/tutup dalam detik
    public float animationDuration = 1f;

    [Header("=== AUDIO ===")]
    // Sound effect saat pintu mulai bergerak
    public AudioClip doorMoveSound;

    // Reference AudioSource
    private AudioSource audioSource;

    // Flag untuk tracking state pintu (true = terbuka, false = tertutup)
    private bool isDoorOpen = false;

    // Flag untuk prevent multiple simultaneous animations
    private bool isAnimating = false;

    // ===== UNITY LIFECYCLE =====

    private void Start()
    {
        // Simpan posisi awal sebagai closedPosition
        closedPosition = transform.position;

        // Ambil AudioSource dari GameObject ini
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = GetComponentInParent<AudioSource>();
        }

        Debug.Log($"GarageDoor: Closed position = {closedPosition}, Open position = {openPosition}");
    }

    // ===== PUBLIC METHODS =====

    /// <summary>
    /// Toggle pintu: jika tertutup → buka, jika terbuka → tutup
    /// </summary>
    public void ToggleDoor()
    {
        // Jika sedang animate, jangan mulai animation baru
        if (isAnimating)
        {
            Debug.LogWarning("GarageDoor: Pintu sedang bergerak, tunggu selesai!");
            return;
        }

        // Jika pintu terbuka, tutup
        if (isDoorOpen)
        {
            CloseDoor();
        }
        else
        {
            // Jika pintu tertutup, buka
            OpenDoor();
        }
    }

    /// <summary>
    /// Buka pintu garasi dengan smooth animation
    /// </summary>
    private void OpenDoor()
    {
        // Mulai coroutine untuk animate pintu
        StartCoroutine(AnimateDoor(closedPosition, openPosition, true));
    }

    /// <summary>
    /// Tutup pintu garasi dengan smooth animation
    /// </summary>
    private void CloseDoor()
    {
        // Mulai coroutine untuk animate pintu
        StartCoroutine(AnimateDoor(openPosition, closedPosition, false));
    }

    // ===== PRIVATE ANIMATION LOGIC =====

    /// <summary>
    /// Coroutine untuk animate pintu dari startPos ke endPos
    /// - Menggunakan Vector3.Lerp untuk smooth movement
    /// - Durasi: animationDuration (diatur di Inspector)
    /// 
    /// Parameter:
    /// - startPos: posisi awal
    /// - endPos: posisi akhir
    /// - opening: true jika sedang buka, false jika tutup (untuk debug log)
    /// </summary>
    private IEnumerator AnimateDoor(Vector3 startPos, Vector3 endPos, bool opening)
    {
        // Set flag bahwa pintu sedang animate
        isAnimating = true;

        // Play door move sound
        if (audioSource != null && doorMoveSound != null)
        {
            audioSource.PlayOneShot(doorMoveSound);
        }

        // Track waktu elapsed untuk Lerp
        float elapsedTime = 0f;

        // Loop selama elapsedTime < animationDuration
        while (elapsedTime < animationDuration)
        {
            // Increment elapsed time setiap frame
            elapsedTime += Time.deltaTime;

            // Calculate normalized time (0 to 1)
            // t=0 → posisi awal, t=1 → posisi akhir
            float t = elapsedTime / animationDuration;

            // Lerp posisi dari startPos ke endPos
            transform.position = Vector3.Lerp(startPos, endPos, t);

            // Yield sampai frame berikutnya
            yield return null;
        }

        // Set posisi akhir (ensure akurat, jangan ada floating point error)
        transform.position = endPos;

        // Update state
        isDoorOpen = opening;

        // Set flag animating = false, sekarang boleh animate lagi
        isAnimating = false;

        // Debug log
        if (opening)
        {
            Debug.Log("GarageDoor: Pintu TERBUKA!");
        }
        else
        {
            Debug.Log("GarageDoor: Pintu TERTUTUP!");
        }
    }
}