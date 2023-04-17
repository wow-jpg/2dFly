using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZJ;

public class Enemy : Character
{
    [SerializeField] int scorePoint = 100;
    [SerializeField] int deathEnergyBonus = 3;

    [SerializeField] protected int healthFactor = 2;

    LootSpawner lootSpawner;


    protected virtual void Awake()
    {
        lootSpawner= GetComponent<LootSpawner>();   
    }

    protected override void OnEnable()
    {
        SetHealth();
        base.OnEnable();

    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.Die();
            Die();
        }
    }

    public override void Die()
    {
        base.Die();
        lootSpawner.Spawn(transform.position);
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);
        EnemyManager.Instance.RemoveFromList(gameObject);
        ScoreManager.Instance.AddScore(scorePoint);
    }

    /// <summary>
    /// …Ë÷√…˙√¸÷µ
    /// </summary>
    protected virtual void SetHealth()
    {
        maxHealth += (int)(EnemyManager.Instance.WaveNumber / healthFactor);
    }
}
