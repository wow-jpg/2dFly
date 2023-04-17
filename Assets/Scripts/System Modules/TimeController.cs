using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZJ;

/// <summary>
/// 游戏时间控制
/// </summary>
public class TimeController : Singleton<TimeController>
{
    [SerializeField,Range(0f,1f)] float bulletTimeScale = 0.1f;


    float defaultFixedDeltaTime;
    float timeScaleBeforePause;
    float t;
    protected override void Awake()
    {
        base.Awake();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void Pause()
    {
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;
  
    }

    public void Unpause()
    {
        Time.timeScale = timeScaleBeforePause;
     
    }


    /// <summary>
    /// 子弹时间
    /// </summary>
    public void BulletTime(float duration)
    {
        Time.timeScale = bulletTimeScale;
        
        StartCoroutine(SlowOutCoroutine(duration));
      
    }

    /// <summary>
    /// 子弹时间
    /// </summary>
    public void BulletTime(float inDuration,float outDuration)
    {
        Time.timeScale = bulletTimeScale;

      //  StartCoroutine(SlowOutCoroutine(duration));
     StartCoroutine(SlowInAndOutCoroutine(inDuration, outDuration));
    }


    /// <summary>
    /// 子弹时间
    /// </summary>
    public void BulletTime(float inDuration, float outDuration,float keepingDuration)
    {
        Time.timeScale = bulletTimeScale;

        //  StartCoroutine(SlowOutCoroutine(duration));
        StartCoroutine(SlowInKeepAndOutCoroutine(inDuration, outDuration,keepingDuration));
    }



    public void SlowIn(float duration)
    {
        StartCoroutine(SlowInCoroutine(duration));

    }

    public void SlowOut(float duration)
    {
        StartCoroutine(SlowOutCoroutine(duration));

    }

    IEnumerator SlowInAndOutCoroutine(float inDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));

        StartCoroutine(SlowOutCoroutine(outDuration));
    }

    
    IEnumerator SlowInKeepAndOutCoroutine(float inDuration,float outDuration,float keepingDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        yield return new WaitForSecondsRealtime(keepingDuration);
        StartCoroutine(SlowOutCoroutine(outDuration));
    }


    /// <summary>
    /// 进入子弹时间协程
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator SlowInCoroutine(float duration)
    {
        float startTime = Time.unscaledTime;
        t = 0f;
        while (t < 1f)
        {
            if(GameManager.GameState!=GameState.Paused)
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;

            }
            yield return null;
        }

     //   Debug.Log(" " + (Time.unscaledTime - startTime));
    }


    /// <summary>
    /// 退出子弹时间协程
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator SlowOutCoroutine(float duration)
    {
        float startTime = Time.unscaledTime;
       t = 0f;
        while(t<1f)
        {
            if (GameManager.GameState != GameState.Paused)
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;

            }
            yield return null;
        }

      //  Debug.Log(" " + (Time.unscaledTime - startTime));
    }
}
