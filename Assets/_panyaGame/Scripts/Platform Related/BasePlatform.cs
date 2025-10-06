using System;
using UnityEngine;

namespace _panyaGame.Scripts.Platform_Related
{
    public abstract class BasePlatform : MonoBehaviour
    {
        protected PlatformType Type = PlatformType.Normal;
        
        protected abstract void InitPlatform();

        protected virtual void OnEnable()
        {
            InitPlatform();
        }
    }
}
