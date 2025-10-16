using _panyaGame.Scripts.Managers;
using _panyaGame.Scripts.Player_Related;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _panyaGame.Scripts.Platform_Related
{
    public class ObstaclePlatform : BasePlatform
    {
        private Vector3 _originalScale;
        private Collider2D _collider;
        private DeathArea _deathArea;
        private void Awake()
        {
            _originalScale = transform.localScale;
            _collider = GetComponent<Collider2D>();
            _deathArea = GetComponentInChildren<DeathArea>();
        }

        protected override void InitPlatform()
        {
            Type = PlatformType.Obstacle;
            transform.localScale = _originalScale;
            _collider.enabled = true;
            _deathArea.gameObject.SetActive(true);

            int random = Random.Range(0, 2); // 0 veya 1 döner
            float yRot = random == 0 ? 0f : 180f;
            transform.rotation = Quaternion.Euler(0f, yRot, 0f);
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;

            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            
            if (player.GetBottomPoint().y > transform.position.y && player.GetLinearVelocity().y <=0)
            {
                player.Jump(16f);
                SFXManager.Instance.PlayOneShot(SFXManager.Instance.CloudObstacleJumpClip);
                KillThisObstacle();
            }
     
        }
    

        private void KillThisObstacle()
        {
            transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => _collider.enabled = false);
            _deathArea.gameObject.SetActive(false);
        }
    }
}
