using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // Konsisten dengan semua script lain di project kamu

        if (GameManager.instance == null)
        {
            Debug.LogError("ExitTrigger: GameManager tidak ditemukan di scene!");
            return;
        }

        GameManager.instance.OnPlayerExit();
        // Beritahu GameManager bahwa player sudah keluar
        // GameManager yang akan memutuskan Ending 1 atau 2 berdasarkan HP
    }
}