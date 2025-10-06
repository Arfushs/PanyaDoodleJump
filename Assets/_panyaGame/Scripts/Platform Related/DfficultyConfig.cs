using UnityEngine;

namespace _panyaGame.Scripts.Platform_Related
{
    [CreateAssetMenu(fileName = "DifficultyConfig", menuName = "Game/Difficulty Config")]
    public class DifficultyConfig : ScriptableObject
    {
        [Header("Height Threshold")]
        public float heightThreshold;
    
        [Header("Platform Spacing")]
        public float minVerticalSpacing = 1.5f;
        public float maxVerticalSpacing = 3f;
        public float minHorizontalOffset = -2f;
        public float maxHorizontalOffset = 2f;
    
        [Header("Platform Types (Weight Based)")]
        public PlatformTypeWeight[] platformWeights;
    
        [Header("Obstacle Settings")]
        [Range(0f, 1f)]
        public float obstacleSpawnChance = 0.2f;
        public ObstacleTypeWeight[] obstacleWeights;
    
        public PlatformType GetRandomPlatformType()
        {
            float totalWeight = 0f;
            foreach (var pw in platformWeights)
                totalWeight += pw.weight;
        
            float random = Random.Range(0f, totalWeight);
            float current = 0f;
        
            foreach (var pw in platformWeights)
            {
                current += pw.weight;
                if (random <= current)
                    return pw.type;
            }
        
            return PlatformType.Normal;
        }
    
        public ObstacleType GetRandomObstacleType()
        {
            if (obstacleWeights.Length == 0) return ObstacleType.None;
        
            float totalWeight = 0f;
            foreach (var ow in obstacleWeights)
                totalWeight += ow.weight;
        
            float random = Random.Range(0f, totalWeight);
            float current = 0f;
        
            foreach (var ow in obstacleWeights)
            {
                current += ow.weight;
                if (random <= current)
                    return ow.type;
            }
        
            return ObstacleType.None;
        }
    }

    [System.Serializable]
    public class PlatformTypeWeight
    {
        public PlatformType type;
        public float weight = 1f;
    }

    [System.Serializable]
    public class ObstacleTypeWeight
    {
        public ObstacleType type;
        public float weight = 1f;
    }

    public enum PlatformType
    {
        Normal,
        Moving,
        Breakable,
        Spring,
        OneTime
    }

    public enum ObstacleType
    {
        None,
        Enemy,
        Spike,
        BlackHole
    }
}