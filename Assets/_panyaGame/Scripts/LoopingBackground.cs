using UnityEngine;

namespace _panyaGame.Scripts
{
    public class LoopingBackground : MonoBehaviour
    {
        [Header("Refs (tam 3 adet)")]
        
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform top;
        [SerializeField] private Transform center;
        [SerializeField] private Transform bottom;

        [Header("Ayarlar")]
        [SerializeField] private float segmentHeight = 0f;   // iki arka planın merkezleri arası mesafe
        [SerializeField] private float threshold = 0.001f;   // jitter önleyici

        private void Awake()
        {
            if (!cameraTransform) cameraTransform = Camera.main.transform;
            SortByYAscending();

            if (segmentHeight <= 0f)
            {
                // mevcut dizilimden otomatik hesap
                float h1 = Mathf.Abs(top.position.y - center.position.y);
                float h2 = Mathf.Abs(center.position.y - bottom.position.y);
                segmentHeight = (h1 > 0f ? h1 : h2);
                if (segmentHeight <= 0f) segmentHeight = 20f; // son çare
            }
        }

        private void Update()
        {
            if (!cameraTransform) return;

            float camY = cameraTransform.position.y;

            // KAMERA CENTER'IN ÜSTÜNE ÇIKTIĞI SÜRECE → alttakini üste taşı
            while (camY > center.position.y + threshold)
                MoveBottomToTop();

            // KAMERA CENTER'IN ALTINA İNDİĞİ SÜRECE → üsttekini alta indir
            while (camY < center.position.y - threshold)
                MoveTopToBottom();
        }

        private void MoveBottomToTop()
        {
            // en alttakini, en üsttekinin bir segment üstüne taşı
            bottom.position = new Vector3(
                bottom.position.x,
                top.position.y + segmentHeight,
                bottom.position.z
            );

            // rolleri döndür: bottom→top, center→bottom, top→center
            Transform oldBottom = bottom;
            bottom = center;
            center = top;
            top    = oldBottom;
        }

        private void MoveTopToBottom()
        {
            // en üsttekini, en alttakinin bir segment altına indir
            top.position = new Vector3(
                top.position.x,
                bottom.position.y - segmentHeight,
                top.position.z
            );

            // rolleri döndür: top→bottom, center→top, bottom→center
            Transform oldTop = top;
            top    = center;
            center = bottom;
            bottom = oldTop;
        }

        private void SortByYAscending()
        {
            Transform[] arr = { top, center, bottom };
            System.Array.Sort(arr, (a, b) => a.position.y.CompareTo(b.position.y));
            bottom = arr[0];
            center = arr[1];
            top    = arr[2];
        }
    }
}
