using UnityEngine;
using UnityEngine.InputSystem;

public class MobileDisableAutoSwitchControls : MonoBehaviour
{
    void Start()
    {
        DisableAutoSwitchControls();
    }

    void DisableAutoSwitchControls()
    {
        // Ambil komponen PlayerInput dari object ini
        PlayerInput playerInput = GetComponent<PlayerInput>();

        // Kalau tidak ditemukan, stop — jangan lanjut
        if (playerInput == null)
        {
            Debug.LogWarning("MobileDisableAutoSwitchControls: PlayerInput tidak ditemukan, dilewati.");
            return;
        }

        // Nonaktifkan auto switch controls
        playerInput.neverAutoSwitchControlSchemes = true;
    }
}