using UnityEngine;

public static class GameResult
{
    public static float finalHpPercent = 0f;
    // HP player saat game berakhir (0–100)

    public static float finalTimeRemaining = 0f;
    // Sisa waktu dalam detik saat game berakhir

    public static int endingNumber = 0;
    // 1=A (>80%), 2=B (60-80%), 3=C (30-60%), 4=D (1-30%), 5=E (gagal)

    public static int currentLevel = 1;
    // Level yang baru saja dimainkan
}   