using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZJ;
using UnityEngine.UI;

/// <summary>
/// 道具
/// </summary>
public class LootItem : MonoBehaviour
{
    [SerializeField] float minSpeed = 5f;
    [SerializeField] float maxSpeed = 15f;

    [SerializeField] protected AudioData defaultPickUpSFX;

    int pickUpStateID = Animator.StringToHash("PickUp");

    protected AudioData pickUpSFX;

    Animator animator;

    protected Player player;

    protected Text lootMessage;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();

        lootMessage = GetComponentInChildren<Text>(true);

        pickUpSFX = defaultPickUpSFX;
    }

    private void OnEnable()
    {
        StartCoroutine(MoveCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickUp();
 
    }

    /// <summary>
    /// 拾取战利品时
    /// </summary>
    protected virtual void PickUp()
    {
        StopAllCoroutines();
        animator.Play(pickUpStateID);
        AudioManager.Instance.PlayRandomSFX(pickUpSFX);
    }


    IEnumerator MoveCoroutine()
    {
        float speed = Random.Range(minSpeed, maxSpeed);

        Vector3 direction = Vector3.left;

        while(true)
        {
            if(player.isActiveAndEnabled)
            {
                direction = (player.transform.position - transform.position);
            }

            transform.Translate(direction * speed * Time.deltaTime);
            yield return null;
        }
    }

}
