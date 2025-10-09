using System.Collections;
using Lean.Pool;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;

    [Header("VFX Prefabs")]
    [SerializeField] private GameObject explosionVFX;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayExplosionVFX(Vector2 position)
    {
        var vfx = LeanPool.Spawn(explosionVFX, position, Quaternion.identity, transform);
        var anim = vfx.GetComponent<Animator>();
        anim.Play("explosion");

        // get animation clip length and despawn after it finishes
        StartCoroutine(DespawnAfterAnim(vfx, anim));
    }

    private IEnumerator DespawnAfterAnim(GameObject vfx, Animator anim)
    {
        // get current state info (0 = base layer)
        yield return null; // wait one frame so animator updates
        var info = anim.GetCurrentAnimatorStateInfo(0);
        float length = info.length;

        yield return new WaitForSeconds(length);
        LeanPool.Despawn(vfx);
    }
}