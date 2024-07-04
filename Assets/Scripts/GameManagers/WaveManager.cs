using System.Collections.Generic;
using Grabbit;
using UnityEngine;
using VInspector;

public class WaveManager : MonoBehaviour
{
    public int ActiveWave;
    public List<Wave> waves = new List<Wave>();

    void Awake()
    {
        
    }

    
    [Button]
    public void SpawnWave()
    {
        if (ActiveWave < 0 || ActiveWave >= waves.Count)
        {
            Debug.LogError("Invalid wave index");
            return;
        }

        Wave wave = waves[ActiveWave];
        foreach (var enemyInfo in wave.enemies)
        {
            for (int i = 0; i < enemyInfo.spawnCount; i++)
            {
                Vector2 SpawnPos = Random.insideUnitCircle * 60;
                Instantiate(enemyInfo.enemyPrefab, SpawnPos, transform.rotation);
            }
        }

        ActiveWave++;
    }
}
