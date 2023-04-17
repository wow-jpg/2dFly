using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOverdrive : MonoBehaviour
{
    public static UnityAction on;
    public static UnityAction off;

   [SerializeField] GameObject triggerVFX;
   [SerializeField] GameObject engineVFXNormal;
   [SerializeField] GameObject engineVFXOverdrive;

   [SerializeField] AudioData onSFX;
   [SerializeField] AudioData offSFX;

    private void Awake()
    {
        on += On;
        off += Off;
    }

    private void Off()
    {
        engineVFXOverdrive.SetActive(false);
        engineVFXNormal.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(offSFX);
    }

    private void On()
    {
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(onSFX);
    }

    private void OnDestroy()
    {
        on -= On;
        off -= Off;
    }

}
