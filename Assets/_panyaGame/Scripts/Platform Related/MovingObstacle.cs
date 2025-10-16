using System;
using _panyaGame.Scripts.Player_Related;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _panyaGame.Scripts.Platform_Related
{
    public class MovingObstacle : BasePlatform
    {
        [Header("Movement Settings")]
        [SerializeField] private Vector2 amplitudeRange = new Vector2(1f, 3f);
        [SerializeField] private Vector2 speedRange = new Vector2(.5f, 1f);

        [Header("World Bounds")]
        [SerializeField] private float leftBoundX = -5f;
        [SerializeField] private float rightBoundX = 5f;

        private Vector3 _startPos;
        private float _amplitude;
        private float _speed;
        private float _effectiveAmp;
        private Collider2D _col;
        private SpriteRenderer _sr;
        private DeathArea _deathArea;
        

        private void Awake()
        {
            _deathArea = GetComponentInChildren<DeathArea>();
            _sr = GetComponent<SpriteRenderer>();
        }

        protected override void InitPlatform()
        {
            Type = PlatformType.Moving;
            _amplitude = Random.Range(amplitudeRange.x, amplitudeRange.y);
            _speed     = Random.Range(speedRange.x,     speedRange.y);
            _deathArea.gameObject.SetActive(true); 
            _sr.DOFade(1, 0f);
            if (!_col) _col = GetComponent<Collider2D>();
            _col.enabled = true;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _startPos = transform.position;
            ClampStartInsideBounds();      // start pozisyonunu güvene al
            RecalcEffectiveAmplitude();    // genişlik + sınır ile efektif amplitude
        }

        private void Update()
        {
            RecalcEffectiveAmplitude();

            float phase = Mathf.PingPong(Time.time * _speed, 1f) * 2f - 1f;
            float desiredX = _startPos.x + phase * _effectiveAmp;

            var (left, right) = GetInnerBounds();
            float x = Mathf.Clamp(desiredX, left, right);

            // 🔹 Yön kontrolü (sağa giderken 180°, sola giderken 0°)
            float deltaX = x - transform.position.x;
            if (Mathf.Abs(deltaX) > 0.001f)
            {
                float yRot = deltaX > 0 ? 180f : 0f;
                transform.rotation = Quaternion.Euler(0f, yRot, 0f);
            }

            transform.position = new Vector3(x, _startPos.y, _startPos.z);
        }


        private (float left, float right) GetInnerBounds()
        {
            float halfW = 0f;
            if (_col) halfW = _col.bounds.extents.x; // platform yarı genişliği (scale dahil)

            float left  = leftBoundX  + halfW;
            float right = rightBoundX - halfW;
            if (right < left) right = left; // patolojik durumda kilitle
            return (left, right);
        }

        private void ClampStartInsideBounds()
        {
            var (left, right) = GetInnerBounds();
            _startPos.x = Mathf.Clamp(_startPos.x, left, right);
            transform.position = _startPos;
        }

        private void RecalcEffectiveAmplitude()
        {
            var (left, right) = GetInnerBounds();

            // start’tan sola/sağa kalabilecek max mesafeler
            float maxLeft  = Mathf.Max(0f, _startPos.x - left);
            float maxRight = Mathf.Max(0f, right - _startPos.x);

            _effectiveAmp = Mathf.Clamp(_amplitude, 0f, Mathf.Min(maxLeft, maxRight));
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;
            
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            
            if (player.GetBottomPoint().y > transform.position.y && player.GetLinearVelocity().y <=0)
            {
                player.Jump();
                KillThisObstacle();

            }
            
        }

        private void KillThisObstacle()
        {
            _deathArea.gameObject.SetActive(false); 
            _col.enabled = false;
            VFXManager.Instance.PlayExplosionVFX(transform.position,new Vector3(.5f,.5f,.5f));
            _sr.DOFade(0, 0.3f);

        }
    }
}
