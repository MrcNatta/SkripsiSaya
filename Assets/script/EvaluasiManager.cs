using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class KutipanData
{
    [TextArea(2, 4)]
    public string kutipan;  // Isi kutipan
    public string sumber;   // Nama sumber/lembaga
}

public class EvaluasiManager : MonoBehaviour
{
    [Header("Teks UI — Kiri")]
    public TextMeshProUGUI titleText;       // "BERHASIL!" / "GAGAL!"
    public TextMeshProUGUI subtitleText;    // Kalimat ringkas
    public TextMeshProUGUI hpResultText;    // "86%" atau "X"
    public TextMeshProUGUI timeResultText;  // "20.00 Detik"
    public TextMeshProUGUI nilaiText;       // "A" / "B" / "C" / "D" / "E"
    public TextMeshProUGUI edukasiText;     // Bullet points edukasi

    [Header("Teks UI — Kanan")]
    public TextMeshProUGUI kutipanText;     // Isi kutipan yang tampil
    public TextMeshProUGUI sumberText;      // Sumber kutipan yang tampil

    [Header("Pool Kutipan per Nilai — isi di Inspector")]
    public KutipanData[] kutipanA;          // Kutipan untuk nilai A
    public KutipanData[] kutipanB;          // Kutipan untuk nilai B
    public KutipanData[] kutipanC;          // Kutipan untuk nilai C
    public KutipanData[] kutipanD;          // Kutipan untuk nilai D
    public KutipanData[] kutipanE;          // Kutipan untuk nilai E (gagal)

    [Header("Navigasi")]
    public float gameDuration = 120f;           // Durasi permainan dalam detik (2 menit)
    public string mainMenuSceneName = "MainMenu";   // Nama scene Main Menu
    public string level2SceneName = "Level 2A";     // Nama scene Level 2 (default)

    [Header("Tombol Level 2")]
    public Button lanjutLevel2Button;       // Drag ButtonLevel2 ke sini di Inspector

    // ─────────────────────────────────────────────────
    void Start()
    {
        // Tentukan nilai dulu — dipakai oleh semua bagian lain
        string nilai = HitungNilai();

        TampilkanStatistik(nilai);
        TampilkanKontenNilai(nilai);
        TampilkanKutipanRandom(nilai);
        AturWarnaGrade(nilai);
        AturTombolLevel2(nilai);
    }

    // ─────────────────────────────────────────────────
    // HITUNG NILAI dari data GameResult
    // ─────────────────────────────────────────────────
    private string HitungNilai()
    {
        // Ending 3 = gagal total, langsung nilai E
        if (GameResult.endingNumber == 3)
            return "E";

        float hp = GameResult.finalHpPercent;

        if (hp > 80f)      return "A";
        else if (hp > 60f) return "B";
        else if (hp > 30f) return "C";
        else               return "D"; // HP 1–30%
    }

    // ─────────────────────────────────────────────────
    // BAGIAN 1: Statistik HP dan Waktu
    // ─────────────────────────────────────────────────
    private void TampilkanStatistik(string nilai)
    {
        // HP — tampilkan "X" jika gagal
        if (hpResultText != null)
        {
            if (nilai == "E")
                hpResultText.text = "X";
            else
                hpResultText.text = Mathf.RoundToInt(GameResult.finalHpPercent) + "%";
        }

        // Waktu — tampilkan "XX.XX" jika gagal karena timer habis
        if (timeResultText != null)
{
    if (nilai == "E" && GameResult.finalTimeRemaining >= gameDuration)
    {
        // Gagal karena timer habis — waktu evakuasi = durasi penuh
        timeResultText.text = "Waktu habis";
    }
    else
    {
        // Tampilkan waktu yang dipakai untuk evakuasi
        // Format: menit:detik, contoh "01:32"
        int totalDetik = Mathf.RoundToInt(GameResult.finalTimeRemaining);
        int menit  = totalDetik / 60;               // ambil bagian menit
        int detik  = totalDetik % 60;               // ambil sisa detik
        timeResultText.text = string.Format("{0:00}:{1:00}", menit, detik);
    }
}
    }

