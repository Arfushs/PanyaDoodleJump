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
        private Dictionary<PlatformType, GameObject> platformDict;
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
            
        }
    
        public GameObject GetPrefab(PlatformType type)
        {
            if (platformDict == null) BuildDictionaries();
        
            if (platformDict != null && platformDict.TryGetValue(type, out GameObject prefab))
                return prefab;
        
            Debug.LogWarning($"Platform prefab not found for type: {type}");
            return null;
        }
        
    }

    [System.Serializable]
    public class PlatformPrefabEntry
    {
        public PlatformType type;
        public GameObject prefab;
    }

    
}