using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave
{
    [Serializable]
    public struct EnemySpawnInfo
    {
        public GameObject enemyPrefab;
        public int spawnCount;
    }

    public List<EnemySpawnInfo> enemies = new List<EnemySpawnInfo>();
}