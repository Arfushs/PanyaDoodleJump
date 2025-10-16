using System;
using System.Collections.Generic;
using _panyaGame.Scripts.Managers;
using Lean.Pool;
using UnityEngine;

namespace _panyaGame.Scripts.Platform_Related
{
    public class PlatformGenerator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform player;
        [SerializeField] private PlatformPrefabLibrary prefabLibrary;

        [Header("Generation Settings")]
        [SerializeField] private float generateAheadDistance = 20f;
        [SerializeField] private float despawnBehindDistance = 10f;
        [SerializeField] private float screenWidth = 5f;
        [SerializeField] private int  maxPlatformsPerFrame = 4; // 💡 ani patlamayı önler

        [Header("Difficulty Configs")]
        [SerializeField] private DifficultyConfig[] difficultyLevels;

        public float LowestPlatformY { get; private set; }

        private float lastPlatformY;
        private int currentDifficultyIndex = 0;
        private readonly List<GameObject> activePlatforms = new();

        // Threshold listesi karışık gelirse düzelt
        private void OnValidate()
        {
            if (difficultyLevels != null && difficultyLevels.Length > 1)
            {
                Array.Sort(difficultyLevels, (a, b) => a.heightThreshold.CompareTo(b.heightThreshold));
            }
        }

        void Start()
        {
            lastPlatformY = 0f;
            // İlk lowest değeri güvenli olsun
            LowestPlatformY = Mathf.Infinity;
            GenerateInitialPlatforms();
        }

        void Update()
        {
            if (!player) return;

            UpdateDifficulty();

            // 💡 World-space: player.position.y + ileri mesafe
            float targetY = player.position.y + generateAheadDistance;

            int spawnedThisFrame = 0;
            while (lastPlatformY < targetY && spawnedThisFrame < maxPlatformsPerFrame)
            {
                GenerateNextPlatform();
                spawnedThisFrame++;
            }

            DespawnOldPlatforms();
            UpdateLowestPlatformY();
        }

        private void UpdateLowestPlatformY()
        {
            if (activePlatforms.Count == 0)
            {
                LowestPlatformY = Mathf.Infinity;
                return;
            }

            float minY = float.MaxValue;
            foreach (var p in activePlatforms)
            {
                if (p && p.transform.position.y < minY)
                    minY = p.transform.position.y;
            }

            LowestPlatformY = minY;
        }

        void GenerateInitialPlatforms()
        {
            // İlk platform player'ın altına
            float firstPlatformY = player.position.y - 1.5f;
            Vector3 firstPos = new Vector3(0f, firstPlatformY, 0f);
            
            GameObject startPrefab = prefabLibrary.GetPrefab(PlatformType.Normal);

            if (startPrefab)
            {
                GameObject startPlatform = LeanPool.Spawn(startPrefab, firstPos, Quaternion.identity, transform);
                activePlatforms.Add(startPlatform);
                lastPlatformY = firstPlatformY;
                LowestPlatformY = firstPlatformY;
            }

            for (int i = 0; i < 9; i++)
                GenerateNextPlatform();
        }

        void GenerateNextPlatform()
        {
            DifficultyConfig config = GetCurrentDifficulty();
            if (!config) return;

            // 🔒 Bu zorluk aralığı: [lower, upper)
            float lower = difficultyLevels[currentDifficultyIndex].heightThreshold;
            float upper = (currentDifficultyIndex < difficultyLevels.Length - 1)
                        ? difficultyLevels[currentDifficultyIndex + 1].heightThreshold
                        : lower + 100000f; // son seviye için geniş aralık

            // CurrentHeight başlangıca göre metre olduğu için progress buradan hesaplanır
            float h = GameManager.Instance ? GameManager.Instance.CurrentHeight : (player ? player.position.y : 0f);
            float t = Mathf.InverseLerp(lower, upper, h);

            // Bu zorlukta spacing: min→max arası
            float verticalSpacing = Mathf.Lerp(config.minVerticalSpacing, config.maxVerticalSpacing, t);
            lastPlatformY += verticalSpacing;

            float horizontalOffset = Mathf.Clamp(
                UnityEngine.Random.Range(config.minHorizontalOffset, config.maxHorizontalOffset),
                -screenWidth, screenWidth
            );

            Vector3 spawnPos = new Vector3(horizontalOffset, lastPlatformY, 0f);

            PlatformType platformType = config.GetRandomPlatformType();
            GameObject platformPrefab = prefabLibrary.GetPrefab(platformType);

            if (platformPrefab)
            {
                GameObject platform = LeanPool.Spawn(platformPrefab, spawnPos, Quaternion.identity, transform);
                activePlatforms.Add(platform);
            }
        }
        
        void DespawnOldPlatforms()
        {
            // 💡 World-space: player.position.y - behind mesafesi
            float behindY = player.position.y - despawnBehindDistance;

            for (int i = activePlatforms.Count - 1; i >= 0; i--)
            {
                var go = activePlatforms[i];
                if (!go)
                {
                    activePlatforms.RemoveAt(i);
                    continue;
                }

                if (go.transform.position.y < behindY)
                {
                    LeanPool.Despawn(go);
                    activePlatforms.RemoveAt(i);
                }
            }
        }

        // ✅ “Threshold geçilene kadar önceki difficulty sabit kalsın” seçimi
        void UpdateDifficulty()
        {
            if (difficultyLevels == null || difficultyLevels.Length == 0) return;

            float h = GameManager.Instance ? GameManager.Instance.CurrentHeight : (player ? player.position.y : 0f);

            int newIndex = 0;
            for (int i = 0; i < difficultyLevels.Length; i++)
            {
                if (h >= difficultyLevels[i].heightThreshold)
                    newIndex = i;
                else
                    break;
            }

            if (newIndex != currentDifficultyIndex)
            {
                currentDifficultyIndex = newIndex;
                OnDifficultyChanged(newIndex);
            }
        }

        DifficultyConfig GetCurrentDifficulty()
        {
            if (difficultyLevels == null || difficultyLevels.Length == 0) return null;
            return difficultyLevels[currentDifficultyIndex];
        }

        void OnDifficultyChanged(int newLevel)
        {
            Debug.Log($"Difficulty set to index {newLevel} (threshold={difficultyLevels[newLevel].heightThreshold})");
        }
    }
}
