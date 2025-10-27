using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SpawnMode
{
    Random,
    Sequential
}

[System.Serializable]
public class WaveSettings
{
    [Tooltip("Количество врагов в волне")]
    public int enemyCount = 5;

    [Tooltip("Интервал между спавном врагов (сек)")]
    public float spawnInterval = 1.0f;

    [Tooltip("Префаб врага")]
    public GameObject enemyPrefab;

    [Tooltip("Точки спавна для этой волны. Если пусто, будут использованы глобальные точки спавна WaveManager.spawnPoints")]
    public Transform[] spawnPoints;

    [Tooltip("Режим выбора точки спавна: случайно или по очереди")]
    public SpawnMode spawnMode = SpawnMode.Random;
}

public class WaveManager : MonoBehaviour
{
    [Header("Настройки волн")]
    public List<WaveSettings> waves = new List<WaveSettings>();

    [Header("Глобальные точки спавна (используются если для волны не заданы свои)")]
    public Transform[] spawnPoints;

    [Header("Интервал между волнами (сек)")]
    public float timeBetweenWaves = 5f;

    private int currentWave = 0;
    private bool isSpawning = false;

    void Start()
    {
        if (waves == null || waves.Count == 0)
        {
            Debug.LogWarning("[WaveManager] Список волн пуст.");
            return;
        }

        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWave < waves.Count)
        {
            yield return StartCoroutine(SpawnWave(waves[currentWave]));
            currentWave++;
            if (currentWave < waves.Count)
                yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    IEnumerator SpawnWave(WaveSettings wave)
    {
        if (wave == null)
        {
            Debug.LogWarning("[WaveManager] Пустые настройки волны.");
            yield break;
        }

        if (wave.enemyPrefab == null)
        {
            Debug.LogWarning("[WaveManager] В волне не задан префаб врага.");
            yield break;
        }

        // Выбираем массив точек спавна: приоритет у настроек волны, иначе глобальный массив
        Transform[] points = (wave.spawnPoints != null && wave.spawnPoints.Length > 0) ? wave.spawnPoints : spawnPoints;

        if (points == null || points.Length == 0)
        {
            Debug.LogWarning("[WaveManager] Нет доступных точек спавна для волны.");
            yield break;
        }

        isSpawning = true;
        int sequentialIndex = 0;

        for (int i = 0; i < wave.enemyCount; i++)
        {
            Transform spawnPoint = SelectSpawnPoint(points, wave.spawnMode, ref sequentialIndex);
            SpawnEnemyAt(wave.enemyPrefab, spawnPoint);
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        isSpawning = false;
    }

    Transform SelectSpawnPoint(Transform[] points, SpawnMode mode, ref int seqIndex)
    {
        if (points == null || points.Length == 0)
            return null;

        switch (mode)
        {
            case SpawnMode.Sequential:
                Transform pt = points[seqIndex % points.Length];
                seqIndex++;
                return pt;

            case SpawnMode.Random:
            default:
                return points[Random.Range(0, points.Length)];
        }
    }

    void SpawnEnemyAt(GameObject enemyPrefab, Transform spawnPoint)
    {
        if (spawnPoint == null || enemyPrefab == null)
            return;

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}