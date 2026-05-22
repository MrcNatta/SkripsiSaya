using UnityEngine;

public class FloorTrigger : MonoBehaviour
{
    [Header("Objek Lantai 2")]
    public GameObject[] objectsFloor2;   // Drag semua objek lantai 2 ke sini

    // Dipanggil saat player MASUK trigger — lantai 2 muncul
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        foreach (GameObject obj in objectsFloor2)
            if (obj != null) obj.SetActive(true);
    }

    // Dipanggil saat player KELUAR trigger — lantai 2 hilang
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        foreach (GameObject obj in objectsFloor2)
            if (obj != null) obj.SetActive(false);
    }
}