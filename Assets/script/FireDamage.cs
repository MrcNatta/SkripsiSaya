using System.Collections;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    [Header("Fire Settings")]
    public float damageAmount = 4f;
    public float damageInterval = 0.1f;

    [Header("Audio")]
    public AudioClip fireLoopClip;
    public AudioClip screamClip;

    // BARU: Referensi ke efek layar merah
    [Header("Screen Effect")]
    public FireScreenEffect fireScreenEffect;
                                            // Drag FireDamageOverlay ke sini di Inspector
                                            // dari setiap FirePoint object

    private AudioSource fireAudioSource;
    private AudioSource screamAudioSource;
    private Coroutine fireDamageCoroutine;

    void Awake()
    {
        fireAudioSource = gameObject.AddComponent<AudioSource>();
        fireAudioSource.loop = true;
        fireAudioSource.spatialBlend = 1f;
        fireAudioSource.maxDistance = 10f;
        fireAudioSource.rolloffMode = AudioRolloffMode.Linear;
        fireAudioSource.playOnAwake = false;

        screamAudioSource = gameObject.AddComponent<AudioSource>();
        screamAudioSource.loop = false;
        screamAudioSource.spatialBlend = 0f;
        screamAudioSource.playOnAwake = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        HealthSystem health = other.GetComponentInParent<HealthSystem>();
        if (health == null) return;

        if (fireLoopClip != null && !fireAudioSource.isPlaying)
        {
            fireAudioSource.clip = fireLoopClip;
            fireAudioSource.Play();
        }

        PlayScream();

        // BARU: Aktifkan efek layar merah
        if (fireScreenEffect != null)
            fireScreenEffect.StartEffect();

        fireDamageCoroutine = StartCoroutine(DealDamage(health));
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        fireAudioSource.Stop();

        // BARU: Hentikan efek layar merah (fade out perlahan)
        if (fireScreenEffect != null)
            fireScreenEffect.StopEffect();

        if (fireDamageCoroutine != null)
        {
            StopCoroutine(fireDamageCoroutine);
            fireDamageCoroutine = null;
        }
    }

    private void PlayScream()
    {
        if (screamClip == null) return;
        if (screamAudioSource.isPlaying) return;
        screamAudioSource.clip = screamClip;
        screamAudioSource.Play();
    }

    IEnumerator DealDamage(HealthSystem health)
    {
        while (true)
        {
            if (health.isDead)
            {
                fireAudioSource.Stop();

                // BARU: Matikan efek layar saat player mati
                if (fireScreenEffect != null)
                    fireScreenEffect.StopEffect();

                yield break;
            }

            health.TakeDamage(damageAmount);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}