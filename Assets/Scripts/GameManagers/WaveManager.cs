using System.Collections.Generic;
using Grabbit;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<Wave> waves = new List<Wave>();

    void Awake()
    {
        
    }

    
    public void SpawnWave(int waveIndex)
    {
        if (waveIndex < 0 || waveIndex >= waves.Count)
        {
            Debug.LogError("Invalid wave index");
            return;
        }

        Wave wave = waves[waveIndex];
        foreach (var enemyInfo in wave.enemies)
        {
            for (int i = 0; i < enemyInfo.spawnCount; i++)
            {
                Instantiate(enemyInfo.enemyPrefab, transform.position, transform.rotation);
            }
        }
    }
}
