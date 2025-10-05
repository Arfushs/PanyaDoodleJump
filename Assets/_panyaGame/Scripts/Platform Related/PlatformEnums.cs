using System;

namespace _panyaGame.Scripts.Platform_Related
{
    [Serializable]
    public enum PlatformType
    {
        Normal,
        Moving,
        Broken,
        Disappearing,
        Explosive
    }
    
    [Serializable]
    public enum ObstacleType
    {
        Static,
        Moving
    }
}