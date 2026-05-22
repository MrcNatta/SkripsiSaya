using UnityEngine;

public class FloorVisibilityManager : MonoBehaviour
{
    [Header("GameObject Lantai 1")]
    public GameObject[] objectsFloor1;
    // Drag semua objek lantai 1 ke sini (furnitur, dinding, lantai, dll)

    [Header("GameObject Lantai 2")]
    public GameObject[] objectsFloor2;
    // Drag semua objek lantai 2 ke sini

    [Header("Lantai Awal")]
    public int startFloor = 1;
    // Player mulai di lantai berapa

    void Start()
    {
        // Set visibility sesuai lantai awal
        SetFloor(startFloor);
    }

    public void SetFloor(int floor)
    {
        if (floor == 1)
        {
            // Tampilkan lantai 1, sembunyikan lantai 2
            SetActive(objectsFloor1, true);
            SetActive(objectsFloor2, false);
        }
        else if (floor == 2)
        {
            // Tampilkan lantai 2, sembunyikan lantai 1
            SetActive(objectsFloor1, false);
            SetActive(objectsFloor2, true);
        }

        Debug.Log("[FloorVisibilityManager] Sekarang di lantai " + floor);
    }

    // Helper — aktifkan atau nonaktifkan semua objek dalam array
    private void SetActive(GameObject[] objects, bool active)
    {
        if (objects == null) return;

        foreach (GameObject obj in objects)
        {
            if (obj != null)
                obj.SetActive(active);
        }
    }
}