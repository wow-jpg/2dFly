using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZJ
{
    public class Viewport : Singleton<Viewport>
    {
        float minX;
        float maxX;
        float minY;
        float maxY;
        float middleX;

        public float MaxX => maxX;
        // Start is called before the first frame update
        void Start()
        {
            Camera camera = Camera.main;
            Vector2 bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0));
            Vector2 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1));

            minX = bottomLeft.x;
            maxX = topRight.x;
            minY = bottomLeft.y;
            maxY = topRight.y;

            middleX = camera.ViewportToWorldPoint(new Vector3(0.5f, 0)).x;
        }

        
        /// <summary>
        /// 限制玩家的位置
        /// </summary>
        /// <param name="playerPosition"></param>
        /// <returns></returns>
        public Vector3 PlayerMoveablePosition(Vector3 playerPosition,float paddingX,float paddingY)
        {
            Vector3 position = Vector3.zero;

            position.x = Mathf.Clamp(playerPosition.x, minX+paddingX, maxX-paddingX);
            position.y = Mathf.Clamp(playerPosition.y, minY+paddingY, maxY-paddingY);

            return position;
        }

        /// <summary>
        /// 获得随机敌人生成位置
        /// </summary>
        /// <param name="paddingX"></param>
        /// <param name="paddingY"></param>
        /// <returns></returns>
        public Vector3 RandomEnemySpawnPosition(float paddingX,float paddingY)
        {
            Vector3 position = Vector3.zero;

            position.x = maxX + paddingX;
            position.y=Random.Range(minY+paddingY,maxY-paddingY);

            return position;
        }


        /// <summary>
        /// 限制敌人在右半边范围
        /// </summary>
        /// <param name="paddingX"></param>
        /// <param name="paddingY"></param>
        /// <returns></returns>
        public Vector3 RandomRightHalfPosition(float paddingX,float paddingY)
        {
            Vector3 position = Vector3.zero;

            position.x = Random.Range(middleX, maxX - paddingX);
            position.y = Random.Range(minY + paddingY, maxY - paddingY);

            return position;
        }


        /// <summary>
        /// 随机敌人移动位置
        /// </summary>
        /// <param name="paddingX"></param>
        /// <param name="paddingY"></param>
        /// <returns></returns>
        public Vector3 RandomEnemyMovePosition(float paddingX, float paddingY)
        {
            Vector3 position = Vector3.zero;

            position.x = Random.Range(minX+paddingX, maxX - paddingX);
            position.y = Random.Range(minY + paddingY, maxY - paddingY);

            return position;
        }
    }

}