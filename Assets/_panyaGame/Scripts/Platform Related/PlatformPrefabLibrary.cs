using System.Collections.Generic;
using UnityEngine;

namespace _panyaGame.Scripts.Platform_Related
{
    [CreateAssetMenu(fileName = "PlatformLibrary", menuName = "Game/Platform Library")]
    public class PlatformPrefabLibrary : ScriptableObject
    {
        [Header("Platform Prefabs")]
        public PlatformPrefabEntry[] platforms;
    
        [Header("Obstacle Prefabs")]
        public ObstaclePrefabEntry[] obstacles;
    
        private Dictionary<PlatformType, GameObject> platformDict;
        private Dictionary<ObstacleType, GameObject> obstacleDict;
    
        void OnEnable()
        {
            BuildDictionaries();
        }
    
        void BuildDictionaries()
        {
            platformDict = new Dictionary<PlatformType, GameObject>();
            foreach (var entry in platforms)
            {
                if (!platformDict.ContainsKey(entry.type))
                    platformDict.Add(entry.type, entry.prefab);
            }
        
            obstacleDict = new Dictionary<ObstacleType, GameObject>();
            foreach (var entry in obstacles)
            {
                if (!obstacleDict.ContainsKey(entry.type))
                    obstacleDict.Add(entry.type, entry.prefab);
            }
        }
    
        public GameObject GetPrefab(PlatformType type)
        {
            if (platformDict == null) BuildDictionaries();
        
            if (platformDict != null && platformDict.TryGetValue(type, out GameObject prefab))
                return prefab;
        
            Debug.LogWarning($"Platform prefab not found for type: {type}");
            return null;
        }
    
        public GameObject GetObstaclePrefab(ObstacleType type)
        {
            if (obstacleDict == null) BuildDictionaries();
        
            if (obstacleDict != null && obstacleDict.TryGetValue(type, out GameObject prefab))
                return prefab;
        
            Debug.LogWarning($"Obstacle prefab not found for type: {type}");
            return null;
        }
    }

    [System.Serializable]
    public class PlatformPrefabEntry
    {
        public PlatformType type;
        public GameObject prefab;
    }

    [System.Serializable]
    public class ObstaclePrefabEntry
    {
        public ObstacleType type;
        public GameObject prefab;
    }
}