using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZJ;

/// <summary>
/// 
/// </summary>
public class Boss : Enemy
{
    BossHealthBar healthBar;

    /// <summary>
    /// Boss的血条显示（最大的那个）
    /// </summary>
    Canvas healthBarCanvas;

  protected  override  void Awake()
    {
        base.Awake();
        healthBar = FindAnyObjectByType<BossHealthBar>();
        healthBarCanvas = healthBar.GetComponentInChildren<Canvas>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        healthBar.Initialize(health, maxHealth);
        healthBarCanvas.enabled = true;
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.Die();
        }
    }


    public override void Die()
    {
        healthBarCanvas.enabled = false;
        base.Die();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.UpdateState(health, maxHealth);
    }

    protected override void SetHealth()
    {
        maxHealth += EnemyManager.Instance.WaveNumber * healthFactor;
    }
}
