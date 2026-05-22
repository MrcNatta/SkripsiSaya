using UnityEngine;
using System.Collections;

public class SpeechBubble : MonoBehaviour
{
    [Header("Referensi")]
    public GameObject bubbleObject; // Drag LockedIcon (Image) ke sini

    [Header("Pengaturan")]
    public float displayDuration = 2f; // Berapa detik icon tampil

    private Coroutine hideCoroutine; // Menyimpan coroutine agar bisa di-reset jika dipanggil ulang

    void Start()
    {
        // Sembunyikan icon saat game mulai
        if (bubbleObject != null)
            bubbleObject.SetActive(false);
    }

    // Update() dihapus total — tidak perlu lagi karena icon tidak perlu menghadap kamera

    public void ShowBubble()
    {
        // Jika bubbleObject tidak di-assign di Inspector, tidak lakukan apapun
        if (bubbleObject == null) return;

        // Tampilkan icon
        bubbleObject.SetActive(true);

        // Jika icon sudah tampil dan dipanggil lagi (player menyentuh pintu berkali-kali),
        // hentikan timer lama dan mulai timer baru agar tidak langsung menghilang
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        // Tunggu selama displayDuration detik
        yield return new WaitForSeconds(displayDuration);

        // Sembunyikan icon setelah waktu habis
        if (bubbleObject != null)
            bubbleObject.SetActive(false);
    }
}