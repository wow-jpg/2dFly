using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// µºµØ
/// </summary>
public class PlayerMissile : PlayerProjectileOverdrive
{
    [SerializeField] AudioData targetAcquiredVoice;

    [Header("-----")]
    [SerializeField] float lowSpeed = 8f;
    [SerializeField] float highSpeed = 25f;
    /// <summary>
    /// —” ±
    /// </summary>
    [SerializeField] float variableSpeedDelay = 0.5f;


    [Header("--- ±¨’® ---")]
    [SerializeField] GameObject explosionVFX = null;
    [SerializeField] AudioData explosionSFX = null;
    [SerializeField] LayerMask enemyLayerMask=default(LayerMask);
    [SerializeField] float explosionRadius = 3f;
    [SerializeField] float explosionDamage = 100f;

    WaitForSeconds waitVariableSpeedDelay;


    protected void Awake()
    {
        // base.Awake();

    }

    protected override void OnEnable()
    {
        waitVariableSpeedDelay = new WaitForSeconds(variableSpeedDelay);
        base.OnEnable();
        StartCoroutine(nameof(VariableSpeedCoroutine));
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        PoolManager.Release(explosionVFX, transform.position);
        AudioManager.Instance.PlayRandomSFX(explosionSFX);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius,enemyLayerMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(explosionDamage);
            }
        }
    }

    IEnumerator VariableSpeedCoroutine()
    {
        moveSpeed = lowSpeed;
        yield return waitVariableSpeedDelay;
        moveSpeed = highSpeed;

        if (target != null)
        {
            AudioManager.Instance.PlayRandomSFX(targetAcquiredVoice);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,explosionRadius);
    }
}
