using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePickUp : LootItem
{
    protected override void PickUp()
    {
        base.PickUp();
        player.PickUpMissile();
    }
}
