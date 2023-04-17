using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileAim : Projectile
{
    private void Awake()
    {
        SetTarget(GameObject.FindGameObjectWithTag("Player"));
    }

    protected override void OnEnable()
    {

        StartCoroutine(MoveDirectionCoroutine());
        base.OnEnable();
    
       
    }


    IEnumerator MoveDirectionCoroutine()
    {
        //  yield return new WaitForSeconds(0.1f);
        yield return null;
        if (target.activeSelf)
        {
        
            moveDirection = (target.transform.position-transform.position).normalized;
        }
    }

}
