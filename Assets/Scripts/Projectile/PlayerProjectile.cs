using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    TrailRenderer trail;



    protected override  void OnEnable()
    {
        base.OnEnable();
        trail =GetComponentInChildren<TrailRenderer>();

        if(moveDirection!=Vector2.right)
        {
            transform.GetChild(0).rotation=Quaternion.FromToRotation(Vector2.right,moveDirection);
        }
    }

    private void OnDisable()
    {
        //Çå³ý¹ì¼£
        trail.Clear();
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        PlayerEnergy.Instance.Obtain(PlayerEnergy.PERCENT);
    }
}
