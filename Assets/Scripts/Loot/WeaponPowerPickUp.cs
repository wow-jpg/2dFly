using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZJ;

public class WeaponPowerPickUp : LootItem
{

    [SerializeField] int fullPowerScoreBonus = 200;
    [SerializeField] AudioData fullPowerPickUpSFX;
    protected override void PickUp()
    {
        if (player.IsFullPower)
        {
            pickUpSFX = fullPowerPickUpSFX;
            lootMessage.text = "���ӷ�����" + fullPowerScoreBonus;
            ScoreManager.Instance.AddScore(fullPowerScoreBonus);
        }
        else
        {
            pickUpSFX = defaultPickUpSFX;
            lootMessage.text = "����������";
            player.PowerUp();
        }



        base.PickUp();

    }
}
