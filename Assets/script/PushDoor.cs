using System.Collections;
using UnityEngine;

public class PushDoor : MonoBehaviour
{
    [Header("Door Settings")]
    public float openAngle = 90f;
    public float openSpeed = 3f;
    public bool isLocked = false;

    [Header("Push Detection")]
    public float pushThreshold = 0.1f;

    [Header("Auto Close")]
    public bool autoClose = true;
    public float autoCloseDelay = 2f;
    public float closeSpeed = 2f;

    [Header("Audio")]
    public AudioClip doorAudioClip;         // OpenandCloseDoor.mp3 — dipakai untuk buka & tutup

    private AudioSource audioSource;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false;
    private bool isAnimating = false;
    private bool playerInTrigger = false;
    private int pushDirection = 1;
    private Coroutine autoCloseCoroutine;
    private Coroutine animateCoroutine;

    void Awake()
    {
        // Buat AudioSource di Awake supaya siap sebelum Start() jalan
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f;      // 2D sound — pasti terdengar dari mana saja
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Start()
    {
        closedRotation = transform.rotation;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (isLocked || isOpen || isAnimating) return;

        var charController = other.GetComponent<CharacterController>();
        if (charController == null) return;

        float speed = new Vector3(
            charController.velocity.x, 0, charController.velocity.z
        ).magnitude;

        if (speed > pushThreshold)
        {
            pushDirection = DeterminePushDirection(other.transform.position);
            OpenDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInTrigger = true;

        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
            autoCloseCoroutine = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        HandlePlayerExit();
    }

    private void Update()
    {
        if (!playerInTrigger || !isOpen || !autoClose) return;

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        Collider col = GetComponent<Collider>();
        if (col == null) return;

        Vector3 closestPoint = col.ClosestPoint(player.transform.position);
        float dist = Vector3.Distance(closestPoint, player.transform.position);

        if (dist > 0.05f)
            HandlePlayerExit();
    }

    private void HandlePlayerExit()
    {
        if (!playerInTrigger) return;

        playerInTrigger = false;

        if (autoClose && isOpen)
            autoCloseCoroutine = StartCoroutine(AutoCloseCountdown());
    }

    private int DeterminePushDirection(Vector3 playerPosition)
    {
        Vector3 toPlayer = playerPosition - transform.position;
        toPlayer.y = 0;
        float dot = Vector3.Dot(transform.forward, toPlayer);
        return dot >= 0 ? 1 : -1;
    }

    public void OpenDoor()
    {
        if (isOpen || isAnimating) return;

        // Mainkan suara buka pintu
        PlayDoorSound();

        openRotation = closedRotation * Quaternion.Euler(0, openAngle * pushDirection, 0);
        animateCoroutine = StartCoroutine(AnimateDoor(openRotation, openSpeed, () => isOpen = true));
        isOpen = true;
    }

    public void CloseDoor()
    {
        if (!isOpen || isAnimating) return;

        // Tidak ada suara saat tutup — file OpenandCloseDoor.mp3 sudah berisi keduanya
        // sehingga suara tutup sudah terdengar dari clip yang diputar saat buka

        if (animateCoroutine != null)
        {
            StopCoroutine(animateCoroutine);
            isAnimating = false;
        }

        animateCoroutine = StartCoroutine(AnimateDoor(closedRotation, closeSpeed, () => isOpen = false));
        isOpen = false;
    }

    private void PlayDoorSound()
    {
        if (doorAudioClip == null) return;
        if (audioSource == null) return;

        audioSource.clip = doorAudioClip;
        audioSource.Play();                 // Tidak loop, satu kali putar per buka/tutup
    }

    private IEnumerator AnimateDoor(Quaternion targetRotation, float speed, System.Action onComplete)
    {
        isAnimating = true;
        float timeout = 5f;
        float elapsed = 0f;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.5f)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * speed
            );

            elapsed += Time.deltaTime;
            if (elapsed >= timeout) break;

            yield return null;
        }

        transform.rotation = targetRotation;
        isAnimating = false;
        onComplete?.Invoke();
    }

    private IEnumerator AutoCloseCountdown()
    {
        yield return new WaitForSeconds(autoCloseDelay);

        if (!playerInTrigger && isOpen)
            CloseDoor();

        autoCloseCoroutine = null;
    }
}