using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] enemyPools;
    [SerializeField] Pool[] playerProjectilePools;
    [SerializeField] Pool[] enemyProjectilePools;
    [SerializeField] Pool[] vfxPools;
    [SerializeField] Pool[] lootItemPools;

    static Dictionary<GameObject, Pool> dictionary;
    void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();
        Initialize(enemyPools);
        Initialize(playerProjectilePools);
        Initialize(enemyProjectilePools);
        Initialize(vfxPools);
        Initialize(lootItemPools);
    }

#if UNITY_EDITOR
    private void OnDestroy()
    {
        CheckPoolSize(enemyPools);
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
        CheckPoolSize(vfxPools);
        CheckPoolSize(lootItemPools);
    }

#endif

    /// <summary>
    /// �ߴ���
    /// </summary>
    /// <param name="pools"></param>
    void CheckPoolSize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning("�����ʵ���������ڶ���س�ʼ������! " +
                    "�����ʵ������:"
                    + pool.RuntimeSize + "   ����س�ʼ��������" + pool.Size+
                    " "+pool.Prefab.name);
            }
        }
    }




    private void Initialize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
#if UNITY_EDITOR
            if (dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("������ͬ��Ԥ����" + pool.Prefab);
                continue;
            }
#endif

            dictionary.Add(pool.Prefab, pool);
            Transform poolParent = new GameObject("Pool:" + pool.Prefab.name).transform;
            poolParent.parent = transform;

            pool.Initialize(poolParent);

        }
    }



    /// <summary>
    /// �ҳ����󲢷���
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("���˶����ڶ������û���ҵ�" + prefab);
            return null;
        }

#endif
        return dictionary[prefab].PreparedObject();
    }

    /// <summary>
    /// �ҳ����󲢷��ز����ö����λ��
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("��������ڶ������û���ҵ�" + prefab);
            return null;
        }

#endif
        return dictionary[prefab].PreparedObject(position);
    }

    /// <summary>
    /// �ҳ����󲢷��ز����ö����λ�á���ת
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("��������ڶ������û���ҵ�" + prefab);
            return null;
        }

#endif
        return dictionary[prefab].PreparedObject(position, rotation);
    }

    /// <summary>
    /// �ҳ����󲢷��ز����ö����λ�á���ת������
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("���˶����ڶ������û���ҵ�" + prefab);
            return null;
        }

#endif
        return dictionary[prefab].PreparedObject(position, rotation, scale);
    }
}
