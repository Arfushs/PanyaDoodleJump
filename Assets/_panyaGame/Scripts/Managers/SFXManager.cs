using System;
using UnityEngine;

namespace _panyaGame.Scripts.Managers
{
    public class SFXManager : MonoBehaviour
    {
        public static SFXManager Instance;
        
        [Header("Audio Clips")]
        [field:SerializeField] public AudioClip  NormalPlatformJumpClip { get; private set; }
        [field:SerializeField] public AudioClip  OneTimePlatformJumpClip { get; private set; }
        [field:SerializeField] public AudioClip  BrokenPlatformJumpClip { get; private set; }
        [field:SerializeField] public AudioClip  CloudObstacleJumpClip { get; private set; }
        [field:SerializeField] public AudioClip  ObstacleCrashed { get; private set; }
        [field:SerializeField] public AudioClip  ExplosionClip { get; private set; }
        [field:SerializeField] public AudioClip  PlayerFailedClip { get; private set; }
        
        
        private AudioSource _audioSource;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
            }
            
            DontDestroyOnLoad(gameObject);
            
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayOneShot(AudioClip clip)
        {
            if (!clip)
            {
                Debug.LogWarning("Clip could not be played !");
                return;
            }
               
            
            _audioSource.PlayOneShot(clip);
        }
        
        
    }
}
