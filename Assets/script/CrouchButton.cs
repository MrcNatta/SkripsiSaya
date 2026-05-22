using UnityEngine;
using UnityEngine.UI;
using StarterAssets;                    // Baris 3: Namespace milik Starter Assets
                                        // Diperlukan agar kita bisa akses ThirdPersonController

public class CrouchButton : MonoBehaviour
{
    [Header("Referensi")]
    public PlayerState playerState;     // Drag object Player ke sini di Inspector
    public Transform playerTransform;   // Drag object PlayerArmature ke sini di Inspector

    // BARU: Referensi ke Animator untuk trigger animasi crouch
    public Animator playerAnimator;     // Drag PlayerArmature ke sini di Inspector

    [Header("Kecepatan")]
    public float normalSpeed = 2f;      // Kecepatan normal
    public float crouchSpeed = 1f;      // Kecepatan saat jongkok

    [Header("Scale Jongkok")]
    public Vector3 normalScale = new Vector3(1f, 1f, 1f);      // Scale normal
    public Vector3 crouchScale = new Vector3(1f, 0.6f, 1f);    // Scale saat jongkok

    private bool isCrouching = false;
    private Button button;
    private ThirdPersonController thirdPersonController;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleCrouch);

        if (playerTransform != null)
        {
            thirdPersonController = playerTransform.GetComponent<ThirdPersonController>();
            // Ambil ThirdPersonController dari PlayerArmature
        }

        // Kalau playerAnimator belum di-assign manual di Inspector,
        // coba ambil otomatis dari playerTransform
        if (playerAnimator == null && playerTransform != null)
        {
            playerAnimator = playerTransform.GetComponent<Animator>();
            // GetComponent mencari komponen Animator di GameObject yang sama
        }

        if (thirdPersonController == null)
            Debug.LogError("CrouchButton: ThirdPersonController tidak ditemukan!");

        if (playerAnimator == null)
            Debug.LogError("CrouchButton: Animator tidak ditemukan di PlayerArmature!");
    }

    void ToggleCrouch()
    {
        isCrouching = !isCrouching;     // Ganti antara true/false setiap kali tombol ditekan

        if (playerState != null)
            playerState.isCrouching = isCrouching;
            // Beritahu PlayerState — SmokeDamage akan membaca ini

        if (thirdPersonController != null)
            thirdPersonController.MoveSpeed = isCrouching ? crouchSpeed : normalSpeed;
            // Jongkok: pakai crouchSpeed | Berdiri: kembalikan normalSpeed

        // BARU: Set parameter Crouch di Animator
        if (playerAnimator != null)
            playerAnimator.SetBool("Crouch", isCrouching);
            // "Crouch" harus persis sama dengan nama parameter
            // yang kamu buat di Animator Controller tadi
            // SetBool karena crouch adalah kondisi on/off (bukan sekali pukul)

        // Ubah warna tombol sebagai feedback visual
        ColorBlock colors = button.colors;
        colors.normalColor = isCrouching ? new Color(0.6f, 0.6f, 0.6f) : Color.white;
        button.colors = colors;
    }
}