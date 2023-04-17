using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateBar : MonoBehaviour
{
    [SerializeField] Image fillImageBack;
    [SerializeField] Image fillImageFront;
    [SerializeField] float fillSpeed = 0.1f;
    /// <summary>
    /// �Ƿ��ӳ����
    /// </summary>
    [SerializeField] bool delayFill = true;
    /// <summary>
    /// �ӳ����ʱ��
    /// </summary>
    [SerializeField] float fillDelay = 0.5f;
    float currentFillAmount;
   protected float targetFillAmount;

    Canvas canvas;
    float t;

    WaitForSeconds waitForDelayFill;

    Coroutine buffered;
    private void Awake()
    {
      //  canvas = GetComponent<Canvas>();
        if(TryGetComponent(out Canvas _canvas))
        {
            _canvas.worldCamera = Camera.main;
        }
     //   canvas.worldCamera = Camera.main;
        waitForDelayFill = new WaitForSeconds(fillDelay);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// ��ʼ��
    /// </summary>
    /// <param name="currentValue"></param>
    /// <param name="maxValue"></param>
    public virtual void Initialize(float currentValue,float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        targetFillAmount = currentFillAmount;
        fillImageBack.fillAmount = currentFillAmount;
        fillImageFront.fillAmount = currentFillAmount;
    }


    /// <summary>
    /// ����״̬
    /// </summary>
    /// <param name="currentValue"></param>
    /// <param name="maxValue"></param>
    public void UpdateState(float currentValue,float maxValue)
    {
        targetFillAmount = currentValue / maxValue;

        if (buffered !=null)
        {
            StopCoroutine(buffered);
        }



        //��״ֵ̬����ʱ
        if(currentFillAmount>targetFillAmount)
        {
            fillImageFront.fillAmount = targetFillAmount;

            buffered=StartCoroutine(BufferedFillingCoroutine(fillImageBack));
        
        }else
        //��״̬����ʱ
        if(currentFillAmount<targetFillAmount)
        {
            fillImageBack.fillAmount = targetFillAmount;

            buffered = StartCoroutine(BufferedFillingCoroutine(fillImageFront));

        }




    }

    /// <summary>
    /// �������Э��
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
   protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        if(delayFill)
        {
            yield return waitForDelayFill;
        }

        t = 0f;
 
        while (t<1f)
        {
            t += Time.deltaTime * fillSpeed;
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, t);
            image.fillAmount = currentFillAmount;
            yield return null;
        }
    }
}
