using System;
using _panyaGame.Scripts.Platform_Related;
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
        if (other.CompareTag("Player") && other.gameObject.GetComponent<Rigidbody2D>().linearVelocityY <= 0)
        {
            BrokePlatform();
        }
    }

    private void BrokePlatform()
    {
        animator.Play("brokenPlatformBroke");
    }
}
