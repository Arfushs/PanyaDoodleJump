using _panyaGame.Scripts.Player_Related;
using DG.Tweening;
using UnityEngine;

namespace _panyaGame.Scripts.Platform_Related
{
    public class OneTimePlatform : BasePlatform
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Collider2D _coll;

        private void Awake()
        {
            _coll = GetComponent<Collider2D>();
        }
        protected override void InitPlatform()
        {
            Type = PlatformType.OneTime;
            _coll.enabled = true;
            spriteRenderer.color = new Color(1f,1f,1f,1f);
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;
            
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            
            if (player.GetBottomPoint().y > transform.position.y && player.GetLinearVelocity().y <=0)
            {
                player.Jump();
                FadeOut();
            }
            
        }

        private void FadeOut()
        {
            spriteRenderer.DOFade(0, .5f).OnComplete(()=>_coll.enabled = false);
        }
    }
}
