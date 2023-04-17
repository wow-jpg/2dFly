using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZJ;

public class BossController : EnemyController
{
    [SerializeField] float continuousFireDuration = 1.5f;

    [Header("--- ������ ---")]
    [SerializeField] Transform playerDetectionTransform;
    [SerializeField] Vector3 playerDetectionSize;
    [SerializeField] LayerMask playerLayer;

    [Header("--- ���� ---")]
    [SerializeField] float beamCooldownTime = 12f;
    [SerializeField] AudioData beamChargingSFX;
    [SerializeField] AudioData beamLaunchSFX;
    /// <summary>
    /// �����Ƿ���ȴ���
    /// </summary>
    bool isBeamReady;

    int launchBeamHash=Animator.StringToHash("launchBeam");

    WaitForSeconds waitForContinuousFireInterval;
    WaitForSeconds waitForFireInterval;
    WaitForSeconds waitBeamCooldownTime;


    /// <summary>
    /// ��
    /// </summary>
    List<GameObject> magazine;
    AudioData launchSFX;

    Animator animator;

    Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        waitForContinuousFireInterval = new WaitForSeconds(minFireInterval);
        waitForFireInterval = new WaitForSeconds(maxFireInterval);

        magazine = new List<GameObject>(projectiles.Length);

        waitBeamCooldownTime = new WaitForSeconds(beamCooldownTime);

        animator = GetComponent<Animator>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void OnEnable()
    {
        isBeamReady = false;
        StartCoroutine(nameof(BeamCooldownCoroutine));
        base.OnEnable();
        
    }


    IEnumerator ContinuousFireCoroutine()
    {
        LoadProjectiles();
        muzzleVFX.Play();

        float continuousFireTimer = 0f;

        while (continuousFireTimer < continuousFireDuration)
        {
            foreach (var item in magazine)
            {
                PoolManager.Release(item, muzzle.position);
            }

            continuousFireTimer += minFireInterval;
            AudioManager.Instance.PlayRandomSFX(launchSFX);

            yield return waitForContinuousFireInterval;
          //  Debug.Log(continuousFireTimer);
        }
        muzzleVFX.Stop();

    }


    protected override IEnumerator RandomlyFireCoroutine()
    {
       

        while (isActiveAndEnabled)
        {
            if (GameManager.GameState == GameState.GameOver) yield break;

            if (isBeamReady)
            {
                ActivateBeamWeapon();
                StartCoroutine(nameof(ChasingPlayerCoroutine));
                yield break;
            }
            yield return waitForFireInterval;
            yield return StartCoroutine(nameof(ContinuousFireCoroutine));
        }



    }


    /// <summary>
    /// װ���ӵ�
    /// </summary>
    void LoadProjectiles()
    {
        magazine.Clear();
        if(PlayerIsInFrontOfBoss())
        {

            magazine.Add(projectiles[0]);
            launchSFX = projectileLaunchData[0];
        }
        else
        {
            if(Random.value<0.5f)
            {
                magazine.Add(projectiles[1]);
                launchSFX = projectileLaunchData[1];
            }
            else
            {
                for (int i = 2; i < projectiles.Length; i++)
                {
                    magazine.Add(projectiles[i]);
                }

                launchSFX = projectileLaunchData[2];
            }

        }

    }


    /// <summary>
    ///  ����Ƿ���Boss����ǰ��
    /// </summary>
    /// <returns></returns>
    bool PlayerIsInFrontOfBoss()
    {
        

        return Physics2D.OverlapBox(playerDetectionTransform.position, playerDetectionSize, 0, playerLayer);
    }

 

    /// <summary>
    /// ������ȴЭ��
    /// </summary>
    /// <returns></returns>
    IEnumerator BeamCooldownCoroutine()
    {
        yield return waitBeamCooldownTime;
        isBeamReady = true;
    }

    /// <summary>
    /// ׷�����Э��
    /// </summary>
    /// <returns></returns>
    IEnumerator ChasingPlayerCoroutine()
    {
        while(isActiveAndEnabled)
        {
            targetPosition.x = Viewport.Instance.MaxX - paddingX;
            targetPosition.y = playerTransform.position.y;

            yield return null;
        }

        
    }


    /// <summary>
    /// ���������
    /// </summary>
    void ActivateBeamWeapon()
    {

        isBeamReady = false;
        animator.SetTrigger(launchBeamHash);
        AudioManager.Instance.PlayRandomSFX(beamChargingSFX);
    }


    /// <summary>
    /// ���伤��ʱ
    /// </summary>
    void AnimationEvent_LaunchBeam()
    {

        AudioManager.Instance.PlayRandomSFX(beamLaunchSFX);

    }


    /// <summary>
    /// ֹͣ����ʱ
    /// </summary>
    void AnimationEvent_StopBeam()
    {
        StartCoroutine(nameof(BeamCooldownCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
        StopCoroutine(nameof(ChasingPlayerCoroutine));
    }











    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerDetectionTransform.position, playerDetectionSize);
    }


}
