using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : LootItem
{
    [SerializeField] int fullHealthScoreBonus = 200;
    [SerializeField] float shieldBonus = 20f;
    protected override void PickUp()
    {
        if(player.IsFullHealth)
        {
            lootMessage.text = "���ӷ�����" + fullHealthScoreBonus;
            ScoreManager.Instance.AddScore(fullHealthScoreBonus);
        }
        else
        {
            player.RestoreHealth(shieldBonus);
        }



        base.PickUp();

    }
}
