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
    /// 尺寸检查
    /// </summary>
    /// <param name="pools"></param>
    void CheckPoolSize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning("对象池实际容量大于对象池初始化容量! " +
                    "对象池实际容量:"
                    + pool.RuntimeSize + "   对象池初始化容量：" + pool.Size+
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
                Debug.LogError("发现相同的预制体" + pool.Prefab);
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
    /// 找出对象并返回
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("这人对象在对象池内没有找到" + prefab);
            return null;
        }

#endif
        return dictionary[prefab].PreparedObject();
    }

    /// <summary>
    /// 找出对象并返回并设置对象的位置
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("这个对象在对象池内没有找到" + prefab);
            return null;
        }

#endif
        return dictionary[prefab].PreparedObject(position);
    }

    /// <summary>
    /// 找出对象并返回并设置对象的位置、旋转
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("这个对象在对象池内没有找到" + prefab);
            return null;
        }

#endif
        return dictionary[prefab].PreparedObject(position, rotation);
    }

    /// <summary>
    /// 找出对象并返回并设置对象的位置、旋转、缩放
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("这人对象在对象池内没有找到" + prefab);
            return null;
        }

#endif
        return dictionary[prefab].PreparedObject(position, rotation, scale);
    }
}
