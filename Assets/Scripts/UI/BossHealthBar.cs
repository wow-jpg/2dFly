using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBar : StateBar_HUD
{
    protected override void SetPercentText()
    {
        // base.SetPercentText();
        //percentText.text = Mathf.RoundToInt(targetFillAmount * 100f) + "%";
        // percentText.text = (targetFillAmount * 100f).ToString("0.00") + "%";
        percentText.text = targetFillAmount.ToString("P2");
    }
}
