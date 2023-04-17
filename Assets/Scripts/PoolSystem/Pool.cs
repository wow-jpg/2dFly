using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool 
{
    public GameObject Prefab
    {
        get
        {
            return prefab;
        }
    }

    public int Size => size;
    /// <summary>
    /// 运行时对象池大小 
    /// </summary>
    public int RuntimeSize=>queue.Count;

    [SerializeField] GameObject prefab;
    [SerializeField] int size = 1;

    Queue<GameObject> queue;

    Transform parent;
    public void Initialize(Transform _parent)
    {
        queue = new Queue<GameObject>();
        parent = _parent;
        for (int i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());
        }
    }



    GameObject Copy()
    {
        var copy=GameObject.Instantiate(prefab,parent);
        copy.SetActive(false);

        return copy;
    }


    /// <summary>
    /// 从对象池中拿取
    /// </summary>
    /// <returns></returns>
    GameObject AvailableObject()
    {
        GameObject availableObject = null;
         
        if(queue.Count > 0&&!queue.Peek().activeSelf)
        {
            availableObject = queue.Dequeue();
        }
        else
        {
            availableObject = Copy();
        }
        queue.Enqueue(availableObject);
        return availableObject;
    }





    /// <summary>
    /// 从对象池中拿取并激活
    /// </summary>
    /// <returns></returns>
    public GameObject PreparedObject()
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        return preparedObject;
    }



    /// <summary>
    /// 从对象池中拿取并激活
    /// </summary>
    /// <returns></returns>
    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        preparedObject.transform.position = position;

        return preparedObject;
    }

    /// <summary>
    /// 从对象池中拿取并激活
    /// </summary>
    /// <returns></returns>
    public GameObject PreparedObject(Vector3 position,Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;

        return preparedObject;
    }


    /// <summary>
    /// 从对象池中拿取并激活
    /// </summary>
    /// <returns></returns>
    public GameObject PreparedObject(Vector3 position, Quaternion rotation,Vector3 localScale)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;
        return preparedObject;
    }
}
