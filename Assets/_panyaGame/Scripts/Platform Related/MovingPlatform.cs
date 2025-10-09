using _panyaGame.Scripts.Player_Related;
using UnityEngine;

namespace _panyaGame.Scripts.Platform_Related
{
    public class MovingPlatform : BasePlatform
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

        protected override void InitPlatform()
        {
            Type = PlatformType.Moving;
            _amplitude = Random.Range(amplitudeRange.x, amplitudeRange.y);
            _speed     = Random.Range(speedRange.x,     speedRange.y);
            if (!_col) _col = GetComponent<Collider2D>();
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
            // parent/scale değişmiş olabilir; her frame’de de güvenli tut
            RecalcEffectiveAmplitude();

            float phase = Mathf.PingPong(Time.time * _speed, 1f) * 2f - 1f;
            float desiredX = _startPos.x + phase * _effectiveAmp;

            var (left, right) = GetInnerBounds(); // halfWidth düşülmüş sınırlar
            float x = Mathf.Clamp(desiredX, left, right);

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
                
            }
            
        }
    }
}
