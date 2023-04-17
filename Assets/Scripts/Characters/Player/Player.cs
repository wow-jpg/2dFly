using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using ZJ.Input;

namespace ZJ
{


    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Character
    {
        public bool IsFullHealth => health == maxHealth;
        public bool IsFullPower => weaponPower == 2;


        [SerializeField] StateBar_HUD statebar_HUD;
        [SerializeField] bool regenerteHealth = true;
        [SerializeField] float healthRegenerateTime;
        [SerializeField] float healthRegeneratePercent;


        [Header("����")]
        [SerializeField] PlayerInput input;
        /// <summary>
        /// �ƶ��ٶ�
        /// </summary>
        [SerializeField] float moveSpeed = 10f;

        /// <summary>
        /// ����ʱ��
        /// </summary>
        [SerializeField] float accelerationTime = 3f;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        [SerializeField] float decelerationTime = 3f;

        /// <summary>
        /// ��ת�Ƕ�
        /// </summary>
        [SerializeField] float moveRotationAngle = 45f;

        float paddingX = 0.2f;
        float paddingY = 0.2f;

        [Header("����")]
        /// <summary>
        /// �ӵ�
        /// </summary>
        [SerializeField] GameObject projectile1;
        /// <summary>
        /// �ӵ�
        /// </summary>
        [SerializeField] GameObject projectile2;
        /// <summary>
        /// �ӵ�
        /// </summary>
        [SerializeField] GameObject projectile3;
        /// <summary>
        /// ��������ʱ�ӵ�
        /// </summary>
        [SerializeField] GameObject projectileOverdrive;
        /// <summary>
        /// ������Ч
        /// </summary>
        [SerializeField] AudioData proectileSFX;


        /// <summary>
        /// ǹ��λ��
        /// </summary>
        [SerializeField] Transform muzzleMiddle;
        /// <summary>
        /// ǹ��λ��
        /// </summary>
        [SerializeField] Transform muzzleTop;
        /// <summary>
        /// ǹ��λ��
        /// </summary>
        [SerializeField] Transform muzzleBottom;
        /// <summary>
        /// ǹ����Ч 
        /// </summary>
        [SerializeField] ParticleSystem muzzleVFX;
        [SerializeField, Range(0, 2)] int weaponPower = 0;

        /// <summary>
        /// ������
        /// </summary>
        [SerializeField] float fireInerval = 0.2f;

        [Header("---����---")]
        [SerializeField] AudioData dodgeSFX;
        /// <summary>
        /// ��������ֵ
        /// </summary>
        [SerializeField, Range(0, 100)] int dodgeEnergyCost = 25;
        [SerializeField] float maxRoll = 720f;
        [SerializeField] float rollSpeed = 360f;
        [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);
        float currentRoll;

        /// <summary>
        /// ����������
        /// </summary>
        bool isDodging = false;
        /// <summary>
        /// �Ƿ�������������
        /// </summary>
        bool isOverdriving = false;
        [Header("---��������---")]
        [SerializeField] int overdriveDodgeFactor = 2;
        [SerializeField] float overdriveSpeedFactor = 1.2f;
        [SerializeField] float overdriveFireFactor = 1.2f;


        float dodgeDuration;
        /// <summary>
        /// ʱ���������ʱ��
        /// </summary>
        readonly float slowMotionDuration = 1f;
        /// <summary>
        /// �޵�ʱ��
        /// </summary>
        readonly float InvincibleTime = 1f;


        WaitForSeconds waitForFireInterval;
        WaitForSeconds waitHealthRegeerateTime;
        WaitForSeconds waitForOverdriveFireInterval;
        WaitForSeconds waitDecelerationTime;
        WaitForSeconds waitInvincibleTime;

        Coroutine moveCoroutine;
        Coroutine healthRegenerateCoroutine;

        Rigidbody2D rigid;

        Collider2D collider2d;

        MissileSystem missile;

        protected override void OnEnable()
        {
            base.OnEnable();
            input.onMove += Move;
            input.onStopMove += StopMove;
            input.onFire += Fire;
            input.onStopFire += StopFire;
            input.onDodge += Dodge;
            input.onOverdrive += Overdrive;
            input.onLaunchMissile += LaunchMissile;

            PlayerOverdrive.on += OverdriveOn;
            PlayerOverdrive.off += OverdriveOff;
        }

    

        private void OnDisable()
        {
            input.onMove -= Move;
            input.onStopMove -= StopMove;
            input.onFire -= Fire;
            input.onStopFire -= StopFire;
            input.onDodge -= Dodge;
            input.onOverdrive -= Overdrive;
            input.onLaunchMissile -= LaunchMissile;

            PlayerOverdrive.on -= OverdriveOn;
            PlayerOverdrive.off -= OverdriveOff;
        }

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();

            collider2d = GetComponent<Collider2D>();
            missile=GetComponent<MissileSystem>();

            var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
            paddingX = size.x / 2f;
            paddingY = size.y / 2f;
        }

