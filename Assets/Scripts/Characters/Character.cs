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
    /// ��ʾUIѪ��
    /// </summary>
    public void ShowOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(true);
        onHeadHealthBar.Initialize(health, maxHealth);
    }

    /// <summary>
    /// ����UIѪ��
    /// </summary>
    public void HideOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(false);
    }

    /// <summary>
    /// �ܵ��˺�
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
    /// ����
    /// </summary>
    public virtual void Die()
    {
        health = 0;
        AudioManager.Instance.PlayRandomSFX(deathData);
        PoolManager.Release(deathVFX,transform.position);
        gameObject.SetActive(false);
    }


    /// <summary>
    /// �ָ�����ֵ
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
    /// �����ظ�����ֵЭ��
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
    /// �����ظ�����ֵЭ��
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
