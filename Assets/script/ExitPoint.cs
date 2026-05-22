using UnityEngine;

public class ExitPoint : MonoBehaviour
{
    // Pengelompokan visual di Inspector, tidak mempengaruhi logika
    [Header("Identitas Exit")]
    public string exitName = "Exit"; // Nama exit untuk keperluan debug di Console
    public bool isPintu = false;     // True jika exit ini berupa pintu fisik, False jika jendela

    [Header("Referensi Pintu Fisik")]
    public PushDoor linkedDoor; // Drag pintu fisik ke sini, hanya untuk Exit_PintuGarasi dan Exit_PintuDapur

    // ===== EXTENDED: Support untuk GarageDoor =====
    [Header("Referensi GarageDoor (Optional)")]
    public GarageDoor linkedGarageDoor; // Drag GarageDoor ke sini jika exit ini adalah garasi
    // (untuk mengintegrasi dengan GarageDoor animation dan locked state)

    [Header("Status")]
    public bool isLocked = false; // Diset manual di Inspector per skenario

    private SpeechBubble playerSpeechBubble; // Referensi ke SpeechBubble milik player

    void Start()
    {
        // Saat game mulai, terapkan status terkunci sesuai nilai isLocked di Inspector
        SetLocked(isLocked);
    }

    /// <summary>
    /// Set locked state untuk exit ini.
    /// Jika exit adalah pintu fisik (PushDoor atau GarageDoor),
    /// sinkronkan status terkunci ke pintu juga.
    /// 
    /// Parameter:
    /// - locked: true = terkunci, false = terbuka
    /// </summary>
    public void SetLocked(bool locked)
    {
        // Simpan nilai locked ke variabel isLocked
        isLocked = locked;

        // Jika exit ini adalah pintu fisik (PushDoor) DAN pintu fisiknya sudah di-assign,
        // sinkronkan status terkunci ke pintu fisik juga
        if (isPintu && linkedDoor != null)
        {
            linkedDoor.isLocked = locked;
            Debug.Log("[ExitPoint] Status PushDoor '" + exitName + "' -> " + (locked ? "TERKUNCI" : "TERBUKA"));
        }

        // EXTENDED: Jika exit ini adalah GarageDoor DAN linkedGarageDoor sudah di-assign,
        // sinkronkan status terkunci ke GarageDoor juga
        if (linkedGarageDoor != null)
        {
            // GarageDoor saat ini tidak punya isLocked field sendiri,
            // tapi kita bisa tambahkan nanti jika perlu
            // Untuk sekarang, ini hanya untuk future-proofing
            Debug.Log("[ExitPoint] Status GarageDoor '" + exitName + "' -> " + (locked ? "TERKUNCI" : "TERBUKA"));
        }

        // Tampilkan status di Console untuk membantu debugging
        Debug.Log("[ExitPoint] " + exitName + (locked ? " -> TERKUNCI" : " -> TERBUKA"));
    }

    /// <summary>
    /// Toggle locked state (lock ↔ unlock).
    /// Contoh: exitPoint.ToggleLocked(); → toggle status
    /// </summary>
    public void ToggleLocked()
    {
        SetLocked(!isLocked);
    }

    public bool IsAvailable()
    {
        // Mengembalikan true jika exit TIDAK terkunci, false jika terkunci
        // Fungsi ini bisa dipakai script lain untuk mengecek status exit
        return !isLocked;
    }

    /// <summary>
    /// OnTriggerEnter dipanggil saat player masuk ke trigger area exit.
    /// Logic:
    /// 1. Cek apakah player yang masuk
    /// 2. Jika exit TERKUNCI → tampilkan gembok, return (jangan trigger ending)
    /// 3. Jika exit TERBUKA → trigger ending via GameManager
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        // OnTriggerEnter dipanggil otomatis oleh Unity saat ada Collider lain masuk ke trigger ini
        // Parameter 'other' adalah Collider yang masuk

        // Jika yang masuk BUKAN player, abaikan sepenuhnya
        if (!other.CompareTag("Player")) return;

        // Jika exit terkunci
        if (isLocked)
        {
            Debug.Log("[ExitPoint] " + exitName + ": Terkunci!");

            // Cari komponen SpeechBubble di parent hierarchy dari collider player
            // Menggunakan GetComponentInParent karena Collider bisa ada di child object,
            // sedangkan SpeechBubble.cs ada di Player root
            playerSpeechBubble = other.GetComponentInParent<SpeechBubble>();

            if (playerSpeechBubble != null)
                playerSpeechBubble.ShowBubble(); // Panggil tanpa parameter karena sudah pakai sprite
            else
                Debug.LogWarning("[ExitPoint] SpeechBubble tidak ditemukan di parent player!");

            // Hentikan eksekusi di sini, jangan trigger ending
            return;
        }

        // Jika GameManager tidak ditemukan di scene, tampilkan error dan berhenti
        if (GameManager.instance == null)
        {
            Debug.LogError("[ExitPoint] GameManager tidak ditemukan!");
            return;
        }

        // Jika sampai sini, berarti exit tidak terkunci dan GameManager ada
        // Beritahu GameManager bahwa player berhasil keluar
        Debug.Log("[ExitPoint] Player keluar melalui: " + exitName);
        GameManager.instance.OnPlayerExit();
    }
}