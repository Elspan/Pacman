using UnityEngine;

[System.Serializable]
public class GameSettings
{
    [Header("Vitesses")]
    [Range(1f, 10f)] public float playerSpeed = 4f;
    [Range(1f, 10f)] public float ghostNormalSpeed = 2f;
    [Range(0.5f, 5f)] public float ghostFrightenedSpeed = 1f;

    [Header("Super Pac-Gum")]
    [Range(2f, 30f)] public float powerPelletDuration = 8f;

    [Header("Prison des Fantômes")]
    [Range(0f, 20f)] public float ghostPrisonTime = 5f;
}
