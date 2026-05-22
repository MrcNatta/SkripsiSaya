using UnityEngine;
using UnityEngine.UI;
using IEnumerator = System.Collections.IEnumerator;

/// <summary>
/// Script untuk control UI Button tombol garasi.
/// - Tampilkan/sembunyikan button saat player masuk/keluar trigger radius
/// - Trigger OnClick event ke GarageDoorButton saat player click button
/// - REUSE sistem button yang sudah ada (seperti PauseButton, CrouchButton)
/// </summary>
public class GarageDoorButtonUI : MonoBehaviour
{
    [Header("=== REFERENSI ===")]
    // Reference ke Button UI component (di GameObject ini)
    public Button garageDoorButton;

    // Reference ke GarageDoorButton script (yang handle logic unlock/fail)
    public GarageDoorButton garageDoorButtonLogic;

    [Header("=== TRIGGER AREA ===")]
    // Reference ke trigger collider yang akan detect player masuk/keluar
    // (sama trigger dengan GarageDoorButton world interaction)
    public Collider triggerArea;

    // Flag untuk track apakah player dalam trigger radius
    private bool isPlayerInRange = false;

    // ===== UNITY LIFECYCLE =====

    /// <summary>
    /// Start: Setup button onClick listener
    /// </summary>
    private void Start()
    {
        // Validasi: pastikan Button component ada
        if (garageDoorButton == null)
        {
            garageDoorButton = GetComponent<Button>();
            if (garageDoorButton == null)
            {
                Debug.LogError("GarageDoorButtonUI: Button component tidak ditemukan!");
                return;
            }
        }

        // Validasi: pastikan GarageDoorButton logic di-assign
        if (garageDoorButtonLogic == null)
        {
            Debug.LogError("GarageDoorButtonUI: Assign garageDoorButtonLogic di Inspector!");
        }

        // Setup button onClick listener
        // Saat player click button UI, panggil AttemptToDoor() dari GarageDoorButton script
        if (garageDoorButton != null)
        {
            garageDoorButton.onClick.AddListener(OnButtonClicked);
        }

        // Sembunyikan button saat game mulai (akan ditampilkan saat player dalam range)
        if (garageDoorButton != null)
        {
            garageDoorButton.gameObject.SetActive(false);
        }

        Debug.Log("GarageDoorButtonUI: Initialized. Button hidden by default.");
    }

    /// <summary>
    /// OnTriggerEnter: Deteksi player masuk ke trigger area
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang masuk adalah Player
        if (other.CompareTag("Player"))
        {
            // Set flag player dalam range
            isPlayerInRange = true;

            // Tampilkan UI button
            ShowButton();

            Debug.Log("GarageDoorButtonUI: Player dalam range. Button ditampilkan.");
        }
    }

    /// <summary>
    /// OnTriggerExit: Deteksi player keluar dari trigger area
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        // Cek apakah yang keluar adalah Player
        if (other.CompareTag("Player"))
        {
            // Set flag player tidak dalam range
            isPlayerInRange = false;

            // Sembunyikan UI button
            HideButton();

            Debug.Log("GarageDoorButtonUI: Player keluar range. Button disembunyikan.");
        }
    }

    // ===== BUTTON LOGIC =====

    /// <summary>
    /// Tampilkan UI button
    /// </summary>
    private void ShowButton()
{
    if (garageDoorButton != null)
    {
        // Delay 1 frame sebelum set active
        // Ini memastikan Canvas sudah siap sebelum button ditampilkan
        StartCoroutine(ShowButtonDelayed());
    }
}

/// <summary>
/// Coroutine: tampilkan button setelah 1 frame delay
/// </summary>
private IEnumerator ShowButtonDelayed()
{
    // Wait 1 frame
    yield return null;
    
    // Sekarang set active true
    if (garageDoorButton != null)
    {
        garageDoorButton.gameObject.SetActive(true);
        Debug.Log("✅ Button ditampilkan (setelah delay 1 frame).");
    }
}

    /// <summary>
    /// Sembunyikan UI button
    /// </summary>
    private void HideButton()
    {
        if (garageDoorButton != null)
        {
            garageDoorButton.gameObject.SetActive(false);
        }
    }

    /// <summary>
/// Dipanggil saat player click UI button
/// - Trigger AttemptToDoor() dari GarageDoorButton script
/// </summary>
private void OnButtonClicked()
{
    Debug.Log("🔵 OnButtonClicked() CALLED!");

    // Validasi: pastikan GarageDoorButton logic ada
    if (garageDoorButtonLogic == null)
    {
        Debug.LogError("GarageDoorButtonUI: garageDoorButtonLogic tidak assign!");
        return;
    }

    // HAPUS validasi isPlayerInRange — tidak perlu di-check saat click UI button
    // (validasi hanya perlu di OnTriggerEnter untuk show/hide button)
    
    // Panggil public method AttemptToDoor() dari GarageDoorButton
    garageDoorButtonLogic.AttemptToDoor();

    Debug.Log("✅ OnButtonClicked() -> AttemptToDoor() dipanggil!");
}
}