        void Start()
        {
            rigid.gravityScale = 0f;

            waitForFireInterval = new WaitForSeconds(fireInerval);
            waitHealthRegeerateTime = new WaitForSeconds(healthRegenerateTime);
            waitForOverdriveFireInterval = new WaitForSeconds(fireInerval / overdriveFireFactor);
            waitDecelerationTime = new WaitForSeconds(decelerationTime);
            waitInvincibleTime = new WaitForSeconds(InvincibleTime);

            statebar_HUD.Initialize(health, maxHealth);

            input.EnableGamePlayInput();

            dodgeDuration = maxRoll / 2f;


            TakeDamage(500f);//DEBUG
        }


        void Update()
        {


        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            PowerDown();
            statebar_HUD.UpdateState(health, maxHealth);

            if (gameObject.activeSelf)
            {
                StartCoroutine(nameof(InvincibleCoroutine));

                if (regenerteHealth)
                {
                    if (healthRegenerateCoroutine != null)
                    {
                        StopCoroutine(healthRegenerateCoroutine);
                    }
                    healthRegenerateCoroutine = StartCoroutine(HealthRegenerateCoroutine(waitHealthRegeerateTime, healthRegeneratePercent));
                }
            }
        }

        public override void RestoreHealth(float value)
        {
            base.RestoreHealth(value);
            statebar_HUD.UpdateState(health, maxHealth);
        }

        public override void Die()
        {
            GameManager.onGameOver?.Invoke();
            GameManager.GameState = GameState.GameOver;
            statebar_HUD.UpdateState(0f, maxHealth);
            
            base.Die();
        }

        /// <summary>
        /// �޵�Э��
        /// </summary>
        /// <returns></returns>
        IEnumerator InvincibleCoroutine()
        {
            collider2d.isTrigger = true;

            yield return waitInvincibleTime;

            collider2d.isTrigger = false;
        }

        #region MOVE

