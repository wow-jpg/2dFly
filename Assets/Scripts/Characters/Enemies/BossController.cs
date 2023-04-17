using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZJ;

public class BossController : EnemyController
{
    [SerializeField] float continuousFireDuration = 1.5f;

    [Header("--- 检测玩家 ---")]
    [SerializeField] Transform playerDetectionTransform;
    [SerializeField] Vector3 playerDetectionSize;
    [SerializeField] LayerMask playerLayer;

    [Header("--- 激光 ---")]
    [SerializeField] float beamCooldownTime = 12f;
    [SerializeField] AudioData beamChargingSFX;
    [SerializeField] AudioData beamLaunchSFX;
    /// <summary>
    /// 激光是否冷却完毕
    /// </summary>
    bool isBeamReady;

    int launchBeamHash=Animator.StringToHash("launchBeam");

    WaitForSeconds waitForContinuousFireInterval;
    WaitForSeconds waitForFireInterval;
    WaitForSeconds waitBeamCooldownTime;


    /// <summary>
    /// 弹
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
    /// 装填子弹
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
    ///  玩家是否在Boss的正前方
    /// </summary>
    /// <returns></returns>
    bool PlayerIsInFrontOfBoss()
    {
        

        return Physics2D.OverlapBox(playerDetectionTransform.position, playerDetectionSize, 0, playerLayer);
    }

 

    /// <summary>
    /// 激光冷却协程
    /// </summary>
    /// <returns></returns>
    IEnumerator BeamCooldownCoroutine()
    {
        yield return waitBeamCooldownTime;
        isBeamReady = true;
    }

    /// <summary>
    /// 追逐玩家协程
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
    /// 激活激光武器
    /// </summary>
    void ActivateBeamWeapon()
    {

        isBeamReady = false;
        animator.SetTrigger(launchBeamHash);
        AudioManager.Instance.PlayRandomSFX(beamChargingSFX);
    }


    /// <summary>
    /// 发射激光时
    /// </summary>
    void AnimationEvent_LaunchBeam()
    {

        AudioManager.Instance.PlayRandomSFX(beamLaunchSFX);

    }


    /// <summary>
    /// 停止激光时
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