    // ─────────────────────────────────────────────────
    // BAGIAN 2: Judul, Nilai, Subtitle, Edukasi
    // ─────────────────────────────────────────────────
    private void TampilkanKontenNilai(string nilai)
    {
        // Tampilkan huruf nilai — warna diatur oleh AturWarnaGrade()
        Set(nilaiText, nilai);

        switch (nilai)
        {
            case "A":
                Set(titleText, "BERHASIL!");
                Set(subtitleText, "Kamu berhasil keluar rumah tanpa mengalami cedera.");
                Set(edukasiText,
                    "• Keputusan cepat dan tepat dapat menyelamatkan nyawa saat keadaan darurat.\n" +
                    "• Memilih jalur aman lebih baik daripada mengambil risiko melalui api.\n" +
                    "• Mengetahui jalur keluar dan tetap tenang adalah kunci keberhasilan evakuasi.");
                break;

            case "B":
                Set(titleText, "BERHASIL!");
                Set(subtitleText, "Kamu berhasil keluar, namun menerima luka selama evakuasi.");
                Set(edukasiText,
                    "• Jalur tercepat belum tentu jalur teraman.\n" +
                    "• Keputusan terburu-buru dapat meningkatkan risiko cedera.\n" +
                    "• Kamu selamat hari ini, tetapi keputusan yang lebih baik bisa memberi hasil yang lebih aman.");
                break;

            case "C":
                Set(titleText, "BERHASIL!");
                Set(subtitleText, "Kamu berhasil keluar, namun menerima luka berat selama evakuasi.");
                Set(edukasiText,
                    "• Kamu berhasil keluar, namun kondisi tubuhmu sangat lemah akibat cedera dan paparan bahaya.\n" +
                    "• Jalur berisiko dapat menyebabkan luka yang serius.\n" +
                    "• Api dan asap dapat melukai dengan cepat. Sedikit keterlambatan bisa berakibat fatal.");
                break;

            case "D":
                Set(titleText, "BERHASIL!");
                Set(subtitleText, "Kamu berhasil keluar dalam kondisi kritis akibat api dan asap.");
                Set(edukasiText,
                    "• Paparan api dan asap yang terlalu lama menyebabkan kondisi tubuhmu sangat lemah.\n" +
                    "• Keputusan yang lebih aman sangat dibutuhkan dalam keadaan darurat.\n" +
                    "• Cobalah tetap tenang saat evakuasi demi menghindari cedera fatal.");
                break;

            default: // E
                Set(titleText, "GAGAL!");
                Set(subtitleText, "Kamu gagal keluar dari rumah tepat waktu dan nyawamu tidak selamat.");
                Set(edukasiText,
                    "• Dalam kebakaran, setiap detik sangat berharga.\n" +
                    "• Terlalu lama ragu dapat berakibat fatal.\n" +
                    "• Memilih jalur berbahaya atau terjebak di area tertutup dapat menghambat evakuasi.\n" +
                    "• Asap beracun dapat melumpuhkan lebih cepat daripada yang disadari.");
                break;
        }
    }

    // ─────────────────────────────────────────────────
    // BAGIAN 3: Warna teks nilai sesuai grade
    // ─────────────────────────────────────────────────
    private void AturWarnaGrade(string nilai)
    {
        // Jika nilaiText tidak di-assign di Inspector, skip
        if (nilaiText == null) return;

        switch (nilai)
        {
            case "A":
                nilaiText.color = new Color(0.13f, 0.59f, 0.95f, 1f); // Biru
                break;
            case "B":
                nilaiText.color = new Color(0.18f, 0.80f, 0.44f, 1f); // Hijau
                break;
            case "C":
                nilaiText.color = new Color(1f, 0.84f, 0.0f, 1f);     // Kuning
                break;
            case "D":
                nilaiText.color = new Color(1f, 0.50f, 0.05f, 1f);    // Oranye
                break;
            default: // E
                nilaiText.color = new Color(0.95f, 0.13f, 0.13f, 1f); // Merah
                break;
        }
    }

