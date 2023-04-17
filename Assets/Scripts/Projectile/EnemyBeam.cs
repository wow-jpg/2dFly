using UnityEngine;
using ZJ;

/// <summary>
/// µ–»Àº§π‚
/// </summary>
public class EnemyBeam : MonoBehaviour
{
    [SerializeField] float damage = 50f;
    [SerializeField] GameObject hitVFX;


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player character))
        {
          
            character.TakeDamage(damage);
            PoolManager.Release(hitVFX, collision.GetContact(0).point
                , Quaternion.LookRotation(collision.GetContact(0).normal));

        }
    }
}
