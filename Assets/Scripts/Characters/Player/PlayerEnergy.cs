using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZJ;

/// <summary>
/// �������ϵͳ
/// </summary>
public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField] EnergyBar energyBar;

    [SerializeField] float overdriveInterval = 0.1f;

    bool available = true;

    /// <summary>
    /// �������
    /// </summary>
    public  const int MAX = 100;
    public  const int PERCENT = 1;

    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    int energy=0;

    WaitForSeconds waitForOverdriveInterval;

    private void OnEnable()
    {
        PlayerOverdrive.on += PlayerOverdriveOn;
        PlayerOverdrive.off += PlayerOverdriveOff;

    }



    private void OnDisable()
    {
        PlayerOverdrive.on -= PlayerOverdriveOn;
        PlayerOverdrive.off -= PlayerOverdriveOff;
    }


    private void Start()
    {
         energyBar.Initialize(energy,MAX);
         Obtain(MAX);
        waitForOverdriveInterval = new WaitForSeconds(overdriveInterval);
    }

    /// <summary>
    /// �������ֵ
    /// </summary>
    /// <param name="value"></param>
    public void Obtain(int value)
    {
        if (energy == MAX||!available) return;

        energy=Mathf.Clamp(energy+value,0,MAX);
        energyBar.UpdateState(energy, MAX);
    }

    /// <summary>
    /// ʹ������ֵ
    /// </summary>
    /// <param name="value"></param>
    public void Use(int value)
    {
        energy -= value;
        energyBar.UpdateState(energy, MAX);

        if(energy==0&&!available)
        {
            PlayerOverdrive.off.Invoke();
        }
    }


    public bool IsEnough(int value) => energy >= value;


    private void PlayerOverdriveOff()
    {
        available = true;
        StopCoroutine(nameof(KeepUsingCoroutine));
    }

    private void PlayerOverdriveOn()
    {
        available = false;
        StartCoroutine(nameof(KeepUsingCoroutine));
    }


    IEnumerator KeepUsingCoroutine()
    {
        while(gameObject.activeSelf&&energy>0)
        {
            yield return waitForOverdriveInterval;

            Use(PERCENT);
        }
    }

}
