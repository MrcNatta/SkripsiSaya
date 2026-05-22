using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;          // Sesuai spesifikasi kamu: HP 100

    private float currentHealth;

    public bool isDead = false;

    [Header("Events")]
    public UnityEvent<float> onHealthChanged;   // Dikirim ke UI Health Bar
    public UnityEvent onDeath;

    void Start()
    {
        currentHealth = maxHealth;          // Mulai dengan HP penuh
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;                 // Sudah mati? Hentikan

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
                                            // Paksa nilai tetap antara 0-100

        onHealthChanged.Invoke(currentHealth / maxHealth);
                                            // Kirim persentase ke Health Bar UI

        if (currentHealth <= 0f)
        {
            isDead = true;
            onDeath.Invoke();               // Nanti dihubungkan ke Game Over
        }
    }

    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }
}