using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// �ӵ���
/// </summary>
public class Projectile : MonoBehaviour
{
  
    [SerializeField] GameObject hitVFX;
    [SerializeField]
    AudioData[] hitData;
    /// <summary>
    /// �˺�ֵ
    /// </summary>
    [SerializeField] float damage;
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected Vector2 moveDirection;

    protected GameObject target;
    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectly());
    }


    IEnumerator MoveDirectly()
    {
        while(gameObject.activeSelf)
        {
            Move();
            yield return null;
        }
    }


    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Character character))
        {
            AudioManager.Instance.PlayRandomSFX(hitData);
            character.TakeDamage(damage);

           // var contactPoint = collision.GetContact(0);//�����ײ��
           //�����ӵ�������Ч��λ�ú���ת
            PoolManager.Release(hitVFX, collision.GetContact(0).point
                ,Quaternion.LookRotation(collision.GetContact(0).normal));

            gameObject.SetActive(false);
        }
    }

    protected void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void Move()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
