using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZJ;

public class EnemyController : MonoBehaviour
{
    [Header("---移动---")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float moveRotationAngle = 25f;

    [Header("---开火---")]
    /// <summary>
    /// 枪口特效 
    /// </summary>
    [SerializeField] protected ParticleSystem muzzleVFX;
    [SerializeField]
    protected AudioData[] projectileLaunchData;
    [SerializeField] protected GameObject[] projectiles;
    [SerializeField] protected Transform muzzle;

    [SerializeField] protected float minFireInterval;
    [SerializeField] protected float maxFireInterval;


    protected Vector3 targetPosition;

    protected float paddingX;
    protected float paddingY;


    protected virtual void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }

    protected virtual void Awake()
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);

        targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);

        while (gameObject.activeSelf)
        {
            if (Vector3.Distance(transform.position, targetPosition) > moveSpeed * Time.fixedDeltaTime)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.right);
            }
            else
            {
                targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
            }

            yield return new WaitForFixedUpdate();
        }
    }



    protected virtual IEnumerator RandomlyFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

            if (GameManager.GameState == GameState.GameOver)
            {
                yield break;
            }

            foreach (var item in projectiles)
            {
                PoolManager.Release(item, muzzle.position);
            }
            AudioManager.Instance.PlayRandomSFX(projectileLaunchData);
            muzzleVFX.Play();
        }

    }
}
