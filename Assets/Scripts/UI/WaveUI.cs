using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZJ;

/// <summary>
/// ����UI��ʾ
/// </summary>
public class WaveUI : MonoBehaviour
{
    Text waveText;

    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        waveText = GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        waveText.text = "--�� " + EnemyManager.Instance.WaveNumber + "��--";
    }
}
