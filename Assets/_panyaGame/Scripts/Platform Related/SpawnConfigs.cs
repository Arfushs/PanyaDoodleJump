using System.Collections.Generic;
using UnityEngine;

namespace _panyaGame.Scripts.Platform_Related
{
    [System.Serializable]
    public class PlatformSpawnConfig
    {
        public PlatformType type;
        [Range(0f, 100f)] public float spawnChance;
        public float minHeight;
        public GameObject prefab;
    }
    
    [System.Serializable]
    public class ObstacleSpawnConfig
    {
        public ObstacleType type;
        [Range(0f, 100f)] public float spawnChance;
        public float minHeight;
        public GameObject prefab;
        public Vector2 heightOffsetRange; // Min-Max height above platform
    }
    
    [System.Serializable]
    public class DifficultySettings
    {
        [Header("Height Settings")]
        public float heightThreshold;
        
        [Header("Platform Settings")]
        public float minDistance;
        public float maxDistance;
        public float horizontalSpread;
        public List<PlatformSpawnConfig> platformConfigs;
        
        [Header("Obstacle Settings")]
        [Range(0f, 100f)] public float obstacleSpawnChance;
        public List<ObstacleSpawnConfig> obstacleConfigs;
    }
}