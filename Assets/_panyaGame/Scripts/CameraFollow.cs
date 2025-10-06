using UnityEngine;
using _panyaGame.Scripts.Player_Related;

namespace _panyaGame.Scripts
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform player;
        [SerializeField] private Transform mainCamera;

        [Header("Normal Follow Settings")]
        [SerializeField] private float smoothSpeedUp = 5f;
        [SerializeField] private float smoothSpeedDown = 2f;
        [SerializeField] private float verticalOffset = 2f;
        [SerializeField] private bool onlyMoveUp = false;

        [Header("Death Follow Settings")]
        [SerializeField] private float deathFollowSpeed = 10f;     // ölüm sonrası takip hızı
        [SerializeField] private float deathFollowDuration = 2.5f; // ölüm sonrası takip süresi
        [SerializeField] private float deathVerticalOffset = -1.5f;// ölüm sırasında offset (daha çok aşağıyı göster)
        [SerializeField] private float offsetLerpSpeed = 6f;       // offset geçiş hızı (normal <-> death)

        private float _highestY;
        private bool _playerLost;
        private float _deathTimer;

        // offset geçişi için runtime değer
        private float _currentOffset;

        private void OnEnable()  => PlayerController.OnPlayerLost += OnPlayerLost;
        private void OnDisable() => PlayerController.OnPlayerLost -= OnPlayerLost;

        private void Start()
        {
            if (!player)
            {
                Debug.LogError("CameraFollow: Player reference missing!");
                enabled = false; return;
            }
            if (!mainCamera)
            {
                Debug.LogError("CameraFollow: Camera reference missing!");
                enabled = false; return;
            }

            _highestY = player.position.y;
            _currentOffset = verticalOffset; // başlangıçta normal offset

            var startPos = mainCamera.position;
            startPos.y = _highestY + _currentOffset;
            mainCamera.position = startPos;
        }

        private void LateUpdate()
        {
            if (!player || !mainCamera) return;

            // hedef offset’i seç (ölüm modunda farklı)
            float targetOffset = _playerLost ? deathVerticalOffset : verticalOffset;
            _currentOffset = Mathf.Lerp(_currentOffset, targetOffset, offsetLerpSpeed * Time.deltaTime);

            float targetY  = player.position.y;
            float desiredY = targetY + _currentOffset;

            // Ölüm sonrası hızlı takip ve sonra durma
            if (_playerLost)
            {
                _deathTimer += Time.deltaTime;

                if (_deathTimer < deathFollowDuration)
                {
                    Vector3 deathPos = new Vector3(
                        mainCamera.position.x,
                        Mathf.Lerp(mainCamera.position.y, desiredY, deathFollowSpeed * Time.deltaTime),
                        mainCamera.position.z
                    );
                    mainCamera.position = deathPos;
                }
                // süre dolunca kamerayı olduğu yerde bırak
                return;
            }

            // Normal takip davranışı
            if (targetY > _highestY || !onlyMoveUp)
            {
                if (targetY > _highestY)
                    _highestY = targetY;

                float speed = targetY > mainCamera.position.y ? smoothSpeedUp : smoothSpeedDown;
                Vector3 newPos = new Vector3(
                    mainCamera.position.x,
                    Mathf.Lerp(mainCamera.position.y, desiredY, speed * Time.deltaTime),
                    mainCamera.position.z
                );
                mainCamera.position = newPos;
            }
        }

        private void OnPlayerLost()
        {
            _playerLost = true;
            _deathTimer = 0f;
        }
    }
}
