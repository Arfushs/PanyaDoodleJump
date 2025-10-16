using System;
using _panyaGame.Scripts.Managers;
using _panyaGame.Scripts.Platform_Related;
using _panyaGame.Scripts.Player_Related;
using UnityEngine;

public class BrokenPlatform : BasePlatform
{
    [SerializeField] private Animator animator;
    

    protected override void InitPlatform()
    {
        Type = PlatformType.Breakable;
        FixPlatform();
    }

    private void FixPlatform()
    {
        animator.Play("brokenPlatformIdle");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
            
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        
        if (player.GetBottomPoint().y > transform.position.y && player.GetLinearVelocity().y <=0)
        {
            BrokePlatform();
            SFXManager.Instance.PlayOneShot(SFXManager.Instance.BrokenPlatformJumpClip);
        }
    }

    private void BrokePlatform()
    {
        animator.Play("brokenPlatformBroke");
    }
}
