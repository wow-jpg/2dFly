using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ZJ
{
    public class EnemyManager : Singleton<EnemyManager>
    {
        public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];

        public int WaveNumber
        {
            get { return waveNumber; }
        }
        public float TimeBetweenWaves=>timeBetweenWaves;
        /// <summary>
        /// 波数UI显示
        /// </summary>
        [SerializeField] GameObject waveUI;
        [SerializeField] bool spawnEnemy = true;
        [SerializeField] GameObject[] enemyPrefabs;

        [SerializeField] float timeBetweenSpawns = 1f;
        [SerializeField,Tooltip("波数时间间隔")] float timeBetweenWaves = 1f;

        [SerializeField] int minEnemyAmount = 4;
        [SerializeField] int maxEnemyAmount = 10;

        [Header("--- Boss波数 ---")]
        [SerializeField] GameObject bossPrefab;
        [SerializeField] int bossWaveNumber;

        /// <summary>
        /// 敌人波数
        /// </summary>
        int waveNumber = 1;
        int enemyAmount;

        /// <summary>
        /// 当前场景中有多少人敌人
        /// </summary>
        List<GameObject> enemyList;
        WaitForSeconds waitTimeBetweenWaves;
        WaitForSeconds waitTimeBetweenSpawns;
        WaitUntil waitUntilNoEnemy;
        protected override void Awake()
        {
            base.Awake();
            enemyList = new List<GameObject>();
            waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
            waitTimeBetweenWaves=new WaitForSeconds(timeBetweenWaves);
            waitUntilNoEnemy = new WaitUntil(()=>enemyList.Count==0);
        }



        IEnumerator Start()
        {
            while(spawnEnemy&&GameManager.GameState!=GameState.GameOver)
            {
           
                waveUI.SetActive(true);
                yield return waitTimeBetweenWaves;
                waveUI.SetActive(false);
                yield return StartCoroutine(RandomlySpawnCoroutine());

            }

        }

        IEnumerator RandomlySpawnCoroutine()
        {
            if(waveNumber%bossWaveNumber==0)
            {
                var boss = PoolManager.Release(bossPrefab);
                enemyList.Add(boss);

            }
            else
            {
                enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber /bossWaveNumber, maxEnemyAmount);


                for (int i = 0; i < enemyAmount; i++)
                {
                    enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));

                    yield return waitTimeBetweenSpawns;
                }
            }




            yield return waitUntilNoEnemy;
            waveNumber++;
          
        }

        public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);



    }
}