    // ─────────────────────────────────────────────────
    // BAGIAN 4: Kutipan Random sesuai Nilai
    // ─────────────────────────────────────────────────
    private void TampilkanKutipanRandom(string nilai)
    {
        // Pilih array kutipan yang sesuai dengan nilai
        KutipanData[] pool = nilai switch
        {
            "A" => kutipanA,
            "B" => kutipanB,
            "C" => kutipanC,
            "D" => kutipanD,
            _   => kutipanE,
        };

        // Jika array kosong atau null, tampilkan teks default dan berhenti
        if (pool == null || pool.Length == 0)
        {
            Set(kutipanText, "-");
            Set(sumberText, "-");
            return;
        }

        // Pilih satu kutipan secara acak dari pool
        int index = Random.Range(0, pool.Length);
        Set(kutipanText, pool[index].kutipan);
        Set(sumberText,  pool[index].sumber);
    }

    // ─────────────────────────────────────────────────
    // BAGIAN 5: Atur Tombol Level 2
    // ─────────────────────────────────────────────────
    private void AturTombolLevel2(string nilai)
    {
        // Jika tombol tidak di-assign di Inspector, skip — tidak error
        if (lanjutLevel2Button == null) return;

        // Hanya aktif jika nilai A
        lanjutLevel2Button.interactable = (nilai == "A");
    }

    // ─────────────────────────────────────────────────
    // HELPER — null check terpusat
    // ─────────────────────────────────────────────────
    private void Set(TextMeshProUGUI tmp, string value)
    {
        if (tmp != null) tmp.text = value;
    }

    // ─────────────────────────────────────────────────
    // TOMBOL NAVIGASI
    // ─────────────────────────────────────────────────
    public void OnRetryClicked()
    {
        GameResult.finalHpPercent = 0f;
        GameResult.finalTimeRemaining = 0f;
        GameResult.endingNumber = 0;
        // currentLevel TIDAK direset — dipakai untuk pilih scene yang benar

        if (GameResult.currentLevel == 2)
        {
            // Coba lagi Level 2 — random antara Level 2A/2B/2C
            string[] scenesLevel2 = { "Level 2A", "Level 2B", "Level 2C" };
            int index = Random.Range(0, scenesLevel2.Length);
            LoadingScreenManager.targetScene = scenesLevel2[index];
        }
        else
        {
            // Coba lagi Level 1 — random antara Level 1A/1B/1C
            string[] scenesLevel1 = { "Level 1A", "Level 1B", "Level 1C" };
            int index = Random.Range(0, scenesLevel1.Length);
            LoadingScreenManager.targetScene = scenesLevel1[index];
        }

        SceneManager.LoadScene("LoadingScreen");
    }

    public void OnMenuClicked()
    {
        GameResult.finalHpPercent = 0f;
        GameResult.finalTimeRemaining = 0f;
        GameResult.endingNumber = 0;

        LoadingScreenManager.targetScene = mainMenuSceneName;
        SceneManager.LoadScene("LoadingScreen");
    }

    public void OnLanjutLevel2Clicked()
    {
        GameResult.finalHpPercent = 0f;
        GameResult.finalTimeRemaining = 0f;
        GameResult.endingNumber = 0;

        // Random antara Level 2A/2B/2C
        string[] scenesLevel2 = { "Level 2A", "Level 2B", "Level 2C" };
        int index = Random.Range(0, scenesLevel2.Length);
        LoadingScreenManager.targetScene = scenesLevel2[index];

        SceneManager.LoadScene("LoadingScreen");
    }
}