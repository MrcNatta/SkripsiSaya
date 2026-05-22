using UnityEngine;
using UnityEngine.UI;

public class WindowInteract : MonoBehaviour
{
    [Header("Titik Teleport")]
    public Transform teleportTarget;
    // Drag Empty GameObject di sisi seberang jendela ke sini

    [Header("Tombol UI")]
    public GameObject interactButtonUI;
    // Drag GameObject tombol "Lewati Jendela" ke sini

    // Menyimpan referensi player yang sedang di dekat jendela
    private GameObject playerObject;

    // ─────────────────────────────────────────────────
    // Saat player masuk area jendela — tampilkan tombol
    // ─────────────────────────────────────────────────
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Simpan referensi player
        playerObject = other.transform.root.gameObject;

        // Tampilkan tombol interaksi
        if (interactButtonUI != null)
            interactButtonUI.SetActive(true);
    }

    // ─────────────────────────────────────────────────
    // Saat player keluar area jendela — sembunyikan tombol
    // ─────────────────────────────────────────────────
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Sembunyikan tombol
        if (interactButtonUI != null)
            interactButtonUI.SetActive(false);

        // Hapus referensi player
        playerObject = null;
    }

    // ─────────────────────────────────────────────────
    // Dipanggil oleh tombol "Lewati Jendela" saat ditekan
    // ─────────────────────────────────────────────────
    public void OnInteractClicked()
    {
        // Jika player tidak ada, skip
        if (playerObject == null) return;

        // Jika target teleport tidak di-assign, skip
        if (teleportTarget == null)
        {
            Debug.LogWarning("[WindowInteract] teleportTarget belum di-assign!");
            return;
        }

        // Ambil CharacterController dari player
        // Harus dimatikan dulu sebelum pindah posisi
        // Kalau tidak, CharacterController akan melawan perpindahan
        CharacterController cc = playerObject.GetComponentInChildren<CharacterController>();

        if (cc != null)
        {
            cc.enabled = false;                         // Matikan sementara
            playerObject.transform.position = teleportTarget.position; // Pindahkan
            cc.enabled = true;                          // Nyalakan kembali
        }
        else
        {
            playerObject.transform.position = teleportTarget.position;
        }

        // Sembunyikan tombol setelah teleport
        if (interactButtonUI != null)
            interactButtonUI.SetActive(false);

        playerObject = null;

        Debug.Log("[WindowInteract] Player melewati jendela.");
    }
}