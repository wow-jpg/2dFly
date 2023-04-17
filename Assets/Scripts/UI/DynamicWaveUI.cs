using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ZJ;

public class DynamicWaveUI : MonoBehaviour
{
    /// <summary>
    /// ��������ʱ��
    /// </summary>
    [SerializeField] float animationTime = 1f;

    [Header("--- �ߵ��ƶ� ---")]
    [SerializeField] Vector2 lineTopStartPosition = new Vector2(-1250f, 75f);
    [SerializeField] Vector2 lineTopTargetPosition = new Vector2(0f, 75f);
    [SerializeField] Vector2 lineBottomStartPosition = new Vector2(1250f, -75f);
    [SerializeField] Vector2 lineBottomTargetPosition = new Vector2(0f, -75f);

    [Header("--- �����ƶ� ---")]
    [SerializeField] Vector2 waveTextStartScale = new Vector2(1f, 0f);
    [SerializeField] Vector2 waveTextTargetScale = Vector2.one;

    RectTransform lineTop;
    RectTransform lineBottom;
    RectTransform waveText;

    WaitForSeconds waitStayTime;

    private void Awake()
    {
        waitStayTime = new WaitForSeconds(EnemyManager.Instance.TimeBetweenWaves - animationTime * 2f);

        lineTop = transform.Find("LineTop").GetComponent<RectTransform>();
        lineBottom = transform.Find("LineBottom").GetComponent<RectTransform>();
        waveText = transform.Find("WaveText").GetComponent<RectTransform>();

        lineTop.localPosition = lineTopStartPosition;
        lineBottom.localPosition = lineBottomStartPosition;
        waveText.localPosition = waveTextStartScale;
    }

    private void OnEnable()
    {
        StartCoroutine(LineMoveCoroutine(lineTop, lineTopTargetPosition, lineTopStartPosition));
        StartCoroutine(LineMoveCoroutine(lineBottom, lineBottomTargetPosition, lineBottomStartPosition));
        StartCoroutine(TextScaleCoroutine(waveText, waveTextTargetScale,waveTextStartScale));
    }


    private void OnDisable()
    {
        //StartCoroutine(TextScaleCoroutine(waveText, waveTextStartScale));
        //StartCoroutine(LineMoveCoroutine(lineTop, lineTopStartPosition, lineTopTargetPosition));
        //StartCoroutine(LineMoveCoroutine(lineBottom, lineBottomStartPosition, lineBottomTargetPosition));
      
    }


    IEnumerator LineMoveCoroutine(RectTransform rect,Vector2 targetPosition,Vector2 startPosition)
    {
        yield return StartCoroutine(UIMoveCoroutine(rect,targetPosition));
        yield return waitStayTime;
        yield return StartCoroutine(UIMoveCoroutine(rect,startPosition));
    }

    /// <summary>
    /// UI�ƶ�Э��
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    IEnumerator UIMoveCoroutine(RectTransform rect,Vector2 position)
    {
        float t = 0;
    
        while (t<1f)
        {
            t += Time.deltaTime / animationTime;
            rect.localPosition = Vector2.Lerp(rect.localPosition, position, t);

            yield return null;
        }
    }


    IEnumerator TextScaleCoroutine(RectTransform rect,Vector2 targetScale,Vector2 startScale)
    {
        yield return StartCoroutine(UIScaleCoroutine(rect,targetScale));
        yield return waitStayTime;
        yield return StartCoroutine(UIScaleCoroutine(rect, startScale));
    }

    /// <summary>
    /// UI����Э��
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    IEnumerator UIScaleCoroutine(RectTransform rect, Vector2 scale)
    {
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime / animationTime;
            rect.localScale = Vector2.Lerp(rect.localScale, scale, t);

            yield return null;
        }
    }

}
