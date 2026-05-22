using System.Collections;
using UnityEngine;

public class SmokeDamage : MonoBehaviour
{
    [Header("Pengaturan Asap")]
    public float damageAmount = 5f;           // damage normal per interval
    public float damageInterval = 4f;         // jeda antar damage (detik)

    [Header("Crouch Damage Reduction")]
    // Multiplier damage saat jongkok. 0.5 = damage berkurang 50% saat crouch
    public float crouchDamageMultiplier = 0.5f;

    [Header("Audio")]
    public AudioClip coughClip;

    private AudioSource audioSource;

    // ─── STATIC: dibagi oleh semua instance SmokeDamage ───
    private static int playerSmokeCount = 0;
    private static SmokeDamage activeInstance = null;
    private static Coroutine sharedCoroutine = null;
    // ──────────────────────────────────────────────────────

    void Awake()
    {
        // Buat AudioSource otomatis di object ini
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f;    // suara 2D, bukan 3D
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Ambil HealthSystem dari parent player
        HealthSystem health = other.GetComponentInParent<HealthSystem>();
        if (health == null) return;

        // Ambil PlayerState dari parent player (untuk cek isCrouching)
        PlayerState playerState = other.GetComponentInParent<PlayerState>();

        playerSmokeCount++;

        // Hanya mulai coroutine jika ini zona asap PERTAMA yang dimasuki
        if (playerSmokeCount == 1)
        {
            activeInstance = this;
            // Kirim KEDUA referensi ke coroutine: health dan playerState
            sharedCoroutine = StartCoroutine(DealSmokeDamage(health, playerState));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerSmokeCount = Mathf.Max(0, playerSmokeCount - 1);

        // Hanya stop jika player sudah keluar dari SEMUA zona asap
        if (playerSmokeCount == 0)
        {
            if (activeInstance != null && sharedCoroutine != null)
            {
                activeInstance.StopCoroutine(sharedCoroutine);
                sharedCoroutine = null;
                activeInstance = null;
            }
        }
    }

    IEnumerator DealSmokeDamage(HealthSystem health, PlayerState playerState)
    {
        while (true)
        {
            if (health == null || health.isDead)
            {
                // Bersihkan static variable saat player mati
                playerSmokeCount = 0;
                activeInstance = null;
                sharedCoroutine = null;
                yield break;
            }


            // Hitung damage berdasarkan kondisi crouch
            float actualDamage;

            // Cek apakah playerState valid DAN player sedang jongkok
            if (playerState != null && playerState.isCrouching)
            {
                actualDamage = damageAmount * crouchDamageMultiplier;
            }
            else
            {
                // Damage normal jika tidak jongkok (atau playerState tidak ditemukan)
                actualDamage = damageAmount;
            }

            health.TakeDamage(actualDamage);
            PlayCough();

            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void PlayCough()
    {
        if (coughClip == null || audioSource == null) return;
        if (audioSource.isPlaying) return;
        audioSource.clip = coughClip;
        audioSource.Play();
    }

    void OnDestroy()
    {
        // Reset static variable saat scene berganti
        if (activeInstance == this)
        {
            playerSmokeCount = 0;
            sharedCoroutine = null;
            activeInstance = null;
        }
    }
}