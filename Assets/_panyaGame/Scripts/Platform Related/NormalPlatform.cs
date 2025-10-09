using _panyaGame.Scripts.Player_Related;
using DG.Tweening;
using UnityEngine;

namespace _panyaGame.Scripts.Platform_Related
{
    public class NormalPlatform : BasePlatform
    {
        [SerializeField] private float bounceDistance = 0.15f;
        [SerializeField] private float bounceDuration = 0.15f;

        private Vector3 _originalPos;
        private Tween _bounceTween;

        protected override void InitPlatform()
        {
            Type = PlatformType.Normal;
            _originalPos = transform.localPosition;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;
            
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            
            if (player.GetBottomPoint().y > _originalPos.y && player.GetLinearVelocity().y <=0)
            {
                player.Jump();
                // cancel old tween if still playing
                _bounceTween?.Kill();

                // simple squash-bounce effect
                _bounceTween = transform
                    .DOLocalMoveY(_originalPos.y - bounceDistance, bounceDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        transform.DOLocalMoveY(_originalPos.y, bounceDuration)
                            .SetEase(Ease.OutBack);
                    });
            }
            
        }
    }
}