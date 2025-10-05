using _panyaGame.Scripts.Player_Related;
using UnityEngine;

namespace _panyaGame.Scripts.Platform_Related
{
    public abstract class BasePlatform : MonoBehaviour
    {
        [SerializeField] protected PlatformType platformType;
        [SerializeField] protected LayerMask platformLayer;
        
        public PlatformType Type => platformType;
        
        // Called when platform is spawned from pool
        public virtual void OnSpawn()
        {
            gameObject.SetActive(true);
        }
        
        // Called before platform is returned to pool
        public virtual void OnDespawn()
        {
            gameObject.SetActive(false);
        }
        
    }
}