        private void Move(Vector2 moveInput)
        {

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);
            moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveInput.normalized * moveSpeed
                , moveRotation));
            StopCoroutine(nameof(DecelerationCoroutine));
            StartCoroutine(nameof(MoveRangeLimitCoroutine));
        }
        private void StopMove()
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
            //StopCoroutine(nameof(MoveRangeLimitCoroutine));
            StartCoroutine(nameof(DecelerationCoroutine));
        }

        /// <summary>
        /// �������ٻ����Э��
        /// </summary>
        /// <param name="moveVelocity"></param>
        /// <returns></returns>
        IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion rotationAngle)
        {
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / time;
                rigid.velocity = Vector2.Lerp(rigid.velocity, moveVelocity, t);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotationAngle, t);
                yield return null;
            }
        }

        /// <summary>
        /// ����λ��Э��
        /// </summary>
        /// <returns></returns>
        IEnumerator MoveRangeLimitCoroutine()
        {
            while (true)
            {
                transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position, paddingX, paddingY);

                yield return null;
            }
        }

        IEnumerator DecelerationCoroutine()
        {
            yield return waitDecelerationTime;

            StopCoroutine(nameof(MoveRangeLimitCoroutine));
        }

        #endregion

        #region FIRE

        private void Fire()
        {
            muzzleVFX.Play();
            StartCoroutine(nameof(FireCoroutine));
        }

        private void StopFire()
        {
            muzzleVFX.Stop();
            StopCoroutine(nameof(FireCoroutine));
        }

        /// <summary>
        /// ����Э��
        /// </summary>
        /// <returns></returns>
        IEnumerator FireCoroutine()
        {

            while (true)
            {
                yield return waitForFireInterval;
                switch (weaponPower)
                {
                    case 0:
                        PoolManager.Release(Projectile(projectile1), muzzleMiddle.position);
                        break;
                    case 1:
                        PoolManager.Release(Projectile(projectile1), muzzleMiddle.position);
                        PoolManager.Release(Projectile(projectile2), muzzleTop.position);
                        break;
                    case 2:
                        PoolManager.Release(Projectile(projectile1), muzzleMiddle.position);
                        PoolManager.Release(Projectile(projectile2), muzzleTop.position);
                        PoolManager.Release(Projectile(projectile3), muzzleBottom.position);
                        break;
                    default:
                        break;
                }


                AudioManager.Instance.PlayRandomSFX(proectileSFX);

                if (isOverdriving)
                {
                    yield return waitForOverdriveFireInterval;
                }
                else
                {
                    yield return waitForFireInterval;
                }
            }
        }

        /// <summary>
        /// �ж�ʲô״̬ʹ��ʲô�ӵ�
        /// </summary>
        /// <returns></returns>
        GameObject Projectile(GameObject _go)
        {
            return isOverdriving ? projectileOverdrive : _go;
        }


        #endregion

        #region ����

        private void Dodge()
        {

            if (isDodging || !PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;
            StartCoroutine(nameof(DodgeCoroutine));
         //   TimeController.Instance.BulletTime(slowMotionDuration/2f, slowMotionDuration/2f);
        }

        /// <summary>
        /// ����Э��
        /// </summary>
        /// <returns></returns>
        IEnumerator DodgeCoroutine()
        {
            AudioManager.Instance.PlayRandomSFX(dodgeSFX);
            PlayerEnergy.Instance.Use(dodgeEnergyCost);
            collider2d.isTrigger = true;
            isDodging = true;
            currentRoll = 0f;

            //    var scale = transform.localScale;

            while (currentRoll < maxRoll)
            {
                currentRoll += rollSpeed * Time.deltaTime;
                transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);

                //if(currentRoll<maxRoll/2f)
                //{
                //    //scale -= Time.deltaTime / dodgeDuration * Vector3.one;
                //    scale.x = Mathf.Clamp(scale.x - Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
                //    scale.y = Mathf.Clamp(scale.y - Time.deltaTime / dodgeDuration, dodgeScale.y, 1f);
                //    scale.z = Mathf.Clamp(scale.z - Time.deltaTime / dodgeDuration, dodgeScale.z, 1f);

                //}
                //else
                //{
                //    scale.x = Mathf.Clamp(scale.x + Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
                //    scale.y = Mathf.Clamp(scale.y + Time.deltaTime / dodgeDuration, dodgeScale.y, 1f);
                //    scale.z = Mathf.Clamp(scale.z + Time.deltaTime / dodgeDuration, dodgeScale.z, 1f);

                //    //scale += Time.deltaTime / dodgeDuration * Vector3.one;
                //}

                transform.localScale = BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeScale, currentRoll / maxRoll);

                //     transform.localScale = scale;

                yield return null;
            }


            collider2d.isTrigger = false;
            isDodging = false;
        }





        #endregion

        #region ����

        private void Overdrive()
        {
            if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX))
                return;

            PlayerOverdrive.on?.Invoke();

        }


        private void OverdriveOn()
        {
            isOverdriving = true;
            dodgeEnergyCost *= overdriveDodgeFactor;
            moveSpeed *= overdriveSpeedFactor;
            TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration, 2f);
        }

        private void OverdriveOff()
        {
            isOverdriving = false;
            dodgeEnergyCost /= overdriveDodgeFactor;
            moveSpeed /= overdriveSpeedFactor;
        }




        #endregion
    
        /// <summary>
        /// ���䵼��
        /// </summary>
        private void LaunchMissile()
        {
            missile.Launch(muzzleMiddle);
        }


        public void PickUpMissile()
        {
            missile.PickUp();
        }



        public void PowerUp()
        {
            //  weaponPower++;
            weaponPower = Mathf.Min(weaponPower + 1, 2);
        }


        void PowerDown()
        {
            weaponPower = Mathf.Max(--weaponPower, 0);
        }
    }

}