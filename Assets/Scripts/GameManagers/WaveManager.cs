using System.Collections.Generic;
using Grabbit;
using UnityEngine;
using UnityEngine.AI;
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
        if (ActiveWave < 0 || ActiveWave >= waves.Count) return;

        Wave wave = waves[ActiveWave];
        foreach (var enemyInfo in wave.enemies)
            for (int i = 0; i < enemyInfo.spawnCount; i++) SpawnEnemy(enemyInfo);

        ActiveWave++;
    }
    
    public void SpawnEnemy(Wave.EnemySpawnInfo enemy)
    {
        Vector2 SpawnPos = Random.insideUnitCircle.normalized * Random.Range(50,70);
        Vector3 SpawnPos3D = new Vector3(SpawnPos.x, 0, SpawnPos.y);

        if(NavMesh.SamplePosition(new Vector3(SpawnPos.x, 0, SpawnPos.y), out NavMeshHit hit, 100000, 0))
            Instantiate(enemy.enemyPrefab, hit.position, transform.rotation);
        else
            Instantiate(enemy.enemyPrefab, SpawnPos3D, transform.rotation);
    }
}
