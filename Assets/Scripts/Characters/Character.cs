using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;
    [SerializeField] AudioData[] deathData;
    [SerializeField] protected float maxHealth;
    [SerializeField] bool showOnHeadHealthBar = true;
    [SerializeField] StateBar onHeadHealthBar;

   protected float health;


    protected virtual void OnEnable()
    {
        health = maxHealth;
        
        if(showOnHeadHealthBar)
        {
            ShowOnHeadHealthBar();
        }
        else
        {
            HideOnHeadHealthBar();
        
        }
    }

    /// <summary>
    /// 显示UI血条
    /// </summary>
    public void ShowOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(true);
        onHeadHealthBar.Initialize(health, maxHealth);
    }

    /// <summary>
    /// 隐藏UI血条
    /// </summary>
    public void HideOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(false);
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage)
    {
        if (health == 0f)
            return;

        health -= damage;

        if(showOnHeadHealthBar)
        {
            onHeadHealthBar.UpdateState(health, maxHealth);
        }


        if(health<=0f)
        {
            Die();
        }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public virtual void Die()
    {
        health = 0;
        AudioManager.Instance.PlayRandomSFX(deathData);
        PoolManager.Release(deathVFX,transform.position);
        gameObject.SetActive(false);
    }


    /// <summary>
    /// 恢复生命值
    /// </summary>
    /// <param name="value"></param>
    public virtual void RestoreHealth(float value)
    {
        if (health == maxHealth)
            return;

        health += value;
        health=Mathf.Clamp(health,0,maxHealth);
        if (showOnHeadHealthBar)
        {
            onHeadHealthBar.UpdateState(health, maxHealth);
        }
    }

    /// <summary>
    /// 持续回复生命值协程
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="percent"></param>
    /// <returns></returns>
    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime,float percent)
    {
        while(health<maxHealth)
        {
            yield return waitTime;

            RestoreHealth(maxHealth *percent);
        }
    }


    /// <summary>
    /// 持续回复生命值协程
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="percent"></param>
    /// <returns></returns>
    protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health >0)
        {
            yield return waitTime;

            RestoreHealth(maxHealth * percent);
        }
    }
}
