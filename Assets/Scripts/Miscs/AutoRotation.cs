using UnityEngine;

/// <summary>
/// �Զ���ת
/// </summary>
public class AutoRotation : MonoBehaviour
{
    [SerializeField] float speed = 360f;
    [SerializeField] Vector3 angle;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(angle * speed * Time.deltaTime);
    }
}
