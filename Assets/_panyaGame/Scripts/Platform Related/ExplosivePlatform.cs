using System.Collections;
using _panyaGame.Scripts.Player_Related;
using DG.Tweening;
using UnityEngine;

namespace _panyaGame.Scripts.Platform_Related
{
    public class ExplosivePlatform : BasePlatform
    {
        [Header("Fuse Settings")]
        [SerializeField] private Vector2 fuseTimeRange = new Vector2(2f, 4f);
        [SerializeField] private float minBlinkInterval = 0.05f;
        [SerializeField] private float maxBlinkInterval = 0.25f;

        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Collider2D coll;

        private bool _fuseStarted;
        private Coroutine _fuseCo;
        private Color _baseColor;

        protected override void InitPlatform()
        {
            Type = PlatformType.Explosive;

            if (!spriteRenderer) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (!coll) coll = GetComponent<Collider2D>();

            _fuseStarted = false;

            if (spriteRenderer)
            {
                _baseColor = spriteRenderer.color;
                spriteRenderer.color = _baseColor; // reset
            }

            if (coll) coll.enabled = true;

            // temizlik
            if (_fuseCo != null) { StopCoroutine(_fuseCo); _fuseCo = null; }
            DOTween.Kill(spriteRenderer, complete: false);
        }

        private void OnDisable()
        {
            if (_fuseCo != null) { StopCoroutine(_fuseCo); _fuseCo = null; }
            DOTween.Kill(spriteRenderer, complete: false);
        }

        private void Update()
        {
            // child SpriteRenderer görünür olduysa başlat
            if (!_fuseStarted && spriteRenderer && spriteRenderer.isVisible)
            {
                _fuseStarted = true;
                float fuse = Random.Range(fuseTimeRange.x, fuseTimeRange.y);
                _fuseCo = StartCoroutine(FuseRoutine(fuse));
            }
        }

        private IEnumerator FuseRoutine(float fuseTime)
        {
            float elapsed = 0f;

            while (elapsed < fuseTime)
            {
                float t = elapsed / fuseTime; // 0→1
                float interval = Mathf.Lerp(maxBlinkInterval, minBlinkInterval, t);

                // kırmızıya git
                spriteRenderer.DOColor(Color.red, interval * 0.5f);
                yield return new WaitForSeconds(interval * 0.5f);

                // base renge dön
                spriteRenderer.DOColor(_baseColor, interval * 0.5f);
                yield return new WaitForSeconds(interval * 0.5f);

                elapsed += interval;
            }

            // süre bitti: collider kapat, görseli söndür (despawn yok; generator yapacak)
            if (coll) coll.enabled = false;
            spriteRenderer.DOFade(0f, 0.3f);
            VFXManager.Instance.PlayExplosionVFX(transform.position);
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;
            
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            
            if (player.GetBottomPoint().y > transform.position.y && player.GetLinearVelocity().y <=0)
            {
                player.Jump();
                
            }
            
        }
    }
}
