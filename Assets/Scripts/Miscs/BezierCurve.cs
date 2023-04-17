using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������߹���
/// </summary>
public class BezierCurve : MonoBehaviour
{
    /// <summary>
    /// ���ض��α����������ϵĵ�
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    /// <param name="controlPoint"></param>
    /// <param name="by"></param>
    /// <returns></returns>
    public static Vector3 QuadraticPoint(Vector3 startPoint,Vector3 endPoint,Vector3 controlPoint,float by)
    {

        return Vector3.Lerp(
            Vector3.Lerp(startPoint, controlPoint, by),
            Vector3.Lerp(controlPoint, endPoint, by),
            by);

    }

    /// <summary>
    /// �������α����������ϵĵ�
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    /// <param name="controlPointStart"></param>
    /// <param name="controlPointEnd"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 CubicPoint(Vector3 startPoint,Vector3 endPoint
        ,Vector3 controlPointStart,Vector3 controlPointEnd,float t)
    {
        return QuadraticPoint(
            Vector3.Lerp(startPoint,controlPointStart,t),
            Vector3.Lerp(controlPointEnd, endPoint,t),
            Vector3.Lerp(controlPointStart,controlPointEnd,t),t);
    }
}
