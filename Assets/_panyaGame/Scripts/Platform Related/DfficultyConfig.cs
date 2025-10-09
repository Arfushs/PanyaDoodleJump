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
    
        
    }

    [System.Serializable]
    public class PlatformTypeWeight
    {
        public PlatformType type;
        public float weight = 1f;
    }
    

    public enum PlatformType
    {
        Normal,
        Moving,
        Breakable,
        Spring,
        Explosive,
        OneTime
    }

